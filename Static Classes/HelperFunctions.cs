using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WpfRequestResponseLogger.Data;
using WpfRequestResponseLogger.Models;

namespace WpfRequestResponseLogger.Static_Classes
{
    internal class HelperFunctions
    {
        public static void AddAlertInstance( AlertInstance ai)
        {
            using (var context = new DataContext())
            {
                if (ai.ClientIP != null)
                {
                    // Check if the IP address is in the whitelist IP list ans is active
                    bool isWhitelisted = context.WhitelistedIps
                        .Any(whitelisted => whitelisted.IpAddress == ai.ClientIP && whitelisted.IsActive);

                    // If the IP address is in the whitelisted IP list do not add an alert instance
                    if (isWhitelisted)
                    {
                        return;
                    }
                }


                // Check if the alert is silenced
                bool isSilenced = context.AlertSilenced
                    .Any(silenced => silenced.AlertRuleId == ai.AlertRuleId &&
                                     silenced.SilencedFromUTC < ai.TriggeredAtUTC &&
                                     silenced.SilencedUntilUTC > ai.TriggeredAtUTC);

                // If the alert is silenced do not add the alert instance
                if (isSilenced)
                {
                    return;
                }


                // Check if exists a register with the same ServerId, AlertRuleId, AlertTypeId and Status
                bool exists = false;
                if (ai.AlertTypeId < 5 || ai.AlertTypeId > 8)
                {
                    exists = context.AlertInstances
                       .Any(existingInstance => existingInstance.ServerId == ai.ServerId &&
                                                existingInstance.AlertRuleId == ai.AlertRuleId &&
                                                existingInstance.AlertTypeId == ai.AlertTypeId &&
                                                existingInstance.Status == ai.Status);
                }
                
                if (ai.AlertTypeId >=5 && ai.AlertTypeId <=8)       // could ckeck also path if AlertTypeId = 5,6,7,8
                {                                                   // but then it will create an alarm instance for every endpoint
                    exists = context.AlertInstances                 // this could be a problem in a fuzzing attack -> thousands of registers and emails
                        .Any(existingInstance => existingInstance.ServerId == ai.ServerId &&
                                                 existingInstance.AlertRuleId == ai.AlertRuleId &&
                                                 existingInstance.AlertTypeId == ai.AlertTypeId &&
                                                 existingInstance.Status == ai.Status);
                                                 //existingInstance.EndpointPath == ai.EndpointPath);   


                }
                // If it does not exist add the new register
                if (!exists)
                {
                    context.AlertInstances.Add(ai);
                    context.SaveChanges();
                    
                    // Obtain the Id of the newly added entity
                    //int newAlertInstanceId = ai.Id;  // The Id will be automatically updated after SaveChanges()
                    
                    CreateNotificationsForAlertInstance(ai);
                }
            }

        }

        public static async void CreateNotificationsForAlertInstance(AlertInstance ai)
        {
            using (var context = new DataContext())
            {
                // Get all the recipients associated with the AlertRuleId
                var recipients = context.AlertRuleRecipients
                    .Where(r => r.AlertRuleId == ai.AlertRuleId)
                    .Join(context.AlertRecipients,
                          ruleRecipient => ruleRecipient.RecipientId,
                          recipient => recipient.Id,
                          (ruleRecipient, recipient) => new
                          {
                              recipient.Recipient,
                              recipient.Email
                          })
                    .ToList();

                // For each recipient, create a new notification in the AlertNotifications table
                foreach (var recipient in recipients)
                {
                    var alertNotification = new AlertNotification
                    {
                        AlertInstanceId = ai.Id,
                        NotificationType = "Email",
                        Recipient = recipient.Recipient,
                        SentAtUTC = DateTime.UtcNow,
                        AlertRuleId = ai.AlertRuleId ?? 0,
                        AlertRuleName = ai.AlertRuleName,
                        AlertTypeId = ai.AlertTypeId,
                        AlertTypeName = ai.AlertTypeName,
                        AlertValue= ai.AlertValue,
                        Severity = ai.Severity ?? string.Empty,  // If ai.Severity is null, assign an empty string
                        ServerId = ai.ServerId,
                        ServerName = ai.ServerName,
                        ClientIP = ai.ClientIP ?? string.Empty,
                        EndpointId = ai.EndpointId ?? 0,
                        EndpointPath = ai.EndpointPath ?? string.Empty,
                        TriggeredAtUTC = ai.TriggeredAtUTC,
                        Status= ai.Status,

                    };

                    context.AlertNotifications.Add(alertNotification);
                    string emailAddress = recipient.Email;
                    try
                    {
                        await EmailService.SendAlertEmailAsync(ai, emailAddress);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error sending the email: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }

                // Save all notifications to the database
                context.SaveChanges();
            }
        }


    }

}

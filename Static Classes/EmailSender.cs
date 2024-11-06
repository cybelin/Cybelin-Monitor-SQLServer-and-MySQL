using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using WpfRequestResponseLogger.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfRequestResponseLogger.Data;
using System.Text.RegularExpressions;



namespace WpfRequestResponseLogger.Static_Classes
{
    

    public static class EmailService
    {
        public static async Task SendAlertEmailAsync(AlertInstance alertInstance, string emailAddress)
        {
            //Validate the email address
            bool emailValid = IsValidEmail(emailAddress);
            if (!emailValid)
            {
                return;
            }

            // Configuration of the SMTP Server 
            EmailConfiguration emailConfig;
            using (var context = new DataContext())
            {
                var emailConfigService = new EmailConfigService(context);
                emailConfig = emailConfigService.LoadEmailConfiguration();
            }
            
            var smtpClient = new SmtpClient(emailConfig.EmailSmtpClient)
            {
                Port = emailConfig.EmailPort,
                UseDefaultCredentials = emailConfig.EmailUseDefaultCredentials,
                Credentials = new NetworkCredential(emailConfig.EmailCredentialName,emailConfig.EmailCredentialPassword),
                EnableSsl = emailConfig.EmailEnableSsl
            };

            // Build the message of the email
            var mailMessage = new MailMessage
            {
                From = new MailAddress(emailConfig.EmailFrom),
                Subject = $"Alert: {alertInstance.AlertTypeName}",
                Body = BuildEmailBody(alertInstance),
                IsBodyHtml = emailConfig.EmailIsBodyHtml // Change the configuration register to true true if you prefer HTML format
            };

            // Add email recipient
            mailMessage.To.Add(emailAddress);

            // Send the email
            await smtpClient.SendMailAsync(mailMessage);




        }

        private static string BuildEmailBody(AlertInstance alertInstance)
        {
           
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"ID of the Alert Instance: {alertInstance.Id}");
            sb.AppendLine($"ID of the Alert Rule: {alertInstance.AlertRuleId}");
            sb.AppendLine($"Name of the Alert: {alertInstance.AlertRuleName}");
            sb.AppendLine($"Descripción: {alertInstance.AlertTypeName}");
            sb.AppendLine($"Alert Value: {alertInstance.AlertValue}");
            sb.AppendLine($"Server ID: {alertInstance.ServerId}");
            sb.AppendLine($"Server Name: {alertInstance.ServerName}");
            sb.AppendLine($"Client IP: {alertInstance.ClientIP}");
            sb.AppendLine($"Endpoint Path: {alertInstance.EndpointPath}");
            sb.AppendLine($"Triggered at UTC: {alertInstance.TriggeredAtUTC}");
            sb.AppendLine($"Severity: {alertInstance.Severity}");
            sb.AppendLine($"Status: {alertInstance.Status}");
           
            return sb.ToString();
        }

        

        public static bool IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            // Regular expression to validate the email
            string pattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
            Regex regex = new Regex(pattern);

            return regex.IsMatch(email);
        }
}



public class EmailConfiguration
    {
        public string EmailSmtpClient { get; set; }
        public int EmailPort { get; set; }
        public bool EmailUseDefaultCredentials { get; set; }
        public string EmailCredentialName { get; set; }
        public string EmailCredentialPassword { get; set; }
        public bool EmailEnableSsl { get; set; }
        public string EmailFrom { get; set; }
        public bool EmailIsBodyHtml { get; set; }
    }

    public class EmailConfigService
    {
        private readonly DataContext _context;

        public EmailConfigService(DataContext context)
        {
            _context = context;
        }

        public EmailConfiguration LoadEmailConfiguration()
        {
            var configurations = _context.MonitorConfigurations.ToList();

            var emailConfig = new EmailConfiguration
            {
                EmailSmtpClient = configurations.FirstOrDefault(c => c.Key == "Email SmtpClient")?.Value,
                EmailPort = int.Parse(configurations.FirstOrDefault(c => c.Key == "Email Port")?.Value ?? "0"),
                EmailUseDefaultCredentials = bool.Parse(configurations.FirstOrDefault(c => c.Key == "Email UseDefaultCredentials")?.Value ?? "false"),
                EmailCredentialName = configurations.FirstOrDefault(c => c.Key == "Email Credential Name")?.Value,
                EmailCredentialPassword = configurations.FirstOrDefault(c => c.Key == "Email Credential Password")?.Value,
                EmailEnableSsl = bool.Parse(configurations.FirstOrDefault(c => c.Key == "Email EnableSsl")?.Value ?? "false"),
                EmailFrom = configurations.FirstOrDefault(c => c.Key == "Email From")?.Value,
                EmailIsBodyHtml = bool.Parse(configurations.FirstOrDefault(c => c.Key == "Email IsBodyHtml")?.Value ?? "false")
            };

            return emailConfig;
        }
    }


}

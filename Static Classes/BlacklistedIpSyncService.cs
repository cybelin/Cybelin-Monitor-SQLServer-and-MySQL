using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using WpfRequestResponseLogger.Data;
using WpfRequestResponseLogger.Models;


namespace WpfRequestResponseLogger
{


    public class BlacklistedIpSyncService
    {

        public static void SyncBlacklistedIps()
        {
            DataContext sourceContext = new DataContext();
            var sourceBlacklistedIps = sourceContext.MonitorBlacklistedIps.ToList();   // Obtain all the blacklisted IPs from the monitoring database
         
            
            // Creates a destination list 
            List<MonitorBlacklistedIp> blacklistedIps = new List<MonitorBlacklistedIp>();

            // Adds the blacklisted IPs to the destination list
            foreach (var sourceIp in sourceBlacklistedIps)
            {
                
                MonitorBlacklistedIp blacklistedIp = new MonitorBlacklistedIp
                {
                    IpAddress = sourceIp.IpAddress,
                    DateAdded = sourceIp.DateAdded,
                    Reason = sourceIp.Reason,
                    IsActive = sourceIp.IsActive
                };

                blacklistedIps.Add(blacklistedIp);
            }


            // Obtain a list of servers to synchronize
            var servers = sourceContext.Servers.ToList();


            // Syncrhonize each server
            foreach (var server in servers)
            {
                
                using (var destinationContext = new DataContext2(server.ConnectionString, server.ServerType))
                {
                    SyncWithServer(destinationContext, blacklistedIps);
                }

            }
        }

        private static void SyncWithServer(DataContext2 destinationContext, List<MonitorBlacklistedIp> sourceBlacklistedIps)
        {
            using (var transaction = destinationContext.Database.BeginTransaction())
            {
                try
                {
                    // Obtain all the blacklisted IPs from the destination server.
                    var destinationBlacklistedIps = destinationContext.BlacklistedIps.ToList();
                    var destinationIpDict = destinationBlacklistedIps.ToDictionary(d => d.IpAddress); // Crear diccionario para búsquedas rápidas

                    // Insert new IPs and update existing IPs in the destination server
                    foreach (var sourceIp in sourceBlacklistedIps)
                    {
                        if (destinationIpDict.TryGetValue(sourceIp.IpAddress, out var destinationIp))
                        {
                            // If the IP exists, update if there are changes
                            if (destinationIp.DateAdded != sourceIp.DateAdded ||
                                destinationIp.Reason != sourceIp.Reason ||
                                destinationIp.IsActive != sourceIp.IsActive)
                            {
                                destinationIp.DateAdded = sourceIp.DateAdded;
                                destinationIp.Reason = sourceIp.Reason;
                                destinationIp.IsActive = sourceIp.IsActive;

                                // Marc the state as modified
                                destinationContext.Entry(destinationIp).State = EntityState.Modified;
                            }
                        }
                        else
                        {
                            // If the IP does not exist then add the new IP
                            destinationContext.BlacklistedIps.Add(new BlacklistedIp
                            {
                                IpAddress = sourceIp.IpAddress,
                                DateAdded = sourceIp.DateAdded,
                                Reason = sourceIp.Reason,
                                IsActive = sourceIp.IsActive
                            });
                        }
                    }

                    // Eliminate IPs that are not in central database of IPs
                    foreach (var destinationIp in destinationBlacklistedIps)
                    {
                        if (!sourceBlacklistedIps.Any(s => s.IpAddress == destinationIp.IpAddress))
                        {
                            destinationContext.BlacklistedIps.Remove(destinationIp);
                        }
                    }

                    // Save changes to the database.
                    destinationContext.SaveChanges();

                    // Confirm the transaction
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    // If there an error then roolback
                    transaction.Rollback();
                    Console.WriteLine($"Error durante la sincronización: {ex.Message}");
                    throw;
                }
            }
        }



    }

}

using Microsoft.EntityFrameworkCore;
using System;
using WpfRequestResponseLogger.Models;

namespace WpfRequestResponseLogger.Data
{
    public class DataContext : DbContext
    {
        public DbSet<RequestLog> RequestLogs { get; set; }
        public DbSet<ResponseLog> ResponseLogs { get; set; }

        public DbSet<BlacklistedIp> MonitorBlacklistedIps { get; set; }
        public DbSet<WhitelistedIp> WhitelistedIps { get; set; }

        public DbSet<Configuration> MonitorConfigurations { get; set; }
        public DbSet<Server> Servers { get; set; } 

        public DbSet<AlertInstance> AlertInstances { get; set; }
        
        public DbSet<AlertNotification> AlertNotifications { get; set; }
        public DbSet<AlertRecipient> AlertRecipients { get; set; }
        public DbSet<AlertRuleRecipient> AlertRuleRecipients { get; set; }
        public DbSet<AlertRule> AlertRules { get; set; }
        public DbSet<AlertSilenced> AlertSilenced { get; set; }
        public DbSet<AlertRuleRecipientDetail> AlertRuleRecipientDetails { get; set; }

        public DbSet<AlertRuleEndpointDetails> AlertRuleEndpointDetails { get; set; }
        public DbSet<AlertType> AlertTypes { get; set; }

        public DbSet<AlertEndpoint> AlertEndpoints { get; set; }
        

        public DbSet<AlertRuleEndpoint> AlertRulesEndpoints { get; set; }
        public DbSet<AlertRulesServer> AlertRulesServers { get; set; }
        public DbSet<AlertRulesServerDetails> AlertRulesServerDetails { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // Use your connection string here
            optionsBuilder.UseSqlServer("Server=PCALVARO;Database=Cybelin;Integrated Security=False;User ID=YourUserName;Password=YourPassword;TrustServerCertificate=True;Trusted_Connection=True;MultipleActiveResultSets=true");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Indicate that AlertRuleRecipientDetail kas no key
            modelBuilder.Entity<AlertRuleRecipientDetail>()
                        .HasNoKey();     // Define AlertRuleEndpointDetails as a keyless entity
            modelBuilder.Entity<AlertRuleEndpointDetails>().HasNoKey();

            base.OnModelCreating(modelBuilder);
        }

        


    }
    public class AlertRuleRecipientDetail
    {
        public int AlertRuleRecipientId { get; set; }
        public string AlertRuleName { get; set; }
        public string RecipientName { get; set; }
        public int AlertRuleId { get; set; }
        public int RecipientId { get; set; }
    }
    public class AlertRuleEndpointDetails
    {
        public int AlertRuleEndpointId { get; set; }
        public int AlertRuleId { get; set; }
        public int AlertEndpointId { get; set; }
        public string AlertRuleName { get; set; }
        public string AlertEndpointPath { get; set; }
    }

    public class AlertRulesServerDetails
    {
        public int Id { get; set; } 
        public int AlertRuleId { get; set; } 
        public int ServerId { get; set; } 
        public string AlertRuleName { get; set; }
        public string ServerName { get; set; }

    }

    public class MonitorBlacklistedIp
    {
        public int Id { get; set; }
        public string IpAddress { get; set; }
        public DateTime DateAdded { get; set; }
        public string Reason { get; set; }
        public bool IsActive { get; set; }
    }

}

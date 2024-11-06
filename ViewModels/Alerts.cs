using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;

namespace WpfRequestResponseLogger.Models
{
    

   
    public class AlertInstance
        {
            public int Id { get; set; }
            public int? AlertRuleId { get; set; }
            public string AlertRuleName { get; set; }
            public int AlertTypeId { get; set; }
            public string AlertTypeName { get; set; }
            public int AlertValue { get; set; }
            public string? Severity { get; set; }
            public int ServerId { get; set; }
            public string ServerName { get; set; }
            public string? ClientIP { get; set; }
            public int? EndpointId { get; set; }
            public string? EndpointPath { get; set; }
        
            public DateTime TriggeredAtUTC { get; set; }
            public DateTime? ResolvedAtUTC { get; set; }
            public string Status { get; set; }
        }
    

   
    public class AlertNotification
    {
        public int Id { get; set; }
        public int? AlertInstanceId { get; set; }
        public string NotificationType { get; set; }
        public string Recipient { get; set; }
        public DateTime SentAtUTC { get; set; }

        public int AlertRuleId { get; set; }
        public string AlertRuleName { get; set; }
        public int AlertTypeId { get; set; }
        public string AlertTypeName { get; set; }
        public int AlertValue { get; set; }
        public string? Severity { get; set; }
        public int ServerId { get; set; }
        public string ServerName { get; set; }
        public string? ClientIP { get; set; }
        public int? EndpointId { get; set; }
        public string? EndpointPath { get; set; }

        public DateTime TriggeredAtUTC { get; set; }
        public string Status { get; set; }

    }

    public class AlertRecipient
    {
        public int Id { get; set; }
        public string Recipient { get; set; }

        public string Email { get; set; }

        public string Phone { get; set; }

        
    }

    public class AlertRuleRecipient
    {
        public int Id { get; set; }
        public int AlertRuleId { get; set; }
        public int RecipientId { get; set; }

       
    }

    public class AlertRule
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int? AlertTypeId { get; set; }

        public string? AlertTypeName { get; set; }

        public int? AlertValue { get; set; }
        public string? Expression { get; set; }

        public string Severity { get; set; }
        public DateTime? CreatedAtUTC { get; set; }
        public DateTime? UpdatedAtUTC { get; set; }
        public bool IsActive { get; set; }

        public bool? ActiveInAllServers { get; set; }
        public bool? ActiveInAllEndpoints { get; set; }

        
    }

    public class AlertType
    {


        public int Id { get; set; }
        public string AlertTypeName { get; set; }

        public bool Script { get; set; }
        public string Expression { get; set; }
    }




    public class AlertSilenced
    {
        public int Id { get; set; }
        public int AlertRuleId { get; set; }
        public DateTime SilencedFromUTC { get; set; }
        public DateTime SilencedUntilUTC { get; set; }
        public string Reason { get; set; }

        
    }

    public class AlertEndpoint
    {
        public int Id { get; set; }
        public string Path { get; set; }
    }

    public class AlertRuleEndpoint
    {
        public int Id { get; set; }
        public int AlertRuleId { get; set; }
        public int AlertEndpointId { get; set; }

        
    }

    public class AlertRulesServer
    {
        public int Id { get; set; } 
        public int AlertRuleId { get; set; } 
        public int ServerId { get; set; } 

        
    }

}

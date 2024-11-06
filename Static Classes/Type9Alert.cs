using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfRequestResponseLogger.Data;
using WpfRequestResponseLogger.Models;

namespace WpfRequestResponseLogger.Static_Classes
{
    public static class Type9Alert
    {
        public static void CheckType9Alerts(int serverId, string serverName, int seconds,int totalResponseSizeInBytes, List<AlertRule> Type9AlertRules)
        {

           
                foreach(var alertRule in Type9AlertRules)
                {
                    
                    
                    float totalResponseSizePerMinute=(float)totalResponseSizeInBytes * ((float)60 / (float)seconds);
                    


                    if (totalResponseSizePerMinute >= alertRule.AlertValue)
                    {
                        AlertInstance ai = new AlertInstance();
                        ai.ServerId = serverId;
                        ai.ServerName = serverName;
                        ai.AlertRuleId = alertRule.Id;
                        ai.AlertRuleName = alertRule.Name;
                        ai.Severity = alertRule.Severity;
                        ai.AlertTypeId = 9;
                        ai.AlertTypeName = "Outbound traffic per server in bytes per minute";
                        ai.AlertValue = (int)totalResponseSizePerMinute;
                        //ai.ClientIP = type1Data.ClientIp;
                        //ai.EndpointId =;
                        ai.EndpointPath = "";
                        ai.TriggeredAtUTC = DateTime.UtcNow;
                        ai.Status = "Firing";

                        HelperFunctions.AddAlertInstance(ai);

                        
                    }
                }
            
        }




    }
        
    
}

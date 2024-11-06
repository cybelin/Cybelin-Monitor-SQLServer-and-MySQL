using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfRequestResponseLogger.Data;
using WpfRequestResponseLogger.Models;

namespace WpfRequestResponseLogger.Static_Classes
{
    public static class Type10Alert
    {
        public static void CheckType10Alerts(int serverId, string serverName, int seconds,int totalRequestsPerServer, List<AlertRule> Type10AlertRules)
        {

           
                foreach(var alertRule in Type10AlertRules)
                {
                    
                    
                    float totalRequestsPerMinute = (float)totalRequestsPerServer * ((float)60 / (float)seconds);
                    


                    if (totalRequestsPerMinute >= alertRule.AlertValue)
                    {
                        AlertInstance ai = new AlertInstance();
                        ai.ServerId = serverId;
                        ai.ServerName = serverName;
                        ai.AlertRuleId = alertRule.Id;
                        ai.AlertRuleName = alertRule.Name;
                        ai.Severity = alertRule.Severity;
                        ai.AlertTypeId = 10;
                        ai.AlertTypeName = "Number of requests per server per minute";
                        ai.AlertValue = (int)totalRequestsPerMinute;
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

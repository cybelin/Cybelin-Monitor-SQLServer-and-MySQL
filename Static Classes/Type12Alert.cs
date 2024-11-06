using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfRequestResponseLogger.Data;
using WpfRequestResponseLogger.Models;

namespace WpfRequestResponseLogger.Static_Classes
{
    public static class Type12Alert
    {
        public static void CheckType12Alerts(int serverId, string serverName, int seconds,int requestWithoutResponsePerServer, List<AlertRule> Type12AlertRules)
        {

           
                foreach(var alertRule in Type12AlertRules)
                {
                    
                   
                    float totalWithoutResponsePerMinute = (float)requestWithoutResponsePerServer * ((float)60 / (float)seconds);
                    


                    if (totalWithoutResponsePerMinute >= alertRule.AlertValue)
                    {
                        AlertInstance ai = new AlertInstance();
                        ai.ServerId = serverId;
                        ai.ServerName = serverName;
                        ai.AlertRuleId = alertRule.Id;
                        ai.AlertRuleName = alertRule.Name;
                        ai.Severity = alertRule.Severity;
                        ai.AlertTypeId = 12;
                        ai.AlertTypeName = "Number of requests without response per server per minute";
                        ai.AlertValue = (int)totalWithoutResponsePerMinute;
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

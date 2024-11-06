using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfRequestResponseLogger.Data;
using WpfRequestResponseLogger.Models;

namespace WpfRequestResponseLogger.Static_Classes
{
    public static class Type11Alert
    {
        public static void CheckType11Alerts(int serverId, string serverName, int seconds,int count4xxStatusCodesPerServer, List<AlertRule> Type11AlertRules)
        {

           
                foreach(var alertRule in Type11AlertRules)
                {
                    
                    float total4XXStatusCodesPerMinute = (float)count4xxStatusCodesPerServer * ((float)60 / (float)seconds);
                    


                    if (total4XXStatusCodesPerMinute >= alertRule.AlertValue)
                    {
                        AlertInstance ai = new AlertInstance();
                        ai.ServerId = serverId;
                        ai.ServerName = serverName;
                        ai.AlertRuleId = alertRule.Id;
                        ai.AlertRuleName = alertRule.Name;
                        ai.Severity = alertRule.Severity;
                        ai.AlertTypeId = 11;
                        ai.AlertTypeName = "Number of 4XX responses per server per minute";
                        ai.AlertValue = (int)total4XXStatusCodesPerMinute;
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

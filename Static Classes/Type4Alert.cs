using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfRequestResponseLogger.Data;
using WpfRequestResponseLogger.Models;

namespace WpfRequestResponseLogger.Static_Classes
{
    public static class Type4Alert
    {
        public static void CheckType4Alerts(int serverId, string serverName, int seconds, List<Type4ListElement> type4GroupedData, List<AlertRule> Type4AlertRules)
        {

            foreach (var type4Data in type4GroupedData)
            {
                foreach(var alertRule in Type4AlertRules)
                {
                    
                                           
                    float totalCountNoResponsePerMinute = (float)type4Data.Count * ((float)60 / (float)seconds);

                    


                    if (totalCountNoResponsePerMinute >= alertRule.AlertValue)
                    {
                        AlertInstance ai = new AlertInstance();
                        ai.ServerId = serverId;
                        ai.ServerName = serverName;
                        ai.AlertRuleId = alertRule.Id;
                        ai.AlertRuleName = alertRule.Name;
                        ai.Severity = alertRule.Severity;
                        ai.AlertTypeId = 4;
                        ai.AlertTypeName = "Number of requests without response per client IP per minute";
                        ai.AlertValue = (int)totalCountNoResponsePerMinute;
                        ai.ClientIP = type4Data.ClientIp;
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
        
    public class Type4ListElement
    {
        public string ClientIp;
        public int Count;
    }
}

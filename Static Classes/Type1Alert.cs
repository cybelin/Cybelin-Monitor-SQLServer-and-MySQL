using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfRequestResponseLogger.Data;
using WpfRequestResponseLogger.Models;

namespace WpfRequestResponseLogger.Static_Classes
{
    public static class Type1Alert
    {
        public static void CheckType1Alerts(int serverId, string serverName, int seconds, List<Type1ListElement> type1GroupedData, List<AlertRule> Type1AlertRules)
        {

            foreach (var type1Data in type1GroupedData)
            {
                foreach(var alertRule in Type1AlertRules)
                {
                    
                    
                    float totalResponseSizePerMinute=(float)type1Data.TotalResponseSize * ((float)60 / (float)seconds);
                    

                    if (totalResponseSizePerMinute >= alertRule.AlertValue)
                    {
                        AlertInstance ai = new AlertInstance();
                        ai.ServerId = serverId;
                        ai.ServerName = serverName;
                        ai.AlertRuleId = alertRule.Id;
                        ai.AlertRuleName = alertRule.Name;
                        ai.Severity = alertRule.Severity;
                        ai.AlertTypeId = 1;
                        ai.AlertTypeName = "Outbound traffic per client IP in bytes per minute";
                        ai.AlertValue = (int)totalResponseSizePerMinute;
                        ai.ClientIP = type1Data.ClientIp;
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
        
    public class Type1ListElement
    {
        public string ClientIp;
        public long TotalResponseSize;
    }
}

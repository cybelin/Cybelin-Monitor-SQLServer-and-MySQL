using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfRequestResponseLogger.Data;
using WpfRequestResponseLogger.Models;

namespace WpfRequestResponseLogger.Static_Classes
{
    public static class Type3Alert
    {
        public static void CheckType3Alerts(int serverId, string serverName, int seconds, List<Type3ListElement> type3GroupedData, List<AlertRule> Type3AlertRules)
        {

            foreach (var type3Data in type3GroupedData)
            {
                foreach(var alertRule in Type3AlertRules)
                {
                    
                    float totalCount4XXPerMinute = (float)type3Data.Count4XXErrors * ((float)60/(float)seconds);
                       
                    


                    if (totalCount4XXPerMinute >= alertRule.AlertValue)
                    {
                        AlertInstance ai = new AlertInstance();
                        ai.ServerId = serverId;
                        ai.ServerName = serverName;
                        ai.AlertRuleId = alertRule.Id;
                        ai.AlertRuleName = alertRule.Name;
                        ai.Severity = alertRule.Severity;
                        ai.AlertTypeId = 3;
                        ai.AlertTypeName = "Number of 4XX responses per client IP per minute";
                        ai.AlertValue = (int)totalCount4XXPerMinute;
                        ai.ClientIP = type3Data.ClientIp;
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
        
    public class Type3ListElement
    {
        public string ClientIp;
        public int Count4XXErrors;
    }
}

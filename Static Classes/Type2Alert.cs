using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfRequestResponseLogger.Data;
using WpfRequestResponseLogger.Models;

namespace WpfRequestResponseLogger.Static_Classes
{
    public static class Type2Alert
    {
        public static void CheckType2Alerts(int serverId, string serverName, int seconds, List<Type2ListElement> type2GroupedData, List<AlertRule> Type2AlertRules)
        {

            foreach (var type2Data in type2GroupedData)
            {
                foreach(var alertRule in Type2AlertRules)
                {
                    
                   
                    float totalCountPerMinute = (float)type2Data.Count * ((float)60/(float)seconds);
                   


                    if (totalCountPerMinute >= alertRule.AlertValue)
                    {
                        AlertInstance ai = new AlertInstance();
                        ai.ServerId = serverId;
                        ai.ServerName = serverName;
                        ai.AlertRuleId = alertRule.Id;
                        ai.AlertRuleName = alertRule.Name;
                        ai.Severity = alertRule.Severity;
                        ai.AlertTypeId = 2;
                        ai.AlertTypeName = "Number of requests per client IP per minute";
                        ai.AlertValue = (int)totalCountPerMinute;
                        ai.ClientIP = type2Data.ClientIp;
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
        
    public class Type2ListElement
    {
        public string ClientIp;
        public int Count;
    }
}

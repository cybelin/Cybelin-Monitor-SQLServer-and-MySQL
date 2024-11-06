using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfRequestResponseLogger.Data;
using WpfRequestResponseLogger.Models;

namespace WpfRequestResponseLogger.Static_Classes
{
    public static class Type5Alert
    {
        public static void CheckType5Alerts(int serverId, string serverName, int seconds, List<Type5ListElement> type5GroupedData, List<AlertRule> Type5AlertRules)
        {

            foreach (var type5Data in type5GroupedData)
            {
                foreach(var alertRule in Type5AlertRules)
                {
                    int alertRuleId = alertRule.Id;
                    string requestPath = type5Data.RequestPath;
                    bool thisEndpoint = false;
                    if (alertRule.ActiveInAllEndpoints == false)    //If ==true there is no need to check this endpoint
                    {
                        var context = new DataContext();
                        List<AlertEndpoint> alertEndpointList = context.Set<AlertEndpoint>()
                            .FromSqlInterpolated($"EXEC GetAlertRuleEndpointsByServerAndRule @ServerId = {serverId}, @AlertRuleId = {alertRuleId}")
                            .ToList();

                        // Check if requestPath is in the list alertEndpointList

                        thisEndpoint = alertEndpointList.Any(endpoint => endpoint.Path == requestPath);
                    }
                    
                    if ((alertRule.ActiveInAllEndpoints == true) || (thisEndpoint == true))
                                     
                    {
                        
                            float totalResponseSizePerMinute = (float)type5Data.TotalResponseSize * ((float)60 / (float)seconds);
                        


                        if (totalResponseSizePerMinute >= alertRule.AlertValue)
                        {
                            AlertInstance ai = new AlertInstance();
                            ai.ServerId = serverId;
                            ai.ServerName = serverName;
                            ai.AlertRuleId = alertRule.Id;
                            ai.AlertRuleName = alertRule.Name;
                            ai.Severity = alertRule.Severity;
                            ai.AlertTypeId = 5;
                            ai.AlertTypeName = "Outbound traffic per endpoint in bytes per minute";
                            ai.AlertValue = (int)totalResponseSizePerMinute;
                            //ai.ClientIP = type5Data.ClientIp;
                            //ai.EndpointId =alertRule.;                        
                            ai.EndpointPath = type5Data.RequestPath;
                            ai.TriggeredAtUTC = DateTime.UtcNow;
                            ai.Status = "Firing";

                            HelperFunctions.AddAlertInstance(ai);


                        }

                    } 
                    
                    
                }
            }
        }




    }
        
    public class Type5ListElement
    {
        public string RequestPath;
        public long TotalResponseSize;
    }
}

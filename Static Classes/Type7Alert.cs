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
    public static class Type7Alert
    {
        public static void CheckType7Alerts(int serverId, string serverName, int seconds, List<Type7ListElement> type7GroupedData, List<AlertRule> Type7AlertRules)
        {

            foreach (var type7Data in type7GroupedData)
            {
                foreach(var alertRule in Type7AlertRules)
                {
                    int alertRuleId = alertRule.Id;
                    string requestPath = type7Data.RequestPath;
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
                        
                        float totalCountPerMinute = (float)type7Data.Count * ((float)60 / (float)seconds);
                        


                        if (totalCountPerMinute >= alertRule.AlertValue)
                        {
                            AlertInstance ai = new AlertInstance();
                            ai.ServerId = serverId;
                            ai.ServerName = serverName;
                            ai.AlertRuleId = alertRule.Id;
                            ai.AlertRuleName = alertRule.Name;
                            ai.Severity = alertRule.Severity;
                            ai.AlertTypeId = 7;
                            ai.AlertTypeName = "Number of 4XX responses per endpoint per minute";
                            ai.AlertValue = (int)totalCountPerMinute;
                            //ai.ClientIP = type5Data.ClientIp;
                            //ai.EndpointId =alertRule.;                        
                            ai.EndpointPath = type7Data.RequestPath;
                            ai.TriggeredAtUTC = DateTime.UtcNow;
                            ai.Status = "Firing";

                            HelperFunctions.AddAlertInstance(ai);


                        }

                    } 
                    
                    
                }
            }
        }




    }
        
    public class Type7ListElement
    {
        public string RequestPath;
        public long Count;
    }
}

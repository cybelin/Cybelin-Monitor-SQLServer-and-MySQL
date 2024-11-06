using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Microsoft.EntityFrameworkCore;
using WpfRequestResponseLogger.Data;
using WpfRequestResponseLogger.Models;
using WpfRequestResponseLogger.Static_Classes;
using static System.Net.Mime.MediaTypeNames;

namespace WpfRequestResponseLogger.Views
{
    public partial class LogsAfterDateWindow : Window
    {

        private System.Timers.Timer autoUpdateTimer;
        private DateTime requestTimeFilter;
        private List<LogData> _logsList = new List<LogData>();



        public LogsAfterDateWindow()
        {
            InitializeComponent();
            LoadServers(); 
        }

        
        private void LoadServers()
        {
            using (var context = new DataContext()) 
            {
                var servers = context.Servers.ToList();
                ServerComboBox.ItemsSource = servers; 
            }
        }

        

        
        public List<LogData> GetLogsAfterDateUsingDbContext(DateTime requestTimeFilter, string connectionString)
        {
            var context = new DataContext2(connectionString);
            var logsList = context.Set<LogData>()
                .FromSqlInterpolated($"EXEC GetLogsAfterDate @RequestTimeFilter = {requestTimeFilter}")
                .ToList();

            return logsList;

        }

        private void AutoUpdateCheckBox_CheckedChanged(object sender, RoutedEventArgs e)
        {
            if (AutoUpdateCheckBox.IsChecked == true)
            {
                int interval;

                if (!int.TryParse(ExecuteEveryTextBox.Text, out interval) || interval <= 0)
                {
                    
                    interval = 60;
                    ExecuteEveryTextBox.Text = "60";
                }

                
                    autoUpdateTimer = new System.Timers.Timer(interval * 1000);

                     autoUpdateTimer.Elapsed += OnAutoUpdateTimerElapsed;

                    autoUpdateTimer.Start();
                   
                
            }
            else
            {
                if (autoUpdateTimer != null)
                {
                    autoUpdateTimer.Stop();
                    autoUpdateTimer.Dispose();
                }
            }
        }

        private void OnAutoUpdateTimerElapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            
            Dispatcher.Invoke(() =>
            {
                if (int.TryParse(ExecuteEveryTextBox.Text, out int secondsToSubtract))
                {
                    
                    DateTime requestTimeFilter = DateTime.UtcNow.AddSeconds(-secondsToSubtract);
                    
                    GetDataFromDatabase(requestTimeFilter);
                    
                }
                else
                {
                    
                    MessageBox.Show("Please enter a valid number of seconds.");
                }
                
            });
        }

        public void GetDataFromDatabase(DateTime requestTimeFilter)
        {
            
            if (true)
            {
                
                string connectionString = (string)ServerComboBox.SelectedValue; // Obtener la cadena de conexión seleccionada
                if (connectionString == null)
                {
                    MessageBox.Show("Please select a SQL server.");
                    return;
                }

                try
                {
                    _logsList = GetLogsAfterDateUsingDbContext(requestTimeFilter, connectionString); // Pasamos la cadena de conexión
                    LogsDataGrid.ItemsSource = _logsList;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error fetching logs: {ex.Message}");
                }

                int serverId = 0;
                string serverName = "";
                
                if (ServerComboBox.SelectedItem is Server selectedServer)
                {
                    serverId = selectedServer.Id;
                    serverName = selectedServer.ServerName;
                    
                }

                //Execute the Stored Procedure called GetAlertRulesByServerId to obtain a list of Alert rules for the server
                var context = new DataContext();
                List<AlertRule> alertRulesList = context.Set<AlertRule>()
                    .FromSqlInterpolated($"EXEC GetAlertRulesByServerId @ServerId = {serverId}")
                    .ToList();

                Tab11DataGrid.ItemsSource = alertRulesList;     // Shows the Alert Rules in Tab 11

                List<AlertRule> type1AlertRules = alertRulesList.Where(alert => alert.AlertTypeId == 1).ToList();
                List<AlertRule> type2AlertRules = alertRulesList.Where(alert => alert.AlertTypeId == 2).ToList();
                List<AlertRule> type3AlertRules = alertRulesList.Where(alert => alert.AlertTypeId == 3).ToList();
                List<AlertRule> type4AlertRules = alertRulesList.Where(alert => alert.AlertTypeId == 4).ToList();
                List<AlertRule> type5AlertRules = alertRulesList.Where(alert => alert.AlertTypeId == 5).ToList();
                List<AlertRule> type6AlertRules = alertRulesList.Where(alert => alert.AlertTypeId == 6).ToList();
                List<AlertRule> type7AlertRules = alertRulesList.Where(alert => alert.AlertTypeId == 7).ToList();
                List<AlertRule> type8AlertRules = alertRulesList.Where(alert => alert.AlertTypeId == 8).ToList();
                List<AlertRule> type9AlertRules = alertRulesList.Where(alert => alert.AlertTypeId == 9).ToList();
                List<AlertRule> type10AlertRules = alertRulesList.Where(alert => alert.AlertTypeId == 10).ToList();
                List<AlertRule> type11AlertRules = alertRulesList.Where(alert => alert.AlertTypeId == 11).ToList();
                List<AlertRule> type12AlertRules = alertRulesList.Where(alert => alert.AlertTypeId == 12).ToList();
                int seconds = 0;
                if (int.TryParse(ExecuteEveryTextBox.Text, out int secondsToSubtract))
                {
                    seconds= secondsToSubtract;
                }
                else
                {
                    ExecuteEveryTextBox.Text = "60";
                    seconds = 60;

                }
                // Refresh the grid of Tab 2

                var groupedData = _logsList
                                 .GroupBy(log => log.ClientIp)
                                 .Select(group => new
                                 {
                                     ClientIp = group.Key,
                                     TotalResponseSize = group.Sum(log => log.ResponseSizeInBytes ?? 0)
                                 })
                                 .ToList();

                Tab2DataGrid.ItemsSource = groupedData;

                List<Type1ListElement> type1GroupedData = new List<Type1ListElement>();
                
                foreach (var element in groupedData)
                {
                    type1GroupedData.Add(new Type1ListElement
                    {
                        ClientIp = element.ClientIp,
                        TotalResponseSize = element.TotalResponseSize
                    });
                }
                
                Type1Alert.CheckType1Alerts( serverId, serverName, seconds,type1GroupedData, type1AlertRules );

                // Refresh the grid of Tab 3

                var groupedByClientIp = _logsList
                                .GroupBy(log => log.ClientIp)
                                .Select(group => new
                                {
                                    ClientIp = group.Key,
                                    Count = group.Count()
                                })
                                .ToList();

                Tab3DataGrid.ItemsSource = groupedByClientIp;

                List<Type2ListElement> type2GroupedData = new List<Type2ListElement>();

                foreach (var element in groupedByClientIp)
                {
                    type2GroupedData.Add(new Type2ListElement
                    {
                        ClientIp = element.ClientIp,
                        Count = element.Count
                    }) ;
                }

                Type2Alert.CheckType2Alerts(serverId, serverName, seconds, type2GroupedData, type2AlertRules);




                // Refresh the grid of tab 4
                var groupedDataByClientIpWithStatusCode4xx = _logsList
                                     .Where(log => log.StatusCode.HasValue && log.StatusCode.ToString().StartsWith("4"))
                                     .GroupBy(log => log.ClientIp)
                                     .Select(group => new
                                     {
                                         ClientIp = group.Key,
                                         Count4xxErrors = group.Count()
                                     })
                                     .ToList();

                Tab4DataGrid.ItemsSource = groupedDataByClientIpWithStatusCode4xx;

                List<Type3ListElement> type3GroupedData = new List<Type3ListElement>();

                foreach (var element in groupedDataByClientIpWithStatusCode4xx)
                {
                    type3GroupedData.Add(new Type3ListElement
                    {
                        ClientIp = element.ClientIp,
                        Count4XXErrors = element.Count4xxErrors
                    });
                }

                Type3Alert.CheckType3Alerts(serverId, serverName, seconds, type3GroupedData, type3AlertRules);

                // Refresh the grid of tab 5
                var groupedDataWithNullStatusCode = _logsList
                                     .Where(log => log.StatusCode == null)
                                     .GroupBy(log => log.ClientIp)
                                     .Select(group => new
                                     {
                                         ClientIp = group.Key,
                                         Count = group.Count()
                                     })
                                     .OrderBy(group => group.ClientIp)
                                     .ToList();

                Tab5DataGrid.ItemsSource = groupedDataWithNullStatusCode;

                List<Type4ListElement> type4GroupedData = new List<Type4ListElement>();

                foreach (var element in groupedDataWithNullStatusCode)
                {
                    type4GroupedData.Add(new Type4ListElement
                    {
                        ClientIp = element.ClientIp,
                        Count = element.Count
                    });
                }

                Type4Alert.CheckType4Alerts(serverId, serverName, seconds, type4GroupedData, type4AlertRules);


                // Refresh the grid of tab 6
                var groupedDataByRequestPath = _logsList
                                     .GroupBy(log => log.RequestPath)
                                     .Select(group => new
                                     {
                                         RequestPath = group.Key,
                                         TotalResponseSize = group.Sum(log => log.ResponseSizeInBytes ?? 0)
                                     })
                                     .ToList();

                Tab6DataGrid.ItemsSource = groupedDataByRequestPath;

                List<Type5ListElement> type5GroupedData = new List<Type5ListElement>();

                foreach (var element in groupedDataByRequestPath)
                {
                    type5GroupedData.Add(new Type5ListElement
                    {
                        RequestPath = element.RequestPath,
                        TotalResponseSize = element.TotalResponseSize
                    });
                }

                Type5Alert.CheckType5Alerts(serverId, serverName, seconds, type5GroupedData, type5AlertRules);



                //Refresh The grid of Tab 7

                var groupedByRequestPath = _logsList
                                    .GroupBy(log => log.RequestPath)
                                    .Select(group => new
                                    {
                                        RequestPath = group.Key,
                                        Count = group.Count()
                                    })
                                    .ToList();
                Tab7DataGrid.ItemsSource = groupedByRequestPath;

                List<Type6ListElement> type6GroupedData = new List<Type6ListElement>();

                foreach (var element in groupedByRequestPath)
                {
                    type6GroupedData.Add(new Type6ListElement
                    {
                        RequestPath = element.RequestPath,
                        Count = element.Count
                    });
                }

                Type6Alert.CheckType6Alerts(serverId, serverName, seconds, type6GroupedData, type6AlertRules);


                //Refresh the grid of Tab 8

                var groupedByRequestPathWith4xxStatusCode = _logsList
                                    .Where(log => log.StatusCode.HasValue && log.StatusCode.Value.ToString().StartsWith("4")) // Filtrar registros con StatusCode que empieza por 4
                                    .GroupBy(log => log.RequestPath) // Agrupar por RequestPath
                                    .Select(group => new
                                    {
                                        RequestPath = group.Key,
                                        Count = group.Count() // Contar registros en cada grupo
                                    })
                                    .ToList();
                Tab8DataGrid.ItemsSource = groupedByRequestPathWith4xxStatusCode;

                List<Type7ListElement> type7GroupedData = new List<Type7ListElement>();

                foreach (var element in groupedByRequestPathWith4xxStatusCode)
                {
                    type7GroupedData.Add(new Type7ListElement
                    {
                        RequestPath = element.RequestPath,
                        Count = element.Count
                    });
                }

                Type7Alert.CheckType7Alerts(serverId, serverName, seconds, type7GroupedData, type7AlertRules);


                //Refresh the grid of Tab 9

                var groupedByRequestPathWithNullResponseLogId = _logsList
                                    .Where(log => log.ResponseLogId == null) 
                                    .GroupBy(log => log.RequestPath) 
                                    .Select(group => new
                                    {
                                        RequestPath = group.Key,
                                        Count = group.Count() 
                                    })
                                    .ToList();

                Tab9DataGrid.ItemsSource = groupedByRequestPathWithNullResponseLogId;

                List<Type8ListElement> type8GroupedData = new List<Type8ListElement>();

                foreach (var element in groupedByRequestPathWithNullResponseLogId)
                {
                    type8GroupedData.Add(new Type8ListElement
                    {
                        RequestPath = element.RequestPath,
                        Count = element.Count
                    });
                }

                Type8Alert.CheckType8Alerts(serverId, serverName, seconds, type8GroupedData, type8AlertRules);


                //Refresh the textbox of Tab 10

               // Textbox 1

                var totalResponseSizeInBytes = _logsList
                                    .Where(log => log.ResponseSizeInBytes.HasValue) 
                                    .Sum(log => log.ResponseSizeInBytes.Value); 
                
                OutboundTrafficTextBox.Text = totalResponseSizeInBytes.ToString();
                int totalResponseSizePerServer = (int)totalResponseSizeInBytes;

                Type9Alert.CheckType9Alerts(serverId, serverName, seconds, totalResponseSizePerServer, type9AlertRules);


                // Textbox 2

                var totalRecords = _logsList.Count();
                int totalRequestsPerServer =(int)totalRecords;

                RequestsPerServerTextBox.Text = totalRecords.ToString();
                Type10Alert.CheckType10Alerts(serverId, serverName, seconds, totalRequestsPerServer, type10AlertRules);


                // Textbox 3 

                var count4xxStatusCodes = _logsList
                                    .Where(log => log.StatusCode.HasValue && log.StatusCode.Value.ToString().StartsWith("4")) // Filtrar registros con StatusCode que empieza por 4
                                    .Count(); // 

                Responses4XXTextBox.Text = count4xxStatusCodes.ToString();
                int count4xxStatusCodesPerServer = (int) count4xxStatusCodes;
                Type11Alert.CheckType11Alerts(serverId, serverName, seconds, count4xxStatusCodesPerServer, type11AlertRules);


                // Textbox 4

                var countNullResponseLogId = _logsList
                                    .Where(log => log.ResponseLogId == null) 
                                    .Count(); 

                RequestsWithoutResponseTextBox.Text = countNullResponseLogId.ToString();
                int requestWithoutResponsePerServer = (int)countNullResponseLogId;
                Type12Alert.CheckType12Alerts(serverId, serverName, seconds, requestWithoutResponsePerServer, type12AlertRules);

            }
        }


        


    }



    
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Threading; 
using Microsoft.EntityFrameworkCore;
using WpfRequestResponseLogger.Data;
using WpfRequestResponseLogger.Models;
using System.Timers;





namespace WpfRequestResponseLogger.Views
{
    public partial class LogsFromOneIpWindow : Window
    {
        private DataContext _context; // 
        
        
        private int _executeEverySeconds; 
        private List<LogData> logsList;  

        private Timer _timer;
        public LogsFromOneIpWindow()
        {
            InitializeComponent();
            InitializeDbContext(); 
            LoadServers();         
           
        }

        
        private void InitializeDbContext()
        {
            try
            {
                _context = new DataContext();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error initializing database context: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        
        private void LoadServers()
        {
            try
            {
                var servers = _context.Servers.ToList(); 

                if (servers.Count > 0)
                {
                    ServerComboBox.ItemsSource = servers;
                    ServerComboBox.DisplayMemberPath = "ServerName";
                    ServerComboBox.SelectedValuePath = "ConnectionString";
                }
                else
                {
                    MessageBox.Show("No servers found in the database.", "No Data", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading servers: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        

        

        

       
        private void GetDataButton_Click(object sender, RoutedEventArgs e)
        {
            ExecuteStoredProcedure();
        }

        private void ExecuteStoredProcedure()
        {
            try
            {
                if (ServerComboBox.SelectedValue == null)
                {
                    MessageBox.Show("Please select a SQL server.");
                    return;
                }
                string connectionString = ServerComboBox.SelectedValue.ToString();
                

                var context = new DataContext2(connectionString);
                DateTime requestTimeFilter = DateTime.Parse(UtcDatetimeTextBox.Text);
                string clientIp = IpAddressTextBox.Text;

                
                if (!IsValidIpAddress(clientIp))
                {
                    MessageBox.Show("The IP address entered is not a valid IPv4 or IPv6 address.", "Invalid IP", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                logsList = context.Set<LogData>()
                    .FromSqlInterpolated($"EXEC GetLogsFilteredByTimeAndClientIp {requestTimeFilter}, {clientIp}")
                    .ToList();


                

                LogsDataGrid.ItemsSource = logsList;

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error fetching data: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }






        }


      

        
        private bool IsValidIpAddress(string ipAddress)
        {
            string ipv4Pattern = @"^(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\." +
                                 @"(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\." +
                                 @"(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\." +
                                 @"(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)$";

            string ipv6Pattern = @"^(([0-9a-fA-F]{1,4}:){7}([0-9a-fA-F]{1,4}|:))|" +
                                 @"(([0-9a-fA-F]{1,4}:){1,7}:)|" +
                                 @"(([0-9a-fA-F]{1,4}:){1,6}(:[0-9a-fA-F]{1,4}){1,6})|" +
                                 @"(::[fF]{4}:([0-9]{1,3}\.){3}[0-9]{1,3})|" +
                                 @"(:(:[0-9a-fA-F]{1,4}){1,7})$";

            return Regex.IsMatch(ipAddress, ipv4Pattern) || Regex.IsMatch(ipAddress, ipv6Pattern);
        }

        
        private DateTime GetTimeUTC()
        {
            return DateTime.UtcNow;
        }

        
        private void UtcNowButton_Click(object sender, RoutedEventArgs e)
        {
            UtcDatetimeTextBox.Text = GetTimeUTC().ToString("yyyy-MM-ddTHH:mm:ssZ");
        }
    }
}

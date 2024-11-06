using Microsoft.EntityFrameworkCore;
using System.Windows;
using WpfRequestResponseLogger.Data;


namespace WpfRequestResponseLogger.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void OpenBlacklistedIpsWindow_Click(object sender, RoutedEventArgs e)
        {
            
            ManageBlacklistedIpsWindow window = new ManageBlacklistedIpsWindow();

            
            window.Show();
        }

        private void OpenConfigurationManagerWindow_Click(object sender, RoutedEventArgs e)
        {
            WpfRequestResponseLogger.Views.ConfigurationManagerWindow configManagerWindow = new WpfRequestResponseLogger.Views.ConfigurationManagerWindow();
            configManagerWindow.ShowDialog();
        }

        private void OpenManageWhitelistedIpsWindow_Click(object sender, RoutedEventArgs e)
        {
            ManageWhitelistedIpsWindow manageIpsWindow = new ManageWhitelistedIpsWindow();
            manageIpsWindow.ShowDialog(); 
        }

        

        private void OpenLogsAfterDateWindow_Click(object sender, RoutedEventArgs e)
        {
            LogsAfterDateWindow logsWindow = new LogsAfterDateWindow();
            logsWindow.Show(); 
        }

        
        private void OpenLogsFromOneIpWindow_Click(object sender, RoutedEventArgs e)
        {
            
            LogsFromOneIpWindow logsFromOneIpWindow = new LogsFromOneIpWindow();
            logsFromOneIpWindow.Show();
        }

        private void ManageServers_Click(object sender, RoutedEventArgs e)
        {
            
            var manageServersWindow = new ManageServersWindow();
            manageServersWindow.Show();
        }

        
        private void ExitMenuItem_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

       

        private void ManageAlertRecipients_Click(object sender, RoutedEventArgs e)
        {
        ManageAlertRecipientsWindow manageAlertRecipientsWindow = new ManageAlertRecipientsWindow();
        manageAlertRecipientsWindow.Show();
        }
        private void ManageAlertRulesWindow(object sender, RoutedEventArgs e)
        {
            
            ManageAlertRulesWindow alertRulesWindow = new ManageAlertRulesWindow();
            alertRulesWindow.ShowDialog(); 
        }

        
        private void ManageAlertSilencedMenuItem_Click(object sender, RoutedEventArgs e)
        {
            
            ManageAlertSilencedWindow manageAlertSilencedWindow = new ManageAlertSilencedWindow();
            manageAlertSilencedWindow.ShowDialog(); 
        }
       
        private void ManageAlertRuleRecipientsMenuItem_Click(object sender, RoutedEventArgs e)
        {
            
            ManageAlertRuleRecipientsWindow manageAlertRuleRecipientsWindow = new ManageAlertRuleRecipientsWindow();
            manageAlertRuleRecipientsWindow.ShowDialog(); 
        }

        // Event handler for the 'Manage Alert Endpoints' menu item click
        private void ManageAlertEndpointsMenuItem_Click(object sender, RoutedEventArgs e)
        {
            // Create a new instance of the ManageAlertEndpointsWindow and open it
            ManageAlertEndpointsWindow manageEndpointsWindow = new ManageAlertEndpointsWindow();
            manageEndpointsWindow.ShowDialog();
        }

        // Event handler for the 'Manage Alert Rules Endpoints' menu item click
        private void ManageAlertRulesEndpointsMenuItem_Click(object sender, RoutedEventArgs e)
        {
            // Create a new instance of the ManageAlertRulesEndpointsWindow and open it
            ManageAlertRulesEndpointsWindow manageWindow = new ManageAlertRulesEndpointsWindow();
            manageWindow.ShowDialog();
        }

        // Event handler for the menu item to open ManageAlertRulesServersWindow
        private void ManageAlertRulesServersMenuItem_Click(object sender, RoutedEventArgs e)
        {
            ManageAlertRulesServersWindow manageWindow = new ManageAlertRulesServersWindow();
            manageWindow.Show(); // Show the new window
        }

        // Event handler for opening the ManageAlertInstancesWindow
        private void ManageAlertInstancesMenuItem_Click(object sender, RoutedEventArgs e)
        {
            var manageAlertInstancesWindow = new ManageAlertInstancesWindow();
            manageAlertInstancesWindow.Show();
        }

        
        private void AlertNotificationsMenuItem_Click(object sender, RoutedEventArgs e)
        {
            AlertNotificationsWindow alertNotificationsWindow = new AlertNotificationsWindow();
            alertNotificationsWindow.Show();
        }
        // Event handler to open ServersConfigurationsWindow
        private void ManageServersConfigurations_Click(object sender, RoutedEventArgs e)
        {
            // Create an instance of the ServersConfigurationsWindow and show it
            ServersConfigurationsWindow serversConfigurationsWindow = new ServersConfigurationsWindow();
            serversConfigurationsWindow.Show();
        }

        private void ManageServersBlacklistedIps_Click(object sender, RoutedEventArgs e)
        {
            // Create and show the ManageServersBlacklistedIps window
            ManageServersBlacklistedIps manageBlacklistedIpsWindow = new ManageServersBlacklistedIps();
            manageBlacklistedIpsWindow.Show();
        }

        
        private void SynchronizeBlacklistedIps_Click(object sender, RoutedEventArgs e)
        {
            
           

            using (var sourceContext = new DataContext())
            using (var serverContext = new DataContext())
            {
                

                
                var syncWindow = new SynchronizeBlacklistedIps();
                syncWindow.ShowDialog();
            }
        }


    }
}


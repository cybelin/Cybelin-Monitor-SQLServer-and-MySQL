using System;
using System.Windows;

namespace WpfRequestResponseLogger
{
    public partial class SynchronizeBlacklistedIps : Window
    {
        private readonly BlacklistedIpSyncService _syncService;

        public SynchronizeBlacklistedIps()
        {
            InitializeComponent();
            
        }

        
        private void SyncButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                
                BlacklistedIpSyncService.SyncBlacklistedIps();
                MessageBox.Show("Synchronization completed successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error during synchronization: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}

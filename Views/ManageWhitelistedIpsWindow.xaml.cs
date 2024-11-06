using System;
using System.Linq;
using System.Windows;
using WpfRequestResponseLogger.Data;

namespace WpfRequestResponseLogger.Views
{
    public partial class ManageWhitelistedIpsWindow : Window
    {
        public ManageWhitelistedIpsWindow()
        {
            InitializeComponent();
            LoadWhitelistedIpsData(); 
        }

        
        private void LoadWhitelistedIpsData()
        {
            using (var context = new DataContext())
            {
                var whitelistedIps = context.WhitelistedIps.ToList();
                WhitelistedIpsDataGrid.ItemsSource = whitelistedIps;
            }
        }

        
        private void OnAddButtonClick(object sender, RoutedEventArgs e)
        {
            var addWindow = new AddWhitelistedIpWindow();

            
            addWindow.OnWhitelistedIpAdded += () =>
            {
                LoadWhitelistedIpsData(); 
            };

            addWindow.ShowDialog(); 
        }

        
        private void OnEditButtonClick(object sender, RoutedEventArgs e)
        {
            
            if (WhitelistedIpsDataGrid.SelectedItem is Models.WhitelistedIp selectedIp)
            {
                var editWindow = new EditWhitelistedIpWindow(selectedIp);

                
                editWindow.ShowDialog();

                
                LoadWhitelistedIpsData();
            }
            else
            {
                MessageBox.Show("Please select an IP to edit.");
            }
        }


        
        private void OnDeleteButtonClick(object sender, RoutedEventArgs e)
        {
            if (WhitelistedIpsDataGrid.SelectedItem is Models.WhitelistedIp selectedIp)
            {
                MessageBoxResult result = MessageBox.Show($"Are you sure you want to delete the IP: {selectedIp.IpAddress}?",
                                                          "Confirm Delete", MessageBoxButton.YesNo, MessageBoxImage.Warning);

                if (result == MessageBoxResult.Yes)
                {
                    DeleteWhitelistedIpFromDatabase(selectedIp);
                    LoadWhitelistedIpsData(); 
                    MessageBox.Show("IP deleted successfully.");
                }
            }
            else
            {
                MessageBox.Show("Please select an IP to delete.");
            }
        }

        
        private void DeleteWhitelistedIpFromDatabase(Models.WhitelistedIp ipToDelete)
        {
            using (var context = new DataContext())
            {
                var ip = context.WhitelistedIps.FirstOrDefault(i => i.Id == ipToDelete.Id);
                if (ip != null)
                {
                    context.WhitelistedIps.Remove(ip);
                    context.SaveChanges();
                }
            }
        }

        
        private void OnExitButtonClick(object sender, RoutedEventArgs e)
        {
            this.Close(); 
        }
    }
}

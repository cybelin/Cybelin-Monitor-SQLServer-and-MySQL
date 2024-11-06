using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using WpfRequestResponseLogger.Views;
using WpfRequestResponseLogger.Models;
using WpfRequestResponseLogger.Data;




namespace WpfRequestResponseLogger
    {
    public partial class ManageBlacklistedIpsWindow : Window
    {
        // Constructor
        public ManageBlacklistedIpsWindow()
        {
            InitializeComponent();
            LoadBlacklistedIps();
        }



        // Event handler for the Add button
        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            
            var addWindow = new AddBlacklistedIpWindow();

            
            addWindow.OnIpAdded += () =>
            {
                LoadBlacklistedIps(); 
            };

            addWindow.ShowDialog(); 
        }


        // Event handler for the Edit button

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            if (BlacklistedIpsDataGrid.SelectedItem is Models.BlacklistedIp selectedIp)
            {
                EditBlacklistedIpWindow editWindow = new EditBlacklistedIpWindow(selectedIp);
                editWindow.ShowDialog();

                
                LoadBlacklistedIps();
            }
            else
            {
                MessageBox.Show("Please select an IP to edit.");
            }
        }






        // Event handler for the Delete button
        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            
            if (BlacklistedIpsDataGrid.SelectedItem is Models.BlacklistedIp selectedIp)
            {
                
                MessageBoxResult result = MessageBox.Show($"Are you sure you want to delete IP: {selectedIp.IpAddress}?", "Confirm Delete", MessageBoxButton.YesNo, MessageBoxImage.Warning);

                if (result == MessageBoxResult.Yes)
                {
                    
                    DeleteIpFromDatabase(selectedIp);

                    
                    LoadBlacklistedIps();

                    MessageBox.Show("IP deleted successfully.");
                }
            }
            else
            {
                MessageBox.Show("Please select an IP to delete.");
            }
        }

        private void DeleteIpFromDatabase(Models.BlacklistedIp selectedIp)
        {
            try
            {
                using (var context = new DataContext())
                {
                    var ipToDelete = context.MonitorBlacklistedIps.FirstOrDefault(ip => ip.Id == selectedIp.Id);

                    if (ipToDelete != null)
                    {
                        context.MonitorBlacklistedIps.Remove(ipToDelete);
                        context.SaveChanges();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error deleting IP: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }




        // Event handler for the Exit button
        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close(); // Close the window
        }

        
        private void LoadBlacklistedIps()
        {
            using (var context = new DataContext())
            {
                var blacklistedIps = context.MonitorBlacklistedIps.ToList();
                BlacklistedIpsDataGrid.ItemsSource = blacklistedIps;
            }
        }

    }

    



}

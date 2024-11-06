using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Windows;
using WpfRequestResponseLogger.Data;
using WpfRequestResponseLogger.Models; 

namespace WpfRequestResponseLogger
{
    public partial class ManageServersBlacklistedIps : Window
    {
        public ManageServersBlacklistedIps()
        {
            InitializeComponent();
            LoadServers();
        }

        // Load the servers into the ComboBox
        private void LoadServers()
        {
            using (var context = new DataContext())
            {
                var servers = context.Servers.ToList();
                ServersComboBox.ItemsSource = servers;
                ServersComboBox.DisplayMemberPath = "ServerName";
                ServersComboBox.SelectedValuePath = "ConnectionString";
            }
        }

        // When a server is selected, load the Blacklisted IPs
        private void ServersComboBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (ServersComboBox.SelectedItem is Server selectedServer)
            {
                LoadBlacklistedIps(selectedServer.ConnectionString);
            }
        }

        // Load the Blacklisted IPs from the selected server
        private void LoadBlacklistedIps(string connectionString)
        {
            using (var context = new DataContext2(connectionString))
            {
                var blacklistedIps = context.BlacklistedIps.ToList();
                BlacklistedIpsDataGrid.ItemsSource = blacklistedIps;
            }
        }

        // Event handler for Add button
        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            if (ServersComboBox.SelectedItem is Server selectedServer)
            {
                // Open the AddServerBlacklistedIp window
                AddServerBlacklistedIp addIpWindow = new AddServerBlacklistedIp(selectedServer.ConnectionString);

                // Show the window as a dialog and if Save was successful, reload the grid
                if (addIpWindow.ShowDialog() == true)
                {
                    LoadBlacklistedIps(selectedServer.ConnectionString);
                }
            }
            else
            {
                MessageBox.Show("Please select a server before adding a blacklisted IP.", "No Server Selected", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }


        // Event handler for Edit button
        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            // Check if an IP is selected in the DataGrid
            if (BlacklistedIpsDataGrid.SelectedItem is BlacklistedIp selectedIp)
            {
                // Get the selected server's connection string from the ComboBox
                if (ServersComboBox.SelectedItem is Server selectedServer)
                {
                    // Open the EditServerBlacklistedIp window, passing the selected server's connection string and the selected BlacklistedIp object
                    EditServerBlacklistedIp editIpWindow = new EditServerBlacklistedIp(selectedServer.ConnectionString, selectedIp);

                    // Show the window as a dialog and check if the save was successful
                    if (editIpWindow.ShowDialog() == true)
                    {
                        // Reload the Blacklisted IPs from the database after the edit
                        LoadBlacklistedIps(selectedServer.ConnectionString);
                    }
                }
                else
                {
                    MessageBox.Show("Please select a server before editing a blacklisted IP.", "No Server Selected", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            else
            {
                MessageBox.Show("Please select an IP to edit.", "No Selection", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }


        // Event handler for Delete button
        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (BlacklistedIpsDataGrid.SelectedItem is BlacklistedIp selectedIp)
            {
                var result = MessageBox.Show($"Are you sure you want to delete the IP: {selectedIp.IpAddress}?", "Confirm Deletion", MessageBoxButton.YesNo, MessageBoxImage.Warning);

                if (result == MessageBoxResult.Yes)
                {
                    DeleteBlacklistedIp(selectedIp);
                }
            }
            else
            {
                MessageBox.Show("Please select an IP to delete.", "No Selection", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        // Delete the selected Blacklisted IP
        private void DeleteBlacklistedIp(BlacklistedIp ip)
        {
            using (var context = new DataContext2(ServersComboBox.SelectedValue.ToString()))
            {
                context.BlacklistedIps.Remove(ip);
                context.SaveChanges();
                
            }

            if (ServersComboBox.SelectedItem is Server selectedServer)
            {
                LoadBlacklistedIps(selectedServer.ConnectionString);
            }
        }

        // Event handler for Exit button
        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close(); // Close the window
        }
    }
}

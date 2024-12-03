using System;
using System.Linq;
using System.Windows;
using WpfRequestResponseLogger.Data;
using WpfRequestResponseLogger.Models;


namespace WpfRequestResponseLogger
{
    public partial class ServersConfigurationsWindow : Window
    {
        public ServersConfigurationsWindow()
        {
            InitializeComponent();
            LoadServers();
        }

        // Load servers into the ComboBox when the window is initialized
        private void LoadServers()
        {
            using (var context = new DataContext()) // Assuming DataContext is the correct DbContext
            {
                var servers = context.Servers.ToList();
                ServersComboBox.ItemsSource = servers;
                ServersComboBox.DisplayMemberPath = "ServerName";
                ServersComboBox.SelectedValuePath = "ConnectionString";
            }
        }

        // Handle the ComboBox selection change event to load the configurations
        private void ServersComboBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (ServersComboBox.SelectedItem is Server selectedServer)
            {
                using (var context = new DataContext2(selectedServer.ConnectionString, selectedServer.ServerType)) // Use the selected server's connection string
                {
                    var configurations = context.Configurations.ToList();
                    ConfigurationsDataGrid.ItemsSource = configurations;
                }
            }
        }

        // Event handler for the "Add" button click
        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            // Ensure a server is selected before proceeding
            if (ServersComboBox.SelectedItem is Server selectedServer)
            {
                // Open the AddServerConfiguration window with the selected server's connection string
                AddServerConfiguration addConfigWindow = new AddServerConfiguration(selectedServer.ConnectionString,selectedServer.ServerType);

                // Show the window as a dialog and check if the Save was successful
                if (addConfigWindow.ShowDialog() == true)
                {
                    // If Save was successful, reload the configurations in the grid
                    using (var context = new DataContext2(selectedServer.ConnectionString, selectedServer.ServerType))
                    {
                        var configurations = context.Configurations.ToList();
                        ConfigurationsDataGrid.ItemsSource = configurations;
                    }
                }
            }
            else
            {
                MessageBox.Show("Please select a server before adding a configuration.", "No Server Selected", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }


        // Event handler for the "Edit" button click
        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            if (!(ServersComboBox.SelectedItem is Server) || ConfigurationsDataGrid.SelectedItem is not Configuration selectedConfiguration)
            {
                MessageBox.Show("Please select a server and a configuration to edit.", "No Selection", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var selectedServer = (Server)ServersComboBox.SelectedItem;

            // Open the EditServerConfiguration window with the selected server's connection string and configuration
            EditServerConfiguration editConfigWindow = new EditServerConfiguration(selectedServer.ConnectionString, selectedServer.ServerType, selectedConfiguration);

            // Show the window as a dialog and if Save was successful, reload the configurations
            if (editConfigWindow.ShowDialog() == true)
            {
                using (var context = new DataContext2(selectedServer.ConnectionString, selectedServer.ServerType))
                {
                    var configurations = context.Configurations.ToList();
                    ConfigurationsDataGrid.ItemsSource = configurations;
                }
            }
        }


        // Event handler for the "Delete" button click
        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            // Ensure a server is selected and a configuration is selected in the DataGrid
            if (!(ServersComboBox.SelectedItem is Server) || ConfigurationsDataGrid.SelectedItem is not Configuration selectedConfiguration)
            {
                MessageBox.Show("Please select a server and a configuration to delete.", "No Selection", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var selectedServer = (Server)ServersComboBox.SelectedItem;

            // Confirm the deletion
            var result = MessageBox.Show($"Are you sure you want to delete the configuration with Key: {selectedConfiguration.Key}?",
                                         "Confirm Deletion", MessageBoxButton.YesNo, MessageBoxImage.Warning);

            if (result == MessageBoxResult.Yes)
            {
                // Perform deletion from the database
                DeleteConfiguration(selectedServer.ConnectionString, selectedServer.ServerType, selectedConfiguration);

                // Refresh the DataGrid after deletion
                using (var context = new DataContext2(selectedServer.ConnectionString, selectedServer.ServerType))
                {
                    var configurations = context.Configurations.ToList();
                    ConfigurationsDataGrid.ItemsSource = configurations;
                }
            }
        }

        // Method to delete the selected configuration from the database
        private void DeleteConfiguration(string connectionString, string serverType,Configuration configuration)
        {
            using (var context = new DataContext2(connectionString, serverType))
            {
                context.Configurations.Remove(configuration);
                context.SaveChanges();
            }
        }


        // Event handler for the "Exit" button click
        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close(); // Close the window
        }
    }
}

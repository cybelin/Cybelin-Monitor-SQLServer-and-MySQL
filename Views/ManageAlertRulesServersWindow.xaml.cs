using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Windows;
using WpfRequestResponseLogger;
using WpfRequestResponseLogger.Data;

using WpfRequestResponseLogger.Models;


namespace WpfRequestResponseLogger
{
    public partial class ManageAlertRulesServersWindow : Window
    {
        private readonly DataContext _context; 
        private List<AlertRulesServerDetails> _alertRulesServerDetails; 

        public ManageAlertRulesServersWindow()
        {
            InitializeComponent();
            _context = new DataContext(); 
            LoadData(); 
        }

        private void LoadData()
        {
            
            var alertRulesServerRawData = _context.AlertRulesServerDetails
                .FromSqlRaw("EXEC GetAlertRulesServersWithDetails")
                .AsNoTracking() // Prevents Entity Framework from tracking changes to the data
                .ToList(); 

            
            

            AlertRulesServersDataGrid.ItemsSource = alertRulesServerRawData
       .OrderBy(x => x.AlertRuleId) 
       .ToList();
        }






        // Event handler for the Add button
        // Inside ManageAlertRulesServersWindow class

        // Event handler for the Add button
        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            AddAlertRuleServers addWindow = new AddAlertRuleServers();
            addWindow.ShowDialog(); // Show the window as a dialog
            LoadData(); // Refresh the data after adding
        }


        // Event handler for the Edit button
               
        
        
        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            // Get the selected AlertRuleEndpointDetails from the DataGrid
            var selectedServerDetails = (AlertRulesServerDetails)AlertRulesServersDataGrid.SelectedItem;

            if (selectedServerDetails != null)
            {
                // Retrieve the real AlertRuleEndpoint from the database using the Id
                var alertRuleServer = _context.AlertRulesServers
                    .FirstOrDefault(ep => ep.Id == selectedServerDetails.Id);

                if (alertRuleServer != null)
                {
                    // Open the EditAlertRuleEndpointsWindow with the selected AlertRuleEndpoint
                    EditAlertRuleServers editWindow = new EditAlertRuleServers(alertRuleServer.Id);
                    editWindow.ShowDialog();

                    // Reload data after editing the record
                    LoadData();
                }
                else
                {
                    MessageBox.Show("The selected server could not be found in the database.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("Please select a server to edit.", "Edit Alert Rule Server", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }




        // Event handler for the Delete button
        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            // Check if an item is selected in the DataGrid
            var selectedServerDetails = (AlertRulesServerDetails)AlertRulesServersDataGrid.SelectedItem;

            if (selectedServerDetails != null)
            {
                // Confirm deletion
                var result = MessageBox.Show("Are you sure you want to delete this server?", "Confirm Deletion", MessageBoxButton.YesNo, MessageBoxImage.Warning);

                if (result == MessageBoxResult.Yes)
                {
                    // Retrieve the real AlertRuleServer from the database using the Id
                    var alertRuleServer = _context.AlertRulesServers.FirstOrDefault(sr => sr.Id == selectedServerDetails.Id);

                    if (alertRuleServer != null)
                    {
                        // Remove the server from the context
                        _context.AlertRulesServers.Remove(alertRuleServer);

                        // Save the changes to the database
                        _context.SaveChanges();

                        // Reload data after deletion
                        LoadData();
                    }
                    else
                    {
                        MessageBox.Show("The selected server could not be found in the database.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("Please select a server to delete.", "Delete Alert Rule Server", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }


        // Event handler for the Exit button
        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close(); // Close the current window
        }
    }
}

using System.Linq;
using System.Windows;
using Microsoft.EntityFrameworkCore;
using WpfRequestResponseLogger.Data;
using WpfRequestResponseLogger.Models;

namespace WpfRequestResponseLogger
{
    public partial class ManageAlertRulesEndpointsWindow : Window
    {
        private readonly DataContext _context;

        // Constructor
        public ManageAlertRulesEndpointsWindow()
        {
            InitializeComponent();
            _context = new DataContext(); // Initialize your DbContext
            LoadAlertRulesEndpoints();
        }

        // Method to load data from the stored procedure into the DataGrid
        private void LoadAlertRulesEndpoints()
        {
            // Execute the stored procedure and load data into DataGrid
            var alertRulesEndpoints = _context.AlertRuleEndpointDetails
                .FromSqlRaw("EXEC GetAllAlertRuleEndpointsWithDetails")
                .AsNoTracking()  // Prevents Entity Framework from tracking changes to the data
                .ToList();

            alertRulesEndpointsDataGrid.ItemsSource = alertRulesEndpoints
                .OrderBy(x => x.AlertRuleEndpointId) 
       .ToList();
        }



        // Event handler for the 'Add' button click
        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            // Open window for adding a new alert rule endpoint
            
            AddAlertRuleEndpointsWindow addWindow = new AddAlertRuleEndpointsWindow();
            addWindow.ShowDialog();
            // Reload data after adding a new record
            LoadAlertRulesEndpoints();
            
        }

        // Event handler for the 'Edit' button click
        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            // Get the selected AlertRuleEndpointDetails from the DataGrid
            var selectedEndpointDetails = (AlertRuleEndpointDetails)alertRulesEndpointsDataGrid.SelectedItem;

            if (selectedEndpointDetails != null)
            {
                // Retrieve the real AlertRuleEndpoint from the database using the Id
                var alertRuleEndpoint = _context.AlertRulesEndpoints
                    .FirstOrDefault(ep => ep.Id == selectedEndpointDetails.AlertRuleEndpointId);

                if (alertRuleEndpoint != null)
                {
                    // Open the EditAlertRuleEndpointsWindow with the selected AlertRuleEndpoint
                    EditAlertRuleEndpointsWindow editWindow = new EditAlertRuleEndpointsWindow(alertRuleEndpoint);
                    editWindow.ShowDialog();

                    // Reload data after editing the record
                    LoadAlertRulesEndpoints();
                }
                else
                {
                    MessageBox.Show("The selected endpoint could not be found in the database.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("Please select an endpoint to edit.", "Edit Alert Rule Endpoint", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        // Event handler for the 'Delete' button click
        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            // Get the selected AlertRuleEndpointDetails from the DataGrid
            var selectedEndpointDetails = (AlertRuleEndpointDetails)alertRulesEndpointsDataGrid.SelectedItem;

            if (selectedEndpointDetails != null)
            {
                // Retrieve the real AlertRuleEndpoint from the database using the Id
                var alertRuleEndpoint = _context.AlertRulesEndpoints
                    .FirstOrDefault(ep => ep.Id == selectedEndpointDetails.AlertRuleEndpointId);

                if (alertRuleEndpoint != null)
                {
                    // Ask for confirmation before deleting
                    var result = MessageBox.Show($"Are you sure you want to delete this alert rule endpoint?",
                                                 "Delete Alert Rule Endpoint", MessageBoxButton.YesNo, MessageBoxImage.Warning);

                    if (result == MessageBoxResult.Yes)
                    {
                        // Delete the selected endpoint from the database
                        _context.AlertRulesEndpoints.Remove(alertRuleEndpoint);
                        _context.SaveChanges();

                        // Reload the data in the DataGrid
                        LoadAlertRulesEndpoints();
                    }
                }
                else
                {
                    MessageBox.Show("The selected endpoint could not be found in the database.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("Please select an endpoint to delete.", "Delete Alert Rule Endpoint", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }


        // Event handler for the 'Exit' button click
        private void BtnExit_Click(object sender, RoutedEventArgs e)
        {
            // Close the window
            this.Close();
        }
    }
}

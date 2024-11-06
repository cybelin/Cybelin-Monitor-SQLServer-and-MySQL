using System.Windows;
using System.Linq;
using System;
using WpfRequestResponseLogger.Data;
using WpfRequestResponseLogger.Models;

namespace WpfRequestResponseLogger
{
    
    public partial class ManageAlertRulesWindow : Window
    {
        private DataContext _context;

        public ManageAlertRulesWindow()
        {
            InitializeComponent();
            _context = new DataContext(); // Initialize your DbContext
            LoadAlertRules();
        }

        // Method to load data into the DataGrid
        private void LoadAlertRules()
        {
            // Query to fetch AlertRules from the database with null handling
            var alertRules = _context.AlertRules
                .Select(ar => new
                {
                    ar.Id,
                    ar.Name,
                    ar.AlertTypeId, // Removed null-coalescing as this is not nullable
                    ar.AlertTypeName,
                    ar.AlertValue, // Removed null-coalescing as this is not nullable
                    Expression = ar.Expression ?? string.Empty, // Assign empty string if null
                    Severity = ar.Severity ?? string.Empty, // Assign empty string if null
                    CreatedAtUTC = ar.CreatedAtUTC ?? default(DateTime), // Assign default DateTime if null
                    UpdatedAtUTC = ar.UpdatedAtUTC ?? default(DateTime), // Assign default DateTime if null
                    IsActive = ar.IsActive, // bool does not need null handling as it is not nullable
                    ActiveInAllServers = ar.ActiveInAllServers ?? false, // Handle nullable bool
                    ActiveInAllEndpoints = ar.ActiveInAllEndpoints ?? false // Handle nullable bool
                })
                .ToList();

            AlertRulesDataGrid.ItemsSource = alertRules;
        }

        // Event handler for Add button click
        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            // Open AddAlertRules window (to be implemented)
            
            var addWindow = new AddAlertRulesWindow();
            addWindow.ShowDialog();
            LoadAlertRules(); // Refresh the grid after adding
            
        }

        // Event handler for Edit button click
        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            var selectedRule = AlertRulesDataGrid.SelectedItem;
            if (selectedRule != null)
            {
                // Rebuild AlertRule object from the anonymous type
                var alertRule = new AlertRule
                {
                    Id = (int)selectedRule.GetType().GetProperty("Id").GetValue(selectedRule),
                    Name = (string)selectedRule.GetType().GetProperty("Name").GetValue(selectedRule),
                    AlertTypeId = (int?)selectedRule.GetType().GetProperty("AlertTypeId").GetValue(selectedRule),
                    AlertTypeName = (string)selectedRule.GetType().GetProperty("AlertTypeName").GetValue(selectedRule),
                    AlertValue = (int?)selectedRule.GetType().GetProperty("AlertValue").GetValue(selectedRule),
                    Severity = (string)selectedRule.GetType().GetProperty("Severity").GetValue(selectedRule),
                    CreatedAtUTC = (DateTime)selectedRule.GetType().GetProperty("CreatedAtUTC").GetValue(selectedRule),
                    UpdatedAtUTC = (DateTime)selectedRule.GetType().GetProperty("UpdatedAtUTC").GetValue(selectedRule),
                    IsActive = (bool)selectedRule.GetType().GetProperty("IsActive").GetValue(selectedRule),
                    ActiveInAllServers = (bool?)selectedRule.GetType().GetProperty("ActiveInAllServers").GetValue(selectedRule),
                    ActiveInAllEndpoints = (bool?)selectedRule.GetType().GetProperty("ActiveInAllEndpoints").GetValue(selectedRule),
                };

                var editWindow = new EditAlertRulesWindow(alertRule);
                editWindow.ShowDialog();
                LoadAlertRules(); // Refresh the grid after editing
            }
            else
            {
                MessageBox.Show("Please select a rule to edit.");
            }
        }

        // Event handler for Delete button click
        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            var selectedRule = AlertRulesDataGrid.SelectedItem;
            if (selectedRule != null)
            {
                
                var selectedRuleId = (int)((dynamic)selectedRule).Id;

                
                var result = MessageBox.Show("Are you sure you want to delete this rule?", "Delete Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Warning);

                if (result == MessageBoxResult.Yes)
                {
                    
                    var ruleToDelete = _context.AlertRules.FirstOrDefault(r => r.Id == selectedRuleId);

                    if (ruleToDelete != null)
                    {
                        _context.AlertRules.Remove(ruleToDelete);
                        _context.SaveChanges();
                        LoadAlertRules(); 
                    }
                }
            }
            else
            {
                MessageBox.Show("Please select a rule to delete.");
            }
        }


        // Event handler for Exit button click
        private void BtnExit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}

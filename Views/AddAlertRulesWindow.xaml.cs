using System;
using System.Linq;
using System.Windows;
using WpfRequestResponseLogger.Data;
using WpfRequestResponseLogger.Models;

namespace WpfRequestResponseLogger
{
    
    public partial class AddAlertRulesWindow : Window
    {
        private DataContext _context;

        public AddAlertRulesWindow()
        {
            InitializeComponent();
            _context = new DataContext(); // Initialize your DbContext
            LoadAlertTypes();
        }

        // Method to load AlertTypes into the ComboBox
        private void LoadAlertTypes()
        {
            var alertTypes = _context.AlertTypes.ToList(); // Assuming AlertTypes is a table in your DbContext
            cmbAlertTypeId.ItemsSource = alertTypes;
        }

        // Event handler for when the AlertType is selected
        private void CmbAlertTypeId_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            
        }

        // Event handler for the Save button click
        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            // Validate the input fields
            if (string.IsNullOrWhiteSpace(txtName.Text) ||
                
                string.IsNullOrWhiteSpace(txtSeverity.Text) ||
                cmbAlertTypeId.SelectedItem == null)
            {
                MessageBox.Show("Please fill in all required fields.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Create a new AlertRule object
            var newAlertRule = new AlertRule
            {
                Name = txtName.Text,
                AlertTypeId = ((AlertType)cmbAlertTypeId.SelectedItem).Id,
                AlertTypeName = ((AlertType)cmbAlertTypeId.SelectedItem).AlertTypeName,
                //AlertTypeName = txtAlertTypeName.Text,
                AlertValue = int.Parse(txtAlertValue.Text),
                //Expression = txtExpression.Text,
                Severity = txtSeverity.Text,
                IsActive = chkIsActive.IsChecked ?? false,
                ActiveInAllServers = chkActiveInAllServers.IsChecked ?? false,
                ActiveInAllEndpoints = chkActiveInAllEndpoints.IsChecked ?? false,
                CreatedAtUTC = DateTime.UtcNow,
                UpdatedAtUTC = DateTime.UtcNow
            };

            // Add the new alert rule to the database
            _context.AlertRules.Add(newAlertRule);
            _context.SaveChanges();

            // Close the window after saving
            this.Close();
        }

        // Event handler for the Exit button click
        private void BtnExit_Click(object sender, RoutedEventArgs e)
        {
            this.Close(); // Close the window without saving
        }
    }
}

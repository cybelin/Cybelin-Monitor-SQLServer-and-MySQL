using System;
using System.Linq;
using System.Windows;
using WpfRequestResponseLogger.Data;
using WpfRequestResponseLogger.Models;

namespace WpfRequestResponseLogger
{
    public partial class EditAlertRulesWindow : Window
    {
        private DataContext _context;
        private AlertRule _alertRule;

        public EditAlertRulesWindow(AlertRule alertRule)
        {
            InitializeComponent();
            _context = new DataContext(); // Initialize your DbContext
            _alertRule = alertRule; // Store the current alert rule to be edited
            DataContext = _alertRule; // Bind the alertRule object to the DataContext for data binding
            LoadAlertTypes();
        }

        // Method to load AlertTypes into the ComboBox
        private void LoadAlertTypes()
        {
            var alertTypes = _context.AlertTypes.ToList(); // Assuming AlertTypes is a table in your DbContext
            cmbAlertTypeId.ItemsSource = alertTypes;
            cmbAlertTypeId.SelectedValue = _alertRule.AlertTypeId; // Set the selected alert type based on the existing rule
        }

        // Event handler for the Save button click
        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            // Validate the input fields
            if (string.IsNullOrWhiteSpace(txtName.Text) ||
                string.IsNullOrWhiteSpace(txtAlertValue.Text) ||
                string.IsNullOrWhiteSpace(txtSeverity.Text) ||
                cmbAlertTypeId.SelectedItem == null)
            {
                MessageBox.Show("Please fill in all required fields.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Update the existing AlertRule object
            _alertRule.Name = txtName.Text;
            _alertRule.AlertTypeId = (int)cmbAlertTypeId.SelectedValue; // Get the selected AlertTypeId
            var selectedAlertType = (AlertType)cmbAlertTypeId.SelectedItem;
            if (selectedAlertType != null)
            {
                _alertRule.AlertTypeName = selectedAlertType.AlertTypeName; // Actualizar el AlertTypeName
            }
            _alertRule.AlertValue = int.Parse(txtAlertValue.Text);
            _alertRule.Severity = txtSeverity.Text;
            _alertRule.IsActive = chkIsActive.IsChecked ?? false;
            _alertRule.ActiveInAllServers = chkActiveInAllServers.IsChecked ?? false;
            _alertRule.ActiveInAllEndpoints = chkActiveInAllEndpoints.IsChecked ?? false;
            _alertRule.UpdatedAtUTC = DateTime.UtcNow;

            // Save the changes to the database
            _context.AlertRules.Update(_alertRule);
            _context.SaveChanges();

            // Close the window after saving
            this.Close();
        }

        // Event handler for the Exit button click
        private void BtnExit_Click(object sender, RoutedEventArgs e)
        {
            this.Close(); // Close the window without saving changes
        }
    }
}

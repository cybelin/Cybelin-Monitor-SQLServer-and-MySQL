using System;
using System.Windows;
using System.Windows.Controls;
using WpfRequestResponseLogger.Data; // Ensure correct namespace for DataContext
using WpfRequestResponseLogger.Models; // Ensure correct namespace for models

namespace WpfRequestResponseLogger.Views
{
    public partial class EditAlertInstanceWindow : Window
    {
        private readonly DataContext _context;
        private readonly AlertInstance _alertInstance;

        public EditAlertInstanceWindow(AlertInstance alertInstance)
        {
            InitializeComponent();
            _context = new DataContext();
            _alertInstance = alertInstance;

            // Load the data into the TextBoxes and ComboBox
            LoadAlertInstanceData();
        }

        // Load data from the alert instance into the form controls
        private void LoadAlertInstanceData()
        {
            AlertRuleNameTextBox.Text = _alertInstance.AlertRuleName;
            AlertTypeNameTextBox.Text = _alertInstance.AlertTypeName;
            AlertValueTextBox.Text = _alertInstance.AlertValue.ToString();
            SeverityTextBox.Text = _alertInstance.Severity;
            ServerNameTextBox.Text = _alertInstance.ServerName;
            ClientIPTextBox.Text = _alertInstance.ClientIP;
            EndpointPathTextBox.Text = _alertInstance.EndpointPath;
            TriggeredAtUTCTextBox.Text = _alertInstance.TriggeredAtUTC.ToString("yyyy-MM-dd HH:mm:ss");
            ResolvedAtUTCTextBox.Text = _alertInstance.ResolvedAtUTC?.ToString("yyyy-MM-dd HH:mm:ss");

            // Set ComboBox value based on the current status
            StatusComboBox.SelectedItem = GetComboBoxItem(_alertInstance.Status);
        }

        // Helper function to find the ComboBoxItem matching the status
        private ComboBoxItem GetComboBoxItem(string status)
        {
            foreach (ComboBoxItem item in StatusComboBox.Items)
            {
                if (item.Content.ToString() == status)
                {
                    return item;
                }
            }
            return null;
        }

        // Event handler for the Exit button click
        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        // Event handler for the Save button click
        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            // Validate the fields
            if (StatusComboBox.SelectedItem == null)
            {
                MessageBox.Show("Please select a valid status.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Update the alert instance with the new status
            var selectedStatus = (StatusComboBox.SelectedItem as ComboBoxItem)?.Content.ToString();
            _alertInstance.Status = selectedStatus;

            // If status is "Resolved" and ResolvedAtUTC is empty, set it to DateTime.UtcNow
            if (selectedStatus == "Resolved" && string.IsNullOrWhiteSpace(ResolvedAtUTCTextBox.Text))
            {
                _alertInstance.ResolvedAtUTC = DateTime.UtcNow;
                ResolvedAtUTCTextBox.Text = _alertInstance.ResolvedAtUTC.Value.ToString("yyyy-MM-dd HH:mm:ss");
            }

            // Save changes to the database
            try
            {
                _context.AlertInstances.Update(_alertInstance);
                _context.SaveChanges();
                MessageBox.Show("Changes saved successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while saving: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}

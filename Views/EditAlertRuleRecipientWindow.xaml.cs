using System;
using System.Linq;
using System.Windows;
using WpfRequestResponseLogger.Data;  


namespace WpfRequestResponseLogger.Views
{
    public partial class EditAlertRuleRecipientWindow : Window
    {
        private readonly DataContext _dbContext;
        private readonly int _alertRuleRecipientId;

        public EditAlertRuleRecipientWindow(int alertRuleRecipientId)
        {
            InitializeComponent();
            _dbContext = new DataContext();  // Initialize DbContext
            _alertRuleRecipientId = alertRuleRecipientId;
            LoadAlertRules();  // Load AlertRules into ComboBox
            LoadRecipients();  // Load Recipients into ComboBox
            LoadAlertRuleRecipientData();  // Load existing data into the form
        }

        // Load the list of AlertRules into the ComboBox
        private void LoadAlertRules()
        {
            var alertRules = _dbContext.AlertRules.ToList();
            AlertRuleComboBox.ItemsSource = alertRules;
        }

        // Load the list of Recipients into the ComboBox
        private void LoadRecipients()
        {
            var recipients = _dbContext.AlertRecipients.ToList();
            RecipientComboBox.ItemsSource = recipients;
        }

        // Load the existing data into the form
        private void LoadAlertRuleRecipientData()
        {
            var alertRuleRecipient = _dbContext.AlertRuleRecipients
                                              .FirstOrDefault(arr => arr.Id == _alertRuleRecipientId);

            if (alertRuleRecipient != null)
            {
                IdTextBox.Text = alertRuleRecipient.Id.ToString();
                AlertRuleComboBox.SelectedValue = alertRuleRecipient.AlertRuleId;
                RecipientComboBox.SelectedValue = alertRuleRecipient.RecipientId;
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            // Validate the input fields
            if (AlertRuleComboBox.SelectedItem == null || RecipientComboBox.SelectedItem == null)
            {
                MessageBox.Show("Please select both an alert rule and a recipient.");
                return;
            }

            // Get the selected AlertRuleId and RecipientId
            var selectedAlertRuleId = (int)AlertRuleComboBox.SelectedValue;
            var selectedRecipientId = (int)RecipientComboBox.SelectedValue;

            // Update the existing AlertRuleRecipient entry
            var alertRuleRecipient = _dbContext.AlertRuleRecipients
                                               .FirstOrDefault(arr => arr.Id == _alertRuleRecipientId);

            if (alertRuleRecipient != null)
            {
                alertRuleRecipient.AlertRuleId = selectedAlertRuleId;
                alertRuleRecipient.RecipientId = selectedRecipientId;

                // Save the updated entry to the database
                try
                {
                    _dbContext.SaveChanges();
                    MessageBox.Show("Alert rule recipient updated successfully.");
                    this.Close();  // Close the window after saving
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error updating alert rule recipient: " + ex.Message);
                }
            }
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            // Close the window without saving changes
            this.Close();
        }
    }
}

using System;
using System.Linq;
using System.Windows;
using WpfRequestResponseLogger.Data;  // Adjust according to your DataContext namespace
using WpfRequestResponseLogger.Models;  // Adjust according to your models

namespace WpfRequestResponseLogger.Views
{
    public partial class AddAlertRuleRecipientWindow : Window
    {
        private readonly DataContext _dbContext;

        public AddAlertRuleRecipientWindow()
        {
            InitializeComponent();
            _dbContext = new DataContext();  // Initialize DbContext
            LoadAlertRules();  // Load AlertRules into ComboBox
            LoadRecipients();  // Load Recipients into ComboBox
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

            // Create a new AlertRuleRecipient entry
            var newAlertRuleRecipient = new AlertRuleRecipient
            {
                AlertRuleId = selectedAlertRuleId,
                RecipientId = selectedRecipientId
            };

            // Save the new entry to the database
            try
            {
                _dbContext.AlertRuleRecipients.Add(newAlertRuleRecipient);
                _dbContext.SaveChanges();
                MessageBox.Show("Alert rule recipient added successfully.");
                this.Close();  // Close the window after saving
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error saving alert rule recipient: " + ex.Message);
            }
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            // Close the window without adding anything to the database
            this.Close();
        }
    }
}

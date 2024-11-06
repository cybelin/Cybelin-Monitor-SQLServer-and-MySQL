using System;
using System.Linq;
using System.Windows;
using WpfRequestResponseLogger.Data; 
using WpfRequestResponseLogger.Models; 

namespace WpfRequestResponseLogger.Views
{
    public partial class AddAlertSilencedWindow : Window
    {
        private readonly DataContext _dbContext;

        public AddAlertSilencedWindow()
        {
            InitializeComponent();
            _dbContext = new DataContext();  // Initialize DbContext
            LoadAlertRules();  // Load AlertRules into ComboBox
        }

        // Load the list of Alert Rules into the ComboBox
        private void LoadAlertRules()
        {
            var alertRules = _dbContext.AlertRules.ToList();
            AlertRuleComboBox.ItemsSource = alertRules;
        }

        // Set current UTC time in "Silenced From" and "Silenced Until" text boxes
        private void GetUtcNowButton_Click(object sender, RoutedEventArgs e)
        {
            string utcNow = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss");
            SilencedFromTextBox.Text = utcNow;
            SilencedUntilTextBox.Text = utcNow;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            // Validate input fields
            if (AlertRuleComboBox.SelectedItem == null ||
                string.IsNullOrWhiteSpace(SilencedFromTextBox.Text) ||
                string.IsNullOrWhiteSpace(SilencedUntilTextBox.Text) ||
                string.IsNullOrWhiteSpace(ReasonTextBox.Text))
            {
                MessageBox.Show("Please fill in all the required fields.");
                return;
            }

            if (!DateTime.TryParse(SilencedFromTextBox.Text, out DateTime silencedFrom))
            {
                MessageBox.Show("Silenced From must be a valid date/time.");
                return;
            }

            if (!DateTime.TryParse(SilencedUntilTextBox.Text, out DateTime silencedUntil))
            {
                MessageBox.Show("Silenced Until must be a valid date/time.");
                return;
            }

            // Get the selected AlertRuleId from the ComboBox
            var selectedAlertRuleId = (int)AlertRuleComboBox.SelectedValue;

            // Create a new AlertSilenced entry
            var newAlertSilenced = new AlertSilenced
            {
                AlertRuleId = selectedAlertRuleId,
                SilencedFromUTC = silencedFrom,
                SilencedUntilUTC = silencedUntil,
                Reason = ReasonTextBox.Text
            };

            // Save the new entry to the database
            try
            {
                _dbContext.AlertSilenced.Add(newAlertSilenced);
                _dbContext.SaveChanges();
                MessageBox.Show("Silenced alert added successfully.");
                this.Close();  // Close the window after saving
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error saving silenced alert: " + ex.Message);
            }
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            // Close the window without adding anything to the database
            this.Close();
        }
    }
}

using System;
using System.Linq;
using System.Windows;
using WpfRequestResponseLogger.Data; 
using WpfRequestResponseLogger.Models; 

namespace WpfRequestResponseLogger.Views
{
    public partial class EditAlertSilencedWindow : Window
    {
        private readonly DataContext _dbContext;
        private readonly AlertSilenced _alertSilenced;  // The alert being edited

        public EditAlertSilencedWindow(AlertSilenced alertSilenced)
        {
            InitializeComponent();
            _dbContext = new DataContext();  // Initialize DbContext
            _alertSilenced = alertSilenced;   // Store the record being edited
            LoadAlertRules();                 // Load Alert Rules into ComboBox
            LoadAlertSilencedData();           // Load the existing data into the form
        }

        // Load the list of Alert Rules into the ComboBox
        private void LoadAlertRules()
        {
            var alertRules = _dbContext.AlertRules.ToList();
            AlertRuleComboBox.ItemsSource = alertRules;
        }

        // Load the current alert data into the form fields
        private void LoadAlertSilencedData()
        {
            IdTextBox.Text = _alertSilenced.Id.ToString();
            AlertRuleComboBox.SelectedValue = _alertSilenced.AlertRuleId;
            SilencedFromTextBox.Text = _alertSilenced.SilencedFromUTC.ToString("yyyy-MM-dd HH:mm:ss");
            SilencedUntilTextBox.Text = _alertSilenced.SilencedUntilUTC.ToString("yyyy-MM-dd HH:mm:ss");
            ReasonTextBox.Text = _alertSilenced.Reason;
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
            // Validate the input fields
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

            // Update the AlertSilenced entry
            _alertSilenced.AlertRuleId = (int)AlertRuleComboBox.SelectedValue;
            _alertSilenced.SilencedFromUTC = silencedFrom;
            _alertSilenced.SilencedUntilUTC = silencedUntil;
            _alertSilenced.Reason = ReasonTextBox.Text;

            // Save the updated entry to the database
            try
            {
                _dbContext.AlertSilenced.Update(_alertSilenced);
                _dbContext.SaveChanges();
                MessageBox.Show("Silenced alert updated successfully.");
                this.Close();  // Close the window after saving
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error saving silenced alert: " + ex.Message);
            }
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            // Close the window without saving changes
            this.Close();
        }
    }
}

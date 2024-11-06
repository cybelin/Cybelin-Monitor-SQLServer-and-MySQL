using System;
using System.Windows;
using WpfRequestResponseLogger.Models; 

namespace WpfRequestResponseLogger
{
    public partial class EditServerConfiguration : Window
    {
        private string _connectionString;
        private Configuration _selectedConfiguration;

        public EditServerConfiguration(string connectionString, Configuration selectedConfiguration)
        {
            InitializeComponent();
            _connectionString = connectionString;
            _selectedConfiguration = selectedConfiguration;

            // Load the configuration data into the TextBoxes
            LoadConfigurationData();
        }

        // Load the configuration data into the UI
        private void LoadConfigurationData()
        {
            IdTextBox.Text = _selectedConfiguration.Id.ToString();
            KeyTextBox.Text = _selectedConfiguration.Key;
            ValueTextBox.Text = _selectedConfiguration.Value;
            LastUpdatedTextBox.Text = _selectedConfiguration.LastUpdated.ToString("yyyy-MM-dd HH:mm:ss");
        }

        // Event handler for the Save button click
        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            // Validate the input fields
            if (string.IsNullOrEmpty(KeyTextBox.Text))
            {
                MessageBox.Show("The Key field cannot be empty.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (string.IsNullOrEmpty(ValueTextBox.Text))
            {
                MessageBox.Show("The Value field cannot be empty.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (!DateTime.TryParse(LastUpdatedTextBox.Text, out DateTime lastUpdated))
            {
                MessageBox.Show("The LastUpdated field must be a valid date.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Update the selected configuration with the new values
            _selectedConfiguration.Key = KeyTextBox.Text;
            _selectedConfiguration.Value = ValueTextBox.Text;
            _selectedConfiguration.LastUpdated = lastUpdated;

            // Save the changes to the database
            using (var context = new DataContext2(_connectionString))
            {
                context.Configurations.Update(_selectedConfiguration);
                context.SaveChanges();
            }

            // Close the window and notify the parent window to refresh the grid
            this.DialogResult = true; // This will indicate the save was successful
            this.Close();
        }

        // Event handler for the Exit button click
        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close(); // Close the window without saving
        }
    }
}

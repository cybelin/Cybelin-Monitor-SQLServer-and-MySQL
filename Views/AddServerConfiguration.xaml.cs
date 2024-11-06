using System;
using System.Windows;
using WpfRequestResponseLogger.Models; 

namespace WpfRequestResponseLogger
{
    public partial class AddServerConfiguration : Window
    {
        private string _connectionString;

        public AddServerConfiguration(string connectionString)
        {
            InitializeComponent();
            _connectionString = connectionString;

            // Automatically fill the LastUpdated field with the current UTC time
            LastUpdatedTextBox.Text = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss");
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

            // Add the new Configuration to the database
            using (var context = new DataContext2(_connectionString)) // Use the connection string of the selected server
            {
                var newConfiguration = new Configuration
                {
                    Key = KeyTextBox.Text,
                    Value = ValueTextBox.Text,
                    LastUpdated = lastUpdated
                };

                context.Configurations.Add(newConfiguration);
                context.SaveChanges();
            }

            // Close the window and notify the parent to update the grid
            this.DialogResult = true; // This will allow us to check if the save was successful
            this.Close();
        }

        // Event handler for the Exit button click
        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close(); // Close the window without saving
        }
    }
}

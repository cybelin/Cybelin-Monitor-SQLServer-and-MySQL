using System;
using System.Windows;
using WpfRequestResponseLogger.Data;

namespace WpfRequestResponseLogger.Views
{
    public partial class AddConfigurationWindow : Window
    {
        
        public event Action OnConfigurationAdded;

        public AddConfigurationWindow()
        {
            InitializeComponent();
            SetLastUpdatedToUTC(); 
        }

        
        private void SetLastUpdatedToUTC()
        {
            LastUpdatedTextBox.Text = GetTimeUTC();
        }

        
        private string GetTimeUTC()
        {
            return DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss");
        }

        
        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (!ValidateFields())
                return;

            try
            {
                
                var newConfig = new Models.Configuration
                {
                    Key = KeyTextBox.Text,
                    Value = ValueTextBox.Text,
                    LastUpdated = DateTime.Parse(LastUpdatedTextBox.Text)
                };

                
                AddConfigurationToDatabase(newConfig);

                MessageBox.Show("Configuration added successfully!");

                
                OnConfigurationAdded?.Invoke();

                
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error adding configuration: {ex.Message}");
            }
        }

        
        private void AddConfigurationToDatabase(Models.Configuration newConfig)
        {
            using (var context = new DataContext())
            {
                context.MonitorConfigurations.Add(newConfig);
                context.SaveChanges(); 
            }
        }

        
        private bool ValidateFields()
        {
            if (string.IsNullOrEmpty(KeyTextBox.Text))
            {
                MessageBox.Show("Key cannot be empty.");
                return false;
            }

            if (string.IsNullOrEmpty(ValueTextBox.Text))
            {
                MessageBox.Show("Value cannot be empty.");
                return false;
            }

            if (!DateTime.TryParse(LastUpdatedTextBox.Text, out _))
            {
                MessageBox.Show("Invalid date format. Please use a valid date.");
                return false;
            }

            return true;
        }

        
        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}

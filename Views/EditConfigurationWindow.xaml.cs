using System;
using System.Windows;
using WpfRequestResponseLogger.Data;

namespace WpfRequestResponseLogger.Views
{
    public partial class EditConfigurationWindow : Window
    {
        private Models.Configuration _configuration;

        
        public EditConfigurationWindow(Models.Configuration configuration)
        {
            InitializeComponent();
            _configuration = configuration;
            LoadConfigurationData(); 
            SetLastUpdatedToUTC(); 
        }

        
        private void LoadConfigurationData()
        {
            IdTextBox.Text = _configuration.Id.ToString();
            KeyTextBox.Text = _configuration.Key;
            ValueTextBox.Text = _configuration.Value;
            LastUpdatedTextBox.Text = _configuration.LastUpdated.ToString("yyyy-MM-dd HH:mm:ss");
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
                
                _configuration.Key = KeyTextBox.Text;
                _configuration.Value = ValueTextBox.Text;
                _configuration.LastUpdated = DateTime.Parse(LastUpdatedTextBox.Text);

                
                SaveConfigurationChangesToDatabase(_configuration);

                MessageBox.Show("Configuration updated successfully!");
                this.Close(); 
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving configuration: {ex.Message}");
            }
        }

        
        private void SaveConfigurationChangesToDatabase(Models.Configuration configuration)
        {
            using (var context = new DataContext())
            {
                var configToUpdate = context.MonitorConfigurations.Find(configuration.Id);

                if (configToUpdate != null)
                {
                    configToUpdate.Key = configuration.Key;
                    configToUpdate.Value = configuration.Value;
                    configToUpdate.LastUpdated = configuration.LastUpdated;

                    context.SaveChanges();
                }
            }
        }

        
        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
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
    }
}

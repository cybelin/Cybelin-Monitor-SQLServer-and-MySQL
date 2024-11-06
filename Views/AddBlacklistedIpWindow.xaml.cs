using System;
using System.Windows;
using WpfRequestResponseLogger.Data;

namespace WpfRequestResponseLogger.Views
{
    public partial class AddBlacklistedIpWindow : Window
    {
        
        public event Action OnIpAdded;

        public AddBlacklistedIpWindow()
        {
            InitializeComponent();
            SetDateAddedToUTC(); // 
        }

        
        private void SetDateAddedToUTC()
        {
            DateAddedTextBox.Text = GetTimeUTC();
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
                
                var newIp = new Models.BlacklistedIp
                {
                    IpAddress = IpAddressTextBox.Text,
                    DateAdded = DateTime.Parse(DateAddedTextBox.Text),
                    Reason = ReasonTextBox.Text,
                    IsActive = IsActiveCheckBox.IsChecked == true
                };

                
                AddBlacklistedIpToDatabase(newIp);

                MessageBox.Show("IP added successfully!");

                
                OnIpAdded?.Invoke();

                
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error adding IP: {ex.Message}");
            }
        }

        
        private void AddBlacklistedIpToDatabase(Models.BlacklistedIp newIp)
        {
            using (var context = new DataContext())
            {
                context.MonitorBlacklistedIps.Add(newIp);
                context.SaveChanges(); 
            }
        }

        
        private bool ValidateFields()
        {
            if (string.IsNullOrEmpty(IpAddressTextBox.Text))
            {
                MessageBox.Show("IP Address cannot be empty.");
                return false;
            }

            if (!DateTime.TryParse(DateAddedTextBox.Text, out _))
            {
                MessageBox.Show("Invalid date format. Please use a valid date.");
                return false;
            }

            if (string.IsNullOrEmpty(ReasonTextBox.Text))
            {
                MessageBox.Show("Reason cannot be empty.");
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

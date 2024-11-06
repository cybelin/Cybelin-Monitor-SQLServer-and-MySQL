using System;
using System.Text.RegularExpressions;
using System.Windows;
using WpfRequestResponseLogger.Data;

namespace WpfRequestResponseLogger.Views
{
    public partial class AddWhitelistedIpWindow : Window
    {
        public event Action OnWhitelistedIpAdded; 

        public AddWhitelistedIpWindow()
        {
            InitializeComponent();
            SetDateAddedToUTC(); 
        }

        
        private void SetDateAddedToUTC()
        {
            DateAddedTextBox.Text = GetTimeUTC();
        }

        
        private string GetTimeUTC()
        {
            return DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss");
        }

        
        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            if (!ValidateFields())
                return;

            try
            {
                
                var newIp = new Models.WhitelistedIp
                {
                    IpAddress = IpAddressTextBox.Text,
                    DateAdded = DateTime.Parse(DateAddedTextBox.Text),
                    Reason = ReasonTextBox.Text,
                    IsActive = IsActiveCheckBox.IsChecked == true
                };

                
                AddWhitelistedIpToDatabase(newIp);

                MessageBox.Show("Whitelisted IP added successfully!");

                
                OnWhitelistedIpAdded?.Invoke();

                
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error adding IP: {ex.Message}");
            }
        }

        
        private void AddWhitelistedIpToDatabase(Models.WhitelistedIp newIp)
        {
            using (var context = new DataContext())
            {
                context.WhitelistedIps.Add(newIp);
                context.SaveChanges(); 
            }
        }

        
        private bool ValidateFields()
        {
            if (!IsValidIpAddress(IpAddressTextBox.Text))
            {
                MessageBox.Show("Invalid IP Address. Please enter a valid IPv4 or IPv6 address.");
                return false;
            }

            if (string.IsNullOrEmpty(ReasonTextBox.Text))
            {
                MessageBox.Show("Reason cannot be empty.");
                return false;
            }

            return true;
        }

        
        private bool IsValidIpAddress(string ipAddress)
        {
            
            string ipv4Pattern = @"^(([0-9]{1,3}\.){3}[0-9]{1,3})$";

            
            string ipv6Pattern = @"^(([0-9a-fA-F]{1,4}:){1,7}[0-9a-fA-F]{1,4}|::([0-9a-fA-F]{1,4}:){0,5}[0-9a-fA-F]{1,4})$";

            
            return Regex.IsMatch(ipAddress, ipv4Pattern) || Regex.IsMatch(ipAddress, ipv6Pattern);
        }


        
        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}

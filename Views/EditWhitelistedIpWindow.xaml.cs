using System;
using System.Text.RegularExpressions;
using System.Windows;
using WpfRequestResponseLogger.Data;

namespace WpfRequestResponseLogger.Views
{
    public partial class EditWhitelistedIpWindow : Window
    {
        private Models.WhitelistedIp _whitelistedIp;

        
        public EditWhitelistedIpWindow(Models.WhitelistedIp whitelistedIp)
        {
            InitializeComponent();
            _whitelistedIp = whitelistedIp;
            LoadWhitelistedIpData(); 
        }

        
        private void LoadWhitelistedIpData()
        {
            IdTextBox.Text = _whitelistedIp.Id.ToString();
            IpAddressTextBox.Text = _whitelistedIp.IpAddress;
            DateAddedTextBox.Text = _whitelistedIp.DateAdded.ToString("yyyy-MM-dd HH:mm:ss");
            ReasonTextBox.Text = _whitelistedIp.Reason;
            IsActiveCheckBox.IsChecked = _whitelistedIp.IsActive;
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
            
            string ipv4Pattern = @"^([0-9]{1,3}\.){3}[0-9]{1,3}$";
            
            string ipv6Pattern = @"^([0-9a-fA-F]{1,4}:){7}([0-9a-fA-F]{1,4}|:)$";

            return Regex.IsMatch(ipAddress, ipv4Pattern) || Regex.IsMatch(ipAddress, ipv6Pattern);
        }

        
        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (!ValidateFields())
                return;

            try
            {
                
                _whitelistedIp.IpAddress = IpAddressTextBox.Text;
                _whitelistedIp.DateAdded = DateTime.Parse(DateAddedTextBox.Text);
                _whitelistedIp.Reason = ReasonTextBox.Text;
                _whitelistedIp.IsActive = IsActiveCheckBox.IsChecked == true;

                
                SaveWhitelistedIpChangesToDatabase(_whitelistedIp);

                MessageBox.Show("Whitelisted IP updated successfully!");
                this.Close(); 
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving IP: {ex.Message}");
            }
        }

        
        private void SaveWhitelistedIpChangesToDatabase(Models.WhitelistedIp whitelistedIp)
        {
            using (var context = new DataContext())
            {
                var ipToUpdate = context.WhitelistedIps.Find(whitelistedIp.Id);

                if (ipToUpdate != null)
                {
                    ipToUpdate.IpAddress = whitelistedIp.IpAddress;
                    ipToUpdate.DateAdded = whitelistedIp.DateAdded;
                    ipToUpdate.Reason = whitelistedIp.Reason;
                    ipToUpdate.IsActive = whitelistedIp.IsActive;

                    context.SaveChanges();
                }
            }
        }

        
        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}

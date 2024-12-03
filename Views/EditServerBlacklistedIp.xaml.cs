using System;
using System.Text.RegularExpressions;
using System.Windows;
using WpfRequestResponseLogger.Models; 

namespace WpfRequestResponseLogger
{
    public partial class EditServerBlacklistedIp : Window
    {
        private string _connectionString;
        private string _serverType;
        private BlacklistedIp _selectedBlacklistedIp;

        public EditServerBlacklistedIp(string connectionString, string serverType, BlacklistedIp selectedBlacklistedIp)
        {
            InitializeComponent();
            _connectionString = connectionString;
            _serverType = serverType;
            _selectedBlacklistedIp = selectedBlacklistedIp;

            // Load the selected Blacklisted IP data into the TextBoxes
            LoadBlacklistedIpData();
        }

        // Load the selected Blacklisted IP data into the UI controls
        private void LoadBlacklistedIpData()
        {
            IdTextBox.Text = _selectedBlacklistedIp.Id.ToString();
            IpAddressTextBox.Text = _selectedBlacklistedIp.IpAddress;
            DateAddedTextBox.Text = _selectedBlacklistedIp.DateAdded.ToString("yyyy-MM-dd HH:mm:ss");
            ReasonTextBox.Text = _selectedBlacklistedIp.Reason;
            IsActiveCheckBox.IsChecked = _selectedBlacklistedIp.IsActive;
        }

        // Event handler for the Save button click
        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            // Validate the input fields
            if (!IsValidIpAddress(IpAddressTextBox.Text))
            {
                MessageBox.Show("The IP address must be a valid IPv4 or IPv6 address.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (!DateTime.TryParse(DateAddedTextBox.Text, out DateTime dateAdded))
            {
                MessageBox.Show("The Date Added field must be a valid date.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (string.IsNullOrEmpty(ReasonTextBox.Text))
            {
                MessageBox.Show("The Reason field cannot be empty.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Update the selected Blacklisted IP with the new values
            _selectedBlacklistedIp.IpAddress = IpAddressTextBox.Text;
            _selectedBlacklistedIp.DateAdded = dateAdded;
            _selectedBlacklistedIp.Reason = ReasonTextBox.Text;
            _selectedBlacklistedIp.IsActive = IsActiveCheckBox.IsChecked ?? false;

            // Save the changes to the database
            using (var context = new DataContext2(_connectionString, _serverType))  
            {
                context.BlacklistedIps.Update(_selectedBlacklistedIp);
                context.SaveChanges();
            }

            // Close the window and notify the parent to refresh the grid
            this.DialogResult = true; // This will indicate that the save was successful
            this.Close();
        }

        // Validate if the provided IP address is valid IPv4 or IPv6
        private bool IsValidIpAddress(string ipAddress)
        {
            // Regex for IPv4 and IPv6 validation
            string ipv4Pattern = @"^(\d{1,3}\.){3}\d{1,3}$";
            string ipv6Pattern = @"^([0-9a-fA-F]{1,4}:){7,7}[0-9a-fA-F]{1,4}$|^([0-9a-fA-F]{1,4}:){1,7}:$|^([0-9a-fA-F]{1,4}:){1,6}:[0-9a-fA-F]{1,4}$|^([0-9a-fA-F]{1,4}:){1,5}(:[0-9a-fA-F]{1,4}){1,2}$|^([0-9a-fA-F]{1,4}:){1,4}(:[0-9a-fA-F]{1,4}){1,3}$|^([0-9a-fA-F]{1,4}:){1,3}(:[0-9a-fA-F]{1,4}){1,4}$|^([0-9a-fA-F]{1,4}:){1,2}(:[0-9a-fA-F]{1,4}){1,5}$|^[0-9a-fA-F]{1,4}:(:[0-9a-fA-F]{1,4}){1,6}$|^:((:[0-9a-fA-F]{1,4}){1,7}|:)$|^fe80:(:[0-9a-fA-F]{0,4}){0,4}%[0-9a-zA-Z]{1,}$|^::(ffff(:0{1,4}){0,1}:){0,1}(\d{1,3}\.){3}\d{1,3}$|^([0-9a-fA-F]{1,4}:){1,4}:(\d{1,3}\.){3}\d{1,3}$";

            return Regex.IsMatch(ipAddress, ipv4Pattern) || Regex.IsMatch(ipAddress, ipv6Pattern);
        }

        // Event handler for the Exit button click
        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close(); // Close the window without saving changes
        }
    }
}

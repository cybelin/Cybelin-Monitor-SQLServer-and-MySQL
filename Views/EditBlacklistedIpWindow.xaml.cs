using System;
using System.Windows;
using WpfRequestResponseLogger.Models;
using WpfRequestResponseLogger;
using WpfRequestResponseLogger.Data;
using System.Linq;
using System.Text.RegularExpressions;


namespace WpfRequestResponseLogger.Views
{
    public partial class EditBlacklistedIpWindow : Window
    {
        private BlacklistedIp _blacklistedIp;

        // Constructor
        public EditBlacklistedIpWindow(BlacklistedIp blacklistedIp)
        {
            InitializeComponent();
            _blacklistedIp = blacklistedIp;
            LoadIpData();
        }

        // Load data into the TextBoxes
        private void LoadIpData()
        {
            IdTextBox.Text = _blacklistedIp.Id.ToString();
            IpAddressTextBox.Text = _blacklistedIp.IpAddress;
            DateAddedTextBox.Text = _blacklistedIp.DateAdded.ToString("yyyy-MM-dd HH:mm:ss");
            ReasonTextBox.Text = _blacklistedIp.Reason;
            IsActiveCheckBox.IsChecked = _blacklistedIp.IsActive;
        }

        // Save changes to the database
        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (!ValidateFields())
                return;

            try
            {
                
                _blacklistedIp.IpAddress = IpAddressTextBox.Text;
                _blacklistedIp.DateAdded = DateTime.Parse(DateAddedTextBox.Text);
                _blacklistedIp.Reason = ReasonTextBox.Text;
                _blacklistedIp.IsActive = IsActiveCheckBox.IsChecked == true;

                
                SaveChangesToDatabase(_blacklistedIp);

                MessageBox.Show("Changes saved successfully!");
                this.Close(); 
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving changes: {ex.Message}");
            }
        }



        // Method to save changes to the database (replace with your actual save logic)
        private void SaveChangesToDatabase(Models.BlacklistedIp updatedIp)
        {
            try
            {
                using (var context = new DataContext())
                {
                    
                    var ipToUpdate = context.MonitorBlacklistedIps.FirstOrDefault(ip => ip.Id == updatedIp.Id);

                    if (ipToUpdate != null)
                    {
                        
                        ipToUpdate.IpAddress = updatedIp.IpAddress;
                        ipToUpdate.DateAdded = updatedIp.DateAdded;
                        ipToUpdate.Reason = updatedIp.Reason;
                        ipToUpdate.IsActive = updatedIp.IsActive;

                        
                        context.SaveChanges();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error updating IP in database: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private bool ValidateFields()
        {
            if (string.IsNullOrEmpty(IpAddressTextBox.Text))
            {
                MessageBox.Show("IP Address cannot be empty.");
                return false;
            }

            // Validar que la dirección IP sea válida en formato IPv4 o IPv6 usando Regex
            if (!IsValidIp(IpAddressTextBox.Text))
            {
                MessageBox.Show("Invalid IP Address. Please enter a valid IPv4 or IPv6 address.");
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

        // Método para validar la IP usando expresiones regulares
        private bool IsValidIp(string ipAddress)
        {
            // Expresión regular para validar IPv4
            string ipv4Pattern = @"^([0-9]{1,3}\.){3}[0-9]{1,3}$";

            // Expresión regular para validar IPv6
            string ipv6Pattern = @"^([0-9a-fA-F]{1,4}:){7}([0-9a-fA-F]{1,4}|:)$";

            // Validar si es IPv4 o IPv6
            return Regex.IsMatch(ipAddress, ipv4Pattern) || Regex.IsMatch(ipAddress, ipv6Pattern);
        }

        // Exit button handler
        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }

    // Example class representing the BlacklistedIp entity (replace with your actual model)
    
    /*
    public class BlacklistedIp
    {
        public int Id { get; set; }
        public string IpAddress { get; set; }
        public DateTime DateAdded { get; set; }
        public string Reason { get; set; }
        public bool IsActive { get; set; }
    }
    */
}

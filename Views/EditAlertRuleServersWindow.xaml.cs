using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Windows;
using WpfRequestResponseLogger.Data; 


namespace WpfRequestResponseLogger
{
    public partial class EditAlertRuleServers : Window
    {
        private readonly DataContext _context;
        private readonly int _alertRulesServerId; // Store the Id of the AlertRulesServer being edited

        public EditAlertRuleServers(int alertRulesServerId)
        {
            InitializeComponent();
            _context = new DataContext();
            _alertRulesServerId = alertRulesServerId;
            LoadComboBoxes();
            LoadData();
        }

        // Load Alert Rules and Servers into ComboBoxes
        private void LoadComboBoxes()
        {
            // Load Alert Rules into the ComboBox
            var alertRules = _context.AlertRules.Select(ar => new { ar.Id, ar.Name }).ToList();
            AlertRuleComboBox.ItemsSource = alertRules;
            AlertRuleComboBox.DisplayMemberPath = "Name"; // Show Name in ComboBox
            AlertRuleComboBox.SelectedValuePath = "Id"; // Use Id as value

            // Load Servers into the ComboBox
            var servers = _context.Servers.Select(s => new { s.Id, s.ServerName }).ToList();
            ServerComboBox.ItemsSource = servers;
            ServerComboBox.DisplayMemberPath = "ServerName"; // Show ServerName in ComboBox
            ServerComboBox.SelectedValuePath = "Id"; // Use Id as value
        }

        // Load data for the selected AlertRulesServer
        private void LoadData()
        {
            var alertRuleServer = _context.AlertRulesServers.Find(_alertRulesServerId);

            if (alertRuleServer != null)
            {
                IdTextBox.Text = alertRuleServer.Id.ToString();
                AlertRuleComboBox.SelectedValue = alertRuleServer.AlertRuleId;
                ServerComboBox.SelectedValue = alertRuleServer.ServerId;
            }
            else
            {
                MessageBox.Show("Alert Rule Server not found.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                this.Close(); // Close the window if the record is not found
            }
        }

        // Event handler for the Save button
        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            
            if (AlertRuleComboBox.SelectedValue == null || ServerComboBox.SelectedValue == null)
            {
                MessageBox.Show("Please select both an Alert Rule and a Server.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            
            var alertRuleServer = _context.AlertRulesServers.Find(_alertRulesServerId);
            if (alertRuleServer != null)
            {
                
                alertRuleServer.AlertRuleId = (int)AlertRuleComboBox.SelectedValue;
                alertRuleServer.ServerId = (int)ServerComboBox.SelectedValue;

                try
                {
                    _context.SaveChanges(); 
                    MessageBox.Show("Alert Rule Server updated successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    this.DialogResult = true; 
                    this.Close(); 
                }
                catch (DbUpdateException ex)
                {
                    
                    MessageBox.Show($"An error occurred while saving: {ex.InnerException?.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }


        // Event handler for the Exit button
        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close(); // Close the current window
        }
    }
}

using System;
using System.Linq;
using System.Windows;
using WpfRequestResponseLogger.Data;
using WpfRequestResponseLogger.Models;

namespace WpfRequestResponseLogger
{
    public partial class AddAlertRuleServers : Window
    {
        private readonly DataContext _context;

        public AddAlertRuleServers()
        {
            InitializeComponent();
            _context = new DataContext();
            LoadComboBoxes();
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

        // Event handler for the Save button
        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            // Validate selected values
            if (AlertRuleComboBox.SelectedValue == null || ServerComboBox.SelectedValue == null)
            {
                MessageBox.Show("Please select both an Alert Rule and a Server.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Create a new AlertRulesServer object
            var newAlertRuleServer = new AlertRulesServer
            {
                AlertRuleId = (int)AlertRuleComboBox.SelectedValue,
                ServerId = (int)ServerComboBox.SelectedValue
            };

            // Add to the database
            _context.AlertRulesServers.Add(newAlertRuleServer);
            _context.SaveChanges(); // Save changes to the database

            MessageBox.Show("Alert Rule Server added successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            this.Close(); // Close the window after saving
        }

        // Event handler for the Exit button
        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close(); // Close the current window
        }
    }
}

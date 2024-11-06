using System;
using System.Linq;
using System.Windows;
using WpfRequestResponseLogger.Data;  
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace WpfRequestResponseLogger.Views
{
    public partial class ManageAlertRuleRecipientsWindow : Window
    {
        private readonly DataContext _dbContext;

        public ManageAlertRuleRecipientsWindow()
        {
            InitializeComponent();
            _dbContext = new DataContext();  
            LoadAlertRuleRecipients();
        }

        // Load the data from the stored procedure and bind it to the DataGrid
        private void LoadAlertRuleRecipients()
        {
            var alertRuleRecipients = _dbContext.AlertRuleRecipientDetails
                                                .FromSqlRaw("EXEC GetAlertRuleRecipientsWithDetails")
                                                .ToList();

            AlertRuleRecipientsGrid.ItemsSource = alertRuleRecipients;
        }

        // Class to represent the data structure from the stored procedure
        public class AlertRuleRecipientDetail
        {
            public int AlertRuleRecipientId { get; set; }
            public string AlertRuleName { get; set; }
            public string RecipientName { get; set; }
            public int AlertRuleId { get; set; }
            public int RecipientId { get; set; }
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            // Open the AddAlertRuleRecipientWindow
            AddAlertRuleRecipientWindow addWindow = new AddAlertRuleRecipientWindow();
            addWindow.ShowDialog();

            // Reload the data after adding a new record
            LoadAlertRuleRecipients();
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            // Get the selected item from the DataGrid as dynamic
            var selectedItem = AlertRuleRecipientsGrid.SelectedItem;

            if (selectedItem == null)
            {
                MessageBox.Show("Please select an alert rule recipient to edit.");
                return;
            }

            // Retrieve the AlertRuleRecipientId dynamically
            var alertRuleRecipientId = (int)((dynamic)selectedItem).AlertRuleRecipientId;

            // Find the AlertRuleRecipient using the selected AlertRuleRecipientId
            var alertRuleRecipient = _dbContext.AlertRuleRecipients
                                               .FirstOrDefault(arr => arr.Id == alertRuleRecipientId);

            if (alertRuleRecipient == null)
            {
                MessageBox.Show("The selected alert rule recipient could not be found.");
                return;
            }

            // Open the EditAlertRuleRecipientWindow with the selected AlertRuleRecipient Id
            EditAlertRuleRecipientWindow editWindow = new EditAlertRuleRecipientWindow(alertRuleRecipient.Id);
            editWindow.ShowDialog();

            // Reload the data after editing
            LoadAlertRuleRecipients();
        }


        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            // Logic to delete the selected alert rule recipient
            MessageBox.Show("Delete button clicked!");
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            // Close the window
            this.Close();
        }
    }
}

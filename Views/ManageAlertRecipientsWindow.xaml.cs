using System;
using System.Linq;
using System.Linq.Expressions;
using System.Windows;
using WpfRequestResponseLogger.Data;
using WpfRequestResponseLogger.Models;
using System.Windows.Controls;
using WpfRequestResponseLogger.Views;




namespace WpfRequestResponseLogger.Views
{
    public partial class ManageAlertRecipientsWindow : Window
    {
        public ManageAlertRecipientsWindow()
        {
            InitializeComponent();
            LoadAlertRecipients();
        }

        private void LoadAlertRecipients()
        {
            
            using (var context = new DataContext())
            {
                var recipients = context.AlertRecipients.ToList();
                alertRecipientsGrid.ItemsSource = recipients;
            }
        }

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            this.Close(); 
        }

        

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            AddAlertRecipientWindow addWindow = new AddAlertRecipientWindow(this);
            addWindow.ShowDialog();
            
            if (addWindow.IsSaved)
            {
                LoadAlertRecipients(); 
            }
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            
            var selectedRecipient = (AlertRecipient)alertRecipientsGrid.SelectedItem;

            if (selectedRecipient == null)
            {
                MessageBox.Show("Please select a recipient to edit.", "Selection Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            
            EditAlertRecipientWindow editWindow = new EditAlertRecipientWindow(selectedRecipient, this);
            editWindow.ShowDialog();
            
            if (editWindow.IsSaved)
            {
                LoadAlertRecipients(); 
            }

        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            
            AlertRecipient selectedRecipient;
            try
            {
                selectedRecipient = (AlertRecipient)alertRecipientsGrid.SelectedItem;
            }
            catch
            {

                MessageBox.Show("Select a recipient to delete", "Selecitionerror", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var result = MessageBox.Show($"Are you sure you want to delete recipient {selectedRecipient.Recipient}?",
                                         "Delete Confirmation",
                                         MessageBoxButton.YesNo,
                                         MessageBoxImage.Warning);

                if (result == MessageBoxResult.Yes)
                {
                    
                    using (var context = new DataContext())
                    {
                        var recipientInDb = context.AlertRecipients.Find(selectedRecipient.Id);
                        if (recipientInDb != null)
                        {
                            context.AlertRecipients.Remove(recipientInDb);
                            context.SaveChanges();
                        }
                    }

                    
                    LoadAlertRecipients();
                    MessageBox.Show("Recipient deleted successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                }

            
           
            
            
        }


    }
}

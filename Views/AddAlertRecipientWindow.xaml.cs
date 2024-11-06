using System;
using System.Windows;
using WpfRequestResponseLogger.Data;
using WpfRequestResponseLogger.Models; 

namespace WpfRequestResponseLogger.Views
{
    public partial class AddAlertRecipientWindow : Window
    {
        public bool IsSaved { get; private set; } = false;
        private ManageAlertRecipientsWindow _parentWindow;

        public AddAlertRecipientWindow(ManageAlertRecipientsWindow parentWindow)
        {
            InitializeComponent();
            _parentWindow = parentWindow; 
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
                       

            if (string.IsNullOrWhiteSpace(txtRecipient.Text))
            {
                MessageBox.Show("Recipient cannot be empty.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            
            using (var context = new DataContext())
            {
                var newRecipient = new AlertRecipient
                {
                    
                    Recipient = txtRecipient.Text,
                    Email = txtEmail.Text,
                    Phone = txtPhone.Text
                };

                context.AlertRecipients.Add(newRecipient);
                context.SaveChanges();
            }

            
            IsSaved = true;
            this.Close(); 
        }

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            this.Close(); 
        }
    }
}

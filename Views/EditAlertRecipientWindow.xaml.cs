using System;
using System.Windows;
using WpfRequestResponseLogger.Data;
using WpfRequestResponseLogger.Models; 

namespace WpfRequestResponseLogger.Views
{
    public partial class EditAlertRecipientWindow : Window
    {
        private AlertRecipient _recipientToEdit;
        public bool IsSaved { get; private set; } = false;

        public EditAlertRecipientWindow(AlertRecipient recipientToEdit, ManageAlertRecipientsWindow parentWindow)
        {
            InitializeComponent();
            _recipientToEdit = recipientToEdit;
            
            LoadRecipientData();
        }

        private void LoadRecipientData()
        {
            
            txtId.Text = _recipientToEdit.Id.ToString();
            txtRecipient.Text = _recipientToEdit.Recipient;
            txtEmail.Text = _recipientToEdit.Email;
            txtPhone.Text = _recipientToEdit.Phone;
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
                var recipientInDb = context.AlertRecipients.Find(_recipientToEdit.Id);

                if (recipientInDb != null)
                {
                    
                    recipientInDb.Recipient = txtRecipient.Text;
                    recipientInDb.Email = txtEmail.Text;
                    recipientInDb.Phone = txtPhone.Text;
                    context.SaveChanges();
                }
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

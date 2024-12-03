using System;
using System.Windows;
using WpfRequestResponseLogger.Models;
using WpfRequestResponseLogger.Data;
using System.Windows.Controls;

namespace WpfRequestResponseLogger.Views
{
    public partial class AddServerWindow : Window
    {
        public bool IsSaved { get; private set; } = false;

        public AddServerWindow()
        {
            InitializeComponent();
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(ServerNameTextBox.Text))
            {
                MessageBox.Show("Please enter a valid server name.");
                return;
            }

            if (string.IsNullOrWhiteSpace(ConnectionStringTextBox.Text))
            {
                MessageBox.Show("Please enter a valid connection string.");
                return;
            }

            // Validate ComboBox selection
            var selectedServerType = (ServerTypeComboBox.SelectedItem as ComboBoxItem)?.Content?.ToString();
            if (string.IsNullOrWhiteSpace(selectedServerType))
            {
                MessageBox.Show("Please select a valid server type.");
                return;
            }

            try
            {
                using (var context = new DataContext())
                {
                    var newServer = new Server
                    {
                        ServerName = ServerNameTextBox.Text,
                        ConnectionString = ConnectionStringTextBox.Text,
                        ServerType = selectedServerType // Store selected server type
                    };

                    context.Servers.Add(newServer);
                    context.SaveChanges();
                }

                IsSaved = true;
                MessageBox.Show("Server added successfully.");
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error adding server: {ex.Message}");
            }
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}

using System;
using System.Windows;
using WpfRequestResponseLogger.Models;
using WpfRequestResponseLogger.Data;

namespace WpfRequestResponseLogger.Views
{
    public partial class EditServerWindow : Window
    {
        private Server _server;

        public bool IsSaved { get; private set; } = false;

        public EditServerWindow(Server server)
        {
            InitializeComponent();
            _server = server;
            LoadServerData();
        }

        private void LoadServerData()
        {
            
            ServerIdTextBox.Text = _server.Id.ToString();
            ServerNameTextBox.Text = _server.ServerName;
            ConnectionStringTextBox.Text = _server.ConnectionString;
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

            
            try
            {
                using (var context = new DataContext())
                {
                    
                    var serverToUpdate = context.Servers.Find(_server.Id);
                    if (serverToUpdate != null)
                    {
                        serverToUpdate.ServerName = ServerNameTextBox.Text;
                        serverToUpdate.ConnectionString = ConnectionStringTextBox.Text;

                        context.SaveChanges();
                        IsSaved = true;
                        MessageBox.Show("Server updated successfully.");
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("Error: Server not found in the database.");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error updating server: {ex.Message}");
            }
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close(); 
        }
    }
}

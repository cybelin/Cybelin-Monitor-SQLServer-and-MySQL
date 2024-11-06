
using System.Collections.Generic;
using System.Linq;

using System.Windows;

using WpfRequestResponseLogger.Data;
using WpfRequestResponseLogger.Models;   

namespace WpfRequestResponseLogger.Views
{
    public partial class ManageServersWindow : Window
    {
        private List<Server> _servers;

        public ManageServersWindow()
        {
            InitializeComponent();
            LoadServers();
        }

        private void LoadServers()
        {
            
            using (var context = new DataContext()) 
            {
                _servers = context.Servers.ToList();
                ServersDataGrid.ItemsSource = _servers;
            }
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            
            var addWindow = new AddServerWindow();
            addWindow.ShowDialog();

            
            if (addWindow.IsSaved)
            {
                LoadServers(); 
            }
        }


        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            if (ServersDataGrid.SelectedItem is Server selectedServer)
            {
                
                var editWindow = new EditServerWindow(selectedServer);
                editWindow.ShowDialog();

                
                if (editWindow.IsSaved)
                {
                    LoadServers(); 
                }
            }
            else
            {
                MessageBox.Show("Please select a server to edit.");
            }
        }


        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (ServersDataGrid.SelectedItem is Server selectedServer)
            {
                if (MessageBox.Show($"Are you sure you want to delete {selectedServer.ServerName}?", "Confirm Delete", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    
                    using (var context = new DataContext()) 
                    {
                        context.Servers.Remove(selectedServer);
                        context.SaveChanges();
                    }
                    LoadServers(); 
                }
            }
            else
            {
                MessageBox.Show("Please select a server to delete.");
            }
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close(); 
        }
    }
}





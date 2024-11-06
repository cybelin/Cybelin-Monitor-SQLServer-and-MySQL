using System;
using System.Linq;
using System.Windows;
using WpfRequestResponseLogger.Data; 

using WpfRequestResponseLogger.Models; 


namespace WpfRequestResponseLogger.Views
{
    public partial class ManageAlertSilencedWindow : Window
    {
        private readonly DataContext _dbContext;  // Your DbContext for database operations

        public ManageAlertSilencedWindow()
        {
            InitializeComponent();
            _dbContext = new DataContext();  // Initialize DbContext
            LoadAlertSilencedData();  // Load data from the database
        }

        private void LoadAlertSilencedData()
        {
            // Load AlertSilenced data from the database
            var alertSilencedData = _dbContext.AlertSilenced.ToList();
            AlertSilencedGrid.ItemsSource = alertSilencedData;
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            // Create an instance of the AddAlertSilencedWindow
            AddAlertSilencedWindow addAlertSilencedWindow = new AddAlertSilencedWindow();

            // Show the window as a modal dialog
            addAlertSilencedWindow.ShowDialog();

            // After the window is closed, reload the data in the DataGrid to reflect any new entries
            LoadAlertSilencedData();
        }


        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            // Get the selected AlertSilenced record from the DataGrid
            var selectedAlertSilenced = (AlertSilenced)AlertSilencedGrid.SelectedItem;

            if (selectedAlertSilenced == null)
            {
                MessageBox.Show("Please select an alert to edit.");
                return;
            }

            // Create an instance of the EditAlertSilencedWindow and pass the selected record
            EditAlertSilencedWindow editAlertSilencedWindow = new EditAlertSilencedWindow(selectedAlertSilenced);
            editAlertSilencedWindow.ShowDialog();  // Show it as a modal window

            // Reload the data in the DataGrid after editing
            LoadAlertSilencedData();
        }


        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            // Get the selected AlertSilenced record from the DataGrid
            var selectedAlertSilenced = (AlertSilenced)AlertSilencedGrid.SelectedItem;

            if (selectedAlertSilenced == null)
            {
                MessageBox.Show("Please select an alert to delete.");
                return;
            }

            // Confirm deletion
            var result = MessageBox.Show($"Are you sure you want to delete the silenced alert: {selectedAlertSilenced.Id}?",
                                         "Confirm Delete", MessageBoxButton.YesNo, MessageBoxImage.Warning);

            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    // Remove the selected alert from the database
                    _dbContext.AlertSilenced.Remove(selectedAlertSilenced);
                    _dbContext.SaveChanges();
                    LoadAlertSilencedData();  // Reload data after deletion
                    MessageBox.Show("Alert silenced entry deleted successfully.");
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error deleting alert silenced entry: {ex.Message}");
                }
            }
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            // Close the window
            this.Close();
        }
    }
}

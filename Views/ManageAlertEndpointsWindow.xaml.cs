using System.Linq;
using System.Windows;
using WpfRequestResponseLogger.Data;
using WpfRequestResponseLogger.Models;
using WpfRequestResponseLogger;



namespace WpfRequestResponseLogger
{
    public partial class ManageAlertEndpointsWindow : Window
    {
        private readonly DataContext _context;

        // Constructor
        public ManageAlertEndpointsWindow()
        {
            InitializeComponent();
            _context = new DataContext(); // Initialize your DbContext here
            LoadAlertEndpoints();
        }

        // Method to load data from the database into the DataGrid
        private void LoadAlertEndpoints()
        {
            var alertEndpoints = _context.AlertEndpoints.ToList();
            alertEndpointsDataGrid.ItemsSource = alertEndpoints;
        }

        // Event handler for the 'Add' button click
        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            // Open window for adding a new alert endpoint
            
            AddAlertEndpointWindow addWindow = new AddAlertEndpointWindow();
            addWindow.ShowDialog();
            // Reload data after adding a new record
            LoadAlertEndpoints();
            
        }

        // Event handler for the 'Edit' button click
        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            // Get the selected alert endpoint
            
            var selectedEndpoint = (AlertEndpoint)alertEndpointsDataGrid.SelectedItem;
            if (selectedEndpoint != null)
            {
                EditAlertEndpointWindow editWindow = new EditAlertEndpointWindow(selectedEndpoint);
                editWindow.ShowDialog();
                // Reload data after editing the record
                LoadAlertEndpoints();
            }
            else
            {
                MessageBox.Show("Please select an endpoint to edit.", "Edit Alert Endpoint", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            
        }

        // Event handler for the 'Delete' button click
        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            var selectedEndpoint = (AlertEndpoint)alertEndpointsDataGrid.SelectedItem;
            if (selectedEndpoint != null)
            {
                var result = MessageBox.Show($"Are you sure you want to delete the alert endpoint '{selectedEndpoint.Path}'?",
                                             "Delete Alert Endpoint", MessageBoxButton.YesNo, MessageBoxImage.Warning);

                if (result == MessageBoxResult.Yes)
                {
                    _context.AlertEndpoints.Remove(selectedEndpoint);
                    _context.SaveChanges();
                    LoadAlertEndpoints(); // Reload the data after deletion
                }
            }
            else
            {
                MessageBox.Show("Please select an endpoint to delete.", "Delete Alert Endpoint", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        // Event handler for the 'Exit' button click
        private void BtnExit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}

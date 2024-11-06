using System.Windows;
using WpfRequestResponseLogger.Data;
using WpfRequestResponseLogger.Models;

namespace WpfRequestResponseLogger
{
    public partial class AddAlertEndpointWindow : Window
    {
        private readonly DataContext _context;

        // Constructor
        public AddAlertEndpointWindow()
        {
            InitializeComponent();
            _context = new DataContext(); // Initialize your DbContext here
        }

        // Event handler for the 'Save' button click
        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            // Validate the Path field
            if (string.IsNullOrWhiteSpace(txtPath.Text))
            {
                MessageBox.Show("Path cannot be empty.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Create a new AlertEndpoint object
            var newEndpoint = new AlertEndpoint
            {
                Path = txtPath.Text
            };

            // Add the new endpoint to the database
            _context.AlertEndpoints.Add(newEndpoint);
            _context.SaveChanges();

            // Close the window after saving
            MessageBox.Show("Alert Endpoint added successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            this.Close();
        }

        // Event handler for the 'Exit' button click
        private void BtnExit_Click(object sender, RoutedEventArgs e)
        {
            // Close the window without saving
            this.Close();
        }
    }
}

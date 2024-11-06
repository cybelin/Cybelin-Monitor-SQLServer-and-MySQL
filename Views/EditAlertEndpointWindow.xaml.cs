using System.Windows;
using WpfRequestResponseLogger.Data;
using WpfRequestResponseLogger.Models;

namespace WpfRequestResponseLogger
{
    public partial class EditAlertEndpointWindow : Window
    {
        private readonly DataContext _context;
        private readonly AlertEndpoint _alertEndpoint;

        // Constructor receives the selected AlertEndpoint object
        public EditAlertEndpointWindow(AlertEndpoint alertEndpoint)
        {
            InitializeComponent();
            _context = new DataContext(); // Initialize your DbContext here
            _alertEndpoint = alertEndpoint;
            LoadAlertEndpointData();
        }

        // Method to load data into TextBoxes
        private void LoadAlertEndpointData()
        {
            // Populate the fields with the existing data
            txtId.Text = _alertEndpoint.Id.ToString();
            txtPath.Text = _alertEndpoint.Path;
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

            // Update the existing AlertEndpoint object
            _alertEndpoint.Path = txtPath.Text;

            // Save changes to the database
            _context.AlertEndpoints.Update(_alertEndpoint);
            _context.SaveChanges();

            // Close the window after saving
            MessageBox.Show("Alert Endpoint updated successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
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

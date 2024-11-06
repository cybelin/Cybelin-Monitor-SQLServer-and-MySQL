using System.Linq;
using System.Windows;
using WpfRequestResponseLogger.Data;
using WpfRequestResponseLogger.Models;

namespace WpfRequestResponseLogger
{
    public partial class AddAlertRuleEndpointsWindow : Window
    {
        private readonly DataContext _context;

        // Constructor
        public AddAlertRuleEndpointsWindow()
        {
            InitializeComponent();
            _context = new DataContext();  // Initialize your DbContext
            LoadComboBoxData();
        }

        // Load AlertRules and AlertEndpoints into ComboBoxes
        private void LoadComboBoxData()
        {
            // Load AlertRules (Id and Name) into cbAlertRules
            cbAlertRules.ItemsSource = _context.AlertRules.Select(ar => new { ar.Id, ar.Name }).ToList();

            // Load AlertEndpoints (Id and Path) into cbAlertEndpoints
            cbAlertEndpoints.ItemsSource = _context.AlertEndpoints.Select(ae => new { ae.Id, ae.Path }).ToList();
        }

        // Event handler for the 'Save' button click
        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            // Validate that both ComboBoxes have a selected value
            if (cbAlertRules.SelectedItem == null || cbAlertEndpoints.SelectedItem == null)
            {
                MessageBox.Show("Please select both an Alert Rule and an Alert Endpoint.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Create new AlertRuleEndpoint object
            var newAlertRuleEndpoint = new AlertRuleEndpoint
            {
                AlertRuleId = (int)cbAlertRules.SelectedValue,
                AlertEndpointId = (int)cbAlertEndpoints.SelectedValue
            };

            // Add the new record to the database
            _context.AlertRulesEndpoints.Add(newAlertRuleEndpoint);
            _context.SaveChanges();

            // Close the window after saving
            MessageBox.Show("Alert Rule Endpoint added successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
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

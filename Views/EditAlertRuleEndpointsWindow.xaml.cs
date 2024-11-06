using System.Linq;
using System.Windows;
using WpfRequestResponseLogger.Data;
using WpfRequestResponseLogger.Models;

namespace WpfRequestResponseLogger
{
    public partial class EditAlertRuleEndpointsWindow : Window
    {
        private readonly DataContext _context;
        private readonly AlertRuleEndpoint _alertRuleEndpoint;

        // Constructor that takes the selected AlertRuleEndpoint
        public EditAlertRuleEndpointsWindow(AlertRuleEndpoint alertRuleEndpoint)
        {
            InitializeComponent();
            _context = new DataContext();  // Initialize your DbContext
            _alertRuleEndpoint = alertRuleEndpoint;
            LoadComboBoxData();
            LoadAlertRuleEndpointData();
        }

        // Load AlertRules and AlertEndpoints into ComboBoxes
        private void LoadComboBoxData()
        {
            // Load AlertRules (Id and Name) into cbAlertRules
            cbAlertRules.ItemsSource = _context.AlertRules.Select(ar => new { ar.Id, ar.Name }).ToList();

            // Load AlertEndpoints (Id and Path) into cbAlertEndpoints
            cbAlertEndpoints.ItemsSource = _context.AlertEndpoints.Select(ae => new { ae.Id, ae.Path }).ToList();
        }

        // Load existing data into controls
        private void LoadAlertRuleEndpointData()
        {
            // Populate fields with existing data
            txtId.Text = _alertRuleEndpoint.Id.ToString();
            cbAlertRules.SelectedValue = _alertRuleEndpoint.AlertRuleId;
            cbAlertEndpoints.SelectedValue = _alertRuleEndpoint.AlertEndpointId;
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

            // Update the existing AlertRuleEndpoint object
            _alertRuleEndpoint.AlertRuleId = (int)cbAlertRules.SelectedValue;
            _alertRuleEndpoint.AlertEndpointId = (int)cbAlertEndpoints.SelectedValue;

            // Save changes to the database
            _context.AlertRulesEndpoints.Update(_alertRuleEndpoint);
            _context.SaveChanges();

            // Close the window after saving
            MessageBox.Show("Alert Rule Endpoint updated successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
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

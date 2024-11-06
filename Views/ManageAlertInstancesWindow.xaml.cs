using System.Linq;
using System.Windows;
using WpfRequestResponseLogger.Data; 
using WpfRequestResponseLogger.Models; 

namespace WpfRequestResponseLogger.Views
{
    public partial class ManageAlertInstancesWindow : Window
    {
        private readonly DataContext _context;

        public ManageAlertInstancesWindow()
        {
            InitializeComponent();
            _context = new DataContext();
            LoadAlertInstances();
        }

        // Loads alert instances from the database, ordered by TriggeredAtUTC descending
        private void LoadAlertInstances()
        {
            var alertInstances = _context.AlertInstances
                .OrderByDescending(ai => ai.TriggeredAtUTC)
                .ToList();

            AlertInstancesGrid.ItemsSource = alertInstances;
        }

        // Event handler for the Add button click
        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            // Implement functionality to open an "Add Alert Instance" window
            MessageBox.Show("Add functionality not implemented yet.");
        }

        // Event handler for the Edit button click
        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            // Check if an alert instance is selected and open an "Edit" window
            if (AlertInstancesGrid.SelectedItem is AlertInstance selectedInstance)
            {
                // Open Edit window, passing the selected alert instance
                var editWindow = new EditAlertInstanceWindow((AlertInstance)AlertInstancesGrid.SelectedItem);
                editWindow.ShowDialog();
                LoadAlertInstances();
            }
            else
            {
                MessageBox.Show("Please select an alert instance to edit.", "No Selection", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        // Event handler for the Delete button click
        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (AlertInstancesGrid.SelectedItem is AlertInstance selectedInstance)
            {
                var result = MessageBox.Show($"Are you sure you want to delete the alert instance with ID {selectedInstance.Id}?",
                                             "Delete Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Warning);

                if (result == MessageBoxResult.Yes)
                {
                    _context.AlertInstances.Remove(selectedInstance);
                    _context.SaveChanges();
                    LoadAlertInstances(); // Reload the grid after deletion
                }
            }
            else
            {
                MessageBox.Show("Please select an alert instance to delete.", "No Selection", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        // Event handler for the Exit button click
        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close(); // Close the window
        }
    }
}

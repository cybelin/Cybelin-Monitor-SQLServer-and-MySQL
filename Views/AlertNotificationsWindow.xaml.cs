using System.Linq;
using System.Windows;
using WpfRequestResponseLogger.Data; 
using WpfRequestResponseLogger.Models; 

namespace WpfRequestResponseLogger.Views
{
    public partial class AlertNotificationsWindow : Window
    {
        private readonly DataContext _context;

        public AlertNotificationsWindow()
        {
            InitializeComponent();
            _context = new DataContext();
            LoadAlertNotifications();
        }

        private void LoadAlertNotifications()
        {
            var alertNotifications = _context.AlertNotifications
                .OrderByDescending(n => n.Id)
                .ToList();

            AlertNotificationsGrid.ItemsSource = alertNotifications;
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (AlertNotificationsGrid.SelectedItem is AlertNotification selectedNotification)
            {
                var result = MessageBox.Show($"Are you sure you want to delete notification with ID {selectedNotification.Id}?",
                                             "Delete Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Warning);

                if (result == MessageBoxResult.Yes)
                {
                    _context.AlertNotifications.Remove(selectedNotification);
                    _context.SaveChanges();
                    LoadAlertNotifications(); 
                }
            }
            else
            {
                MessageBox.Show("Please select a notification to delete.", "No Selection", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}

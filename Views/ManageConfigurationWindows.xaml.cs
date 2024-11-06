using System;
using System.Linq;
using System.Windows;
using WpfRequestResponseLogger.Data;

namespace WpfRequestResponseLogger.Views
{
    public partial class ConfigurationManagerWindow : Window
    {
        public ConfigurationManagerWindow()
        {
            InitializeComponent();
            LoadConfigurationsData(); 
        }

        
        private void LoadConfigurationsData()
        {
            using (var context = new DataContext())
            {
                var configurations = context.MonitorConfigurations.ToList();
                ConfigurationDataGrid.ItemsSource = configurations;
            }
        }



        
        private void OnEditButtonClick(object sender, RoutedEventArgs e)
        {
            
            if (ConfigurationDataGrid.SelectedItem is Models.Configuration selectedConfig)
            {
                
                var editWindow = new EditConfigurationWindow(selectedConfig);

                
                editWindow.ShowDialog();

                
                LoadConfigurationsData();
            }
            else
            {
                MessageBox.Show("Please select a configuration to edit.");
            }
        }


        
        private void OnDeleteButtonClick(object sender, RoutedEventArgs e)
        {
            if (ConfigurationDataGrid.SelectedItem is Models.Configuration selectedConfig)
            {
                MessageBoxResult result = MessageBox.Show($"Are you sure you want to delete the configuration: {selectedConfig.Key}?",
                                                          "Confirm Delete", MessageBoxButton.YesNo, MessageBoxImage.Warning);

                if (result == MessageBoxResult.Yes)
                {
                    DeleteConfigurationFromDatabase(selectedConfig);
                    LoadConfigurationsData(); 
                    MessageBox.Show("Configuration deleted successfully.");
                }
            }
            else
            {
                MessageBox.Show("Please select a configuration to delete.");
            }
        }

        
        private void DeleteConfigurationFromDatabase(Models.Configuration configToDelete)
        {
            using (var context = new DataContext())
            {
                var config = context.MonitorConfigurations.FirstOrDefault(c => c.Id == configToDelete.Id);
                if (config != null)
                {
                    context.MonitorConfigurations.Remove(config);
                    context.SaveChanges();
                }
            }
        }

        
        private void OnExitButtonClick(object sender, RoutedEventArgs e)
        {
            this.Close(); 
        }

        private void OnAddButtonClick(object sender, RoutedEventArgs e)
        {
            
            var addWindow = new AddConfigurationWindow();

            
            addWindow.OnConfigurationAdded += () =>
            {
                LoadConfigurationsData(); 
            };

            addWindow.ShowDialog(); 
        }

    }
}

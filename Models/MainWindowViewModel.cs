using System.Collections.ObjectModel;
using WpfRequestResponseLogger.Models;

namespace WpfRequestResponseLogger.ViewModels
{
    public class MainWindowViewModel
    {
        public ObservableCollection<RequestLog> RequestLogs { get; set; }
        public ObservableCollection<ResponseLog> ResponseLogs { get; set; }

        public MainWindowViewModel()
        {
            
        }
    }
}

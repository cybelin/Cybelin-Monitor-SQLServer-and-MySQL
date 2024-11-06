using System.Collections.ObjectModel;
using System.Linq;
using WpfRequestResponseLogger.Data;
using WpfRequestResponseLogger.Models;

namespace WpfRequestResponseLogger.ViewModels
{
    public class RequestsViewModel
    {
        public ObservableCollection<RequestLog> RequestLogs { get; set; }

        public RequestsViewModel()
        {
            using (var context = new DataContext())
            {
                var logs = context.RequestLogs.ToList();
                RequestLogs = new ObservableCollection<RequestLog>(logs);
            }
        }
    }
}

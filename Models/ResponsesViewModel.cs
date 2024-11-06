using System.Collections.ObjectModel;
using System.Linq;
using WpfRequestResponseLogger.Data;
using WpfRequestResponseLogger.Models;

namespace WpfRequestResponseLogger.ViewModels
{
    public class ResponsesViewModel
    {
        public ObservableCollection<ResponseLog> ResponseLogs { get; set; }

        public ResponsesViewModel()
        {
            using (var context = new DataContext())
            {
                var logs = context.ResponseLogs.ToList();
                ResponseLogs = new ObservableCollection<ResponseLog>(logs);
            }
        }
    }
}

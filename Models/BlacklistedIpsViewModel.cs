using System.Collections.ObjectModel;
using System.Linq;
using WpfRequestResponseLogger.Data;
using WpfRequestResponseLogger.Models;

namespace WpfRequestResponseLogger.ViewModels
{
    public class BlacklistedIpsViewModel
    {
        public ObservableCollection<BlacklistedIp> BlacklistedIps { get; set; }

        public BlacklistedIpsViewModel()
        {
            using (var context = new DataContext())
            {
                var ips = context.MonitorBlacklistedIps.ToList();
                BlacklistedIps = new ObservableCollection<BlacklistedIp>(ips);
            }
        }
    }
}

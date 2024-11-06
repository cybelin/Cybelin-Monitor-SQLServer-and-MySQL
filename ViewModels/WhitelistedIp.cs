using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfRequestResponseLogger.Models
{
    public class WhitelistedIp
    {
        public int Id { get; set; }
        public string IpAddress { get; set; }
        public DateTime DateAdded { get; set; }
        public string Reason { get; set; }
        public bool IsActive { get; set; }
    }
}

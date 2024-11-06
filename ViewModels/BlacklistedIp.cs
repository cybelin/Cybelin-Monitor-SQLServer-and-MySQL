using System;

namespace WpfRequestResponseLogger.Models
{
    
    public class BlacklistedIp
    {
        public int Id { get; set; }
        public string IpAddress { get; set; }
        public DateTime DateAdded { get; set; }
        public string Reason { get; set; }
        public bool IsActive { get; set; }
    }
    
}

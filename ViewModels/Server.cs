using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfRequestResponseLogger.Models
{
    public class Server
    {
        public int Id { get; set; }
        public string ServerName { get; set; }
        public string ConnectionString { get; set; }
        public string ServerType { get; set; }
    }
}

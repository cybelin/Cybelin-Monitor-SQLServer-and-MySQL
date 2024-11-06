using System;

namespace WpfRequestResponseLogger.Models
{
    public class ResponseLog
    {
        public long ResponseLogId { get; set; }
        public Guid RequestId { get; set; }
        public int StatusCode { get; set; }
        public string ResponseHeaders { get; set; }
        public DateTime ResponseTime { get; set; }
        public long DurationMs { get; set; }
        public string ServerIp { get; set; }
        public long ResponseSizeInBytes { get; set; }
    }
}

using System;

namespace WpfRequestResponseLogger.Models
{
    public class RequestLog
    {
        public long RequestLogId { get; set; }
        public Guid RequestId { get; set; }
        public string HttpMethod { get; set; }
        public string RequestPath { get; set; }
        public string QueryString { get; set; }
        public string RequestHeaders { get; set; }
        public string ClientIp { get; set; }
        public string UserAgent { get; set; }
        public DateTime RequestTime { get; set; }
        public string HttpVersion { get; set; }
        public string RequestBody { get; set; }
    }
}

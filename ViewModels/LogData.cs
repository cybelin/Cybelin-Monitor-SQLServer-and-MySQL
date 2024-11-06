using System;

namespace WpfRequestResponseLogger.Models
{
    
    public class LogData
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
        public long? ResponseLogId { get; set; }
        public int? StatusCode { get; set; }
        public string ResponseHeaders { get; set; }
        public DateTime? ResponseTime { get; set; }
        public long? DurationMs { get; set; }
        public string ServerIp { get; set; }
        public long? ResponseSizeInBytes { get; set; }
    }




}

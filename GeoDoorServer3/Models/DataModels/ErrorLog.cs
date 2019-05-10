using System;
using Microsoft.Extensions.Logging;

namespace GeoDoorServer3.Models.DataModels
{
    public class ErrorLog
    {
        public int Id { get; set; }
        public LogLevel LogLevel { get; set; } 
        public DateTime MsgDateTime { get; set; }
        public string Message { get; set; }
    }
}

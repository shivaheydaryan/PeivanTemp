using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Models
{
    public class Token
    {
        public Int64 id { get; set; }
        public string TokenCode { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public DateTime? Date { get; set; }
        public string IP { get; set; }
        public bool? Valid { get; set; }
        public string Platform { get; set; }
        public string PlatformVersion { get; set; }
        public string AppVersion { get; set; }
        public string DeviceNumber { get; set; }
        public string BuildNumber { get; set; }

    }
}
using System;
using System.Collections.Generic;
using System.Text;

namespace SilupostMobileApp.Models
{
    public class PhoneCallLogsModel
    {
        public string ContactNumber { get; set; }
        public string CallType { get; set; }
        public DateTime DateAndTime { get; set; }
        public int CallDuration { get; set; }
    }
}

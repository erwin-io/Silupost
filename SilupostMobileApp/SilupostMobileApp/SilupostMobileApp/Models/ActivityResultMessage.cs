using System;
using System.Collections.Generic;
using System.Text;

namespace SilupostMobileApp.Models
{
    public class ActivityResultMessage
    {
        public static string Key = "arm";

        public int RequestCode { get; set; }

        public object ResultCode { get; set; }

        public object Data { get; set; }
    }
}

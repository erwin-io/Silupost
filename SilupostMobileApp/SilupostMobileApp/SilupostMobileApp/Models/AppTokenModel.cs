using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace SilupostMobileApp.Models
{
    public class AppTokenModel
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
    }
}

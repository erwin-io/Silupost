using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace POSWeb.POS.API.Models
{
    public class AppResponseModel<T>
    {
        public bool IsSuccess { get; set; } = false;
        public string Message { get; set; }
        public string DeveloperMessage { get; set; }
        public T Data { get; set; }
        public bool IsWarning { get; set; } = false;

    }
}
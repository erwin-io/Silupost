using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SilupostWeb.API.Helpers
{
    public static class GlobalFunctions
    {
        public static bool IsValidTimeFormat(this string input)
        {
            TimeSpan dummyOutput;
            return TimeSpan.TryParse(input, out dummyOutput);
        }
    }
}
using System;
using System.Collections.Generic;
using System.Text;
using SilupostMobileApp.Common;

namespace SilupostMobileApp.Models
{
    public class MapConfigModel
    {
        public float Latitude { get; set; }
        public float Longitude { get; set; }
        public decimal Radius { get; set; }
        public SilupostMapLookupTypeEnums LookupType { get; set; }
    }
}

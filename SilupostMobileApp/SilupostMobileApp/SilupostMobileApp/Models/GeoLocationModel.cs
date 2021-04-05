using System;
using System.Collections.Generic;
using System.Text;

namespace SilupostMobileApp.Models
{
    public class GeoLocationModel
    {
        public string GeoAddress { get; set; }
        public string GeoStreet { get; set; }
        public string GeoDistrict { get; set; }
        public string GeoCityMun { get; set; }
        public string GeoProvince { get; set; }
        public string GeoCountry { get; set; }
        public float GeoLatitude { get; set; }
        public float GeoLongitude { get; set; }
        public decimal Radius { get; set; }
    }
}

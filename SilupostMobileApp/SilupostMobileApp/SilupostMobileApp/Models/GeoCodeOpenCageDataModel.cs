using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace SilupostMobileApp.Models
{
    public class GeoCodeOpenCageDataModel
    {
        [JsonProperty("results")]
        public List<Result> Results { get; set; }
    }
    public class Components
    {
        [JsonProperty("continent")]
        public string Continent { get; set; }

        [JsonProperty("country")]
        public string Country { get; set; }

        [JsonProperty("country_code")]
        public string CountryCode { get; set; }

        [JsonProperty("neighbourhood")]
        public string Neighbourhood { get; set; }

        [JsonProperty("postcode")]
        public string Postcode { get; set; }

        [JsonProperty("region")]
        public string Region { get; set; }

        [JsonProperty("road")]
        public string Road { get; set; }

        [JsonProperty("school")]
        public string School { get; set; }

        [JsonProperty("state")]
        public string State { get; set; }

        [JsonProperty("state_code")]
        public string StateCode { get; set; }

        [JsonProperty("suburb")]
        public string Suburb { get; set; }

        [JsonProperty("city")]
        public string City { get; set; }

        [JsonProperty("town")]
        public string Town { get; set; }

        [JsonProperty("county")]
        public string County { get; set; }

        [JsonProperty("village")]
        public string Village { get; set; }
        
    }

    public class Result
    {
        [JsonProperty("components")]
        public Components Components { get; set; }

        [JsonProperty("formatted")]
        public string Formatted { get; set; }
    }

}

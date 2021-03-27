using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using SilupostMobileApp.BindingModels;
using SilupostMobileApp.Models;

namespace SilupostMobileApp.Services.Interface
{
    public interface IGeoCodeOpenCageDataService
    {
        Task<GeoCodeOpenCageDataModel> GetGeoAddressAsync(string Latitude, string Longitude);
    }
}

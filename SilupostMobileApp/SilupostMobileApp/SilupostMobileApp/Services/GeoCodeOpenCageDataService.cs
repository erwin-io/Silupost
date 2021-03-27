using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SilupostMobileApp.BindingModels;
using SilupostMobileApp.Common;
using SilupostMobileApp.Models;
using SilupostMobileApp.Services.Interface;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace SilupostMobileApp.Services
{
    public class GeoCodeOpenCageDataService : IGeoCodeOpenCageDataService
    {
        public async Task<GeoCodeOpenCageDataModel> GetGeoAddressAsync(string Latitude, string Longitude)
        {
            try
            {
                #region
                using (var client = new HttpClient())
                {
                    string baseURI = "https://api.opencagedata.com/";
                    string token = "75135653b8d042a0882878fcb4235ed8";
                    client.BaseAddress = new Uri(baseURI);
                    string _paramString = AppSettingsHelper.goSILUPOST_WEBAPI_Authentication;

                    client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _paramString);
                    HttpResponseMessage _responseMessage = new HttpResponseMessage();
                    _responseMessage = await client.GetAsync(string.Format("{0}geocode/v1/json?key={1}&q={2},{3}&pretty=1&no_annotations=1", baseURI, token, Latitude, Longitude));
                    string _json = await _responseMessage.Content.ReadAsStringAsync();

                    var response = new GeoCodeOpenCageDataModel();
                    JObject obj = JsonConvert.DeserializeObject<JObject>(_json);
                    response = obj.ToObject<GeoCodeOpenCageDataModel>();
                    if (response != null && response.Results.Count > 0)
                    {
                        return response;
                    }
                    else
                    {
                        throw new Exception(SilupostMessage.SERVER_ERROR);
                    }
                }
                #endregion
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}

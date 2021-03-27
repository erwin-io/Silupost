using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SilupostMobileApp.BindingModels;
using SilupostMobileApp.Common;
using SilupostMobileApp.Models;
using SilupostMobileApp.Services.Interface;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace SilupostMobileApp.Services
{
    public class CrimeIncidentCategoryService : ICrimeIncidentCategoryService
    {
        public async Task<IEnumerable<CrimeIncidentCategoryModel>> GetAllAsync()
        {
            try
            {
                #region
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(AppSettingsHelper.goSILUPOST_WEBAPI_URI);
                    string _paramString = AppSettingsHelper.AppSettings.AppToken.AccessToken;

                    client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _paramString);
                    HttpResponseMessage _responseMessage = new HttpResponseMessage();
                    _responseMessage = await client.GetAsync(string.Format("{0}CrimeIncidentCategory", AppSettingsHelper.goSILUPOST_WEBAPI_URI));
                    string _json = await _responseMessage.Content.ReadAsStringAsync();

                    var response = new SillupostWebAPIResponseModel<List<CrimeIncidentCategoryModel>>();
                    JObject obj = JsonConvert.DeserializeObject<JObject>(_json);
                    response = obj.ToObject<SillupostWebAPIResponseModel<List<CrimeIncidentCategoryModel>>>();
                    if (response.IsSuccess)
                    {
                        return response.Data;
                    }
                    else
                    {
                        throw new Exception(response.Message);
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

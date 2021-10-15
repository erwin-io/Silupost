using ModernHttpClient;
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
    public class AppConnectService : IAppConnectService
    {
        public async Task<AppConnectConfigModel> Get(string jsonUrl)
        {
            try
            {
                #region
                using (HttpClient client = new HttpClient())
                {
                    client.BaseAddress = new Uri(jsonUrl);

                    HttpResponseMessage _responseMessage = await client.GetAsync(jsonUrl);
                    string _json = await _responseMessage.Content.ReadAsStringAsync();

                    AppConnectConfigModel response = new AppConnectConfigModel();
                    JObject obj = JsonConvert.DeserializeObject<JObject>(_json);
                    response = obj.ToObject<AppConnectConfigModel>();
                    if (response != null && !string.IsNullOrEmpty(response.SILUPOST_API_URL))
                    {
                        return response;
                    }
                    else
                    {
                        throw new SilupostServiceException(SilupostMessage.SERVER_ERROR);
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

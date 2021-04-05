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
    public class SystemLookupService : ISystemLookupService
    {

        public async Task<List<LookupTableModel>> GetLookup(string tableNames)
        {
            var result = new List<LookupTableModel>();
            try
            {
                #region
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(AppSettingsHelper.goSILUPOST_WEBAPI_URI);
                    HttpResponseMessage _responseMessage = new HttpResponseMessage();
                    _responseMessage = await client.GetAsync(string.Format("{0}SystemLookup/GetAllByTableNames?TableNames={1}", AppSettingsHelper.goSILUPOST_WEBAPI_URI, tableNames));
                    string _json = await _responseMessage.Content.ReadAsStringAsync();

                    var response = new SillupostWebAPIResponseModel<Dictionary<string, List<LookupModel>>>();
                    JObject obj = JsonConvert.DeserializeObject<JObject>(_json);
                    response = obj.ToObject<SillupostWebAPIResponseModel<Dictionary<string, List<LookupModel>>>>();
                    if (response.IsSuccess)
                    {
                        if(response.Data != null)
                        {
                            foreach (var lookup in response.Data)
                            {
                                result.Add(new LookupTableModel() { LookupName = lookup.Key, LookupData = lookup.Value });
                            }
                        }
                        return result;
                    }
                    else
                    {
                        throw new SilupostServiceException(response.Message);
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

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
    public class FileService : IFileService
    {
        public async Task<FileModel> Get(string fileId)
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
                    _responseMessage = await client.GetAsync(string.Format("{0}File/getFile?FileId={1}", AppSettingsHelper.goSILUPOST_WEBAPI_URI, fileId));
                    string _json = await _responseMessage.Content.ReadAsStringAsync();

                    var response = new SillupostWebAPIResponseModel<FileModel>();
                    JObject obj = JsonConvert.DeserializeObject<JObject>(_json);
                    response = obj.ToObject<SillupostWebAPIResponseModel<FileModel>>();
                    if (response.IsSuccess)
                    {
                        return response.Data;
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

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
    public class CrimeIncidentReportMediaService : ICrimeIncidentReportMediaService
    {
        public async Task<bool> AddAsync(CreateCrimeIncidentReportMediaBindingModel model)
        {
            try
            {
                #region
                using (var client = new HttpClient())
                {

                    client.BaseAddress = new Uri(AppSettingsHelper.goSILUPOST_WEBAPI_URI);
                    string _paramString = AppSettingsHelper.AppSettings.AppToken.AccessToken;
                    client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _paramString);

                    MultipartFormDataContent multiContent = new MultipartFormDataContent();

                    //Handles File Stream and send as FormData
                    HttpContent fileStreamContent = new StreamContent(new MemoryStream(model.File.FileContent));
                    fileStreamContent.Headers.ContentDisposition = new
                    System.Net.Http.Headers.ContentDispositionHeaderValue("form-data")
                    {
                        Name = "File",
                        FileName = model.File.FileName
                    };
                    fileStreamContent.Headers.ContentType = new
                    System.Net.Http.Headers.MediaTypeHeaderValue(model.File.MimeType);
                    multiContent.Add(fileStreamContent);
                    //end

                    //Add json in Form Data
                    model.File = new FileBindingModel();
                    var serializedObject = JsonConvert.SerializeObject(model);
                    multiContent.Add(new StringContent(serializedObject), "model");
                    //end

                    HttpResponseMessage _responseMessage = new HttpResponseMessage();
                    _responseMessage = await client.PostAsync(string.Format("{0}CrimeIncidentReportMedia/createCrimeIncidentReportMedia", AppSettingsHelper.goSILUPOST_WEBAPI_URI), multiContent);
                    string _json = await _responseMessage.Content.ReadAsStringAsync();

                    var response = new SillupostWebAPIResponseModel<CrimeIncidentReportMediaModel>();
                    JObject obj = JsonConvert.DeserializeObject<JObject>(_json);
                    response = obj.ToObject<SillupostWebAPIResponseModel<CrimeIncidentReportMediaModel>>();
                    if (response.IsSuccess)
                    {
                        return response.IsSuccess;
                    }
                    else
                    {
                        var exception = new SilupostServiceException(response.Message);
                        throw new Exception(exception.ExceptionMessage);
                    }
                }
                #endregion
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> DeleteAsync(string id)
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
                    _responseMessage = await client.DeleteAsync(string.Format("{0}CrimeIncidentReportMedia/{1}", AppSettingsHelper.goSILUPOST_WEBAPI_URI, id));
                    string _json = await _responseMessage.Content.ReadAsStringAsync();

                    var response = new SillupostWebAPIResponseModel<object>();
                    JObject obj = JsonConvert.DeserializeObject<JObject>(_json);
                    response = obj.ToObject<SillupostWebAPIResponseModel<object>>();
                    if (response.IsSuccess)
                    {
                        return response.IsSuccess;
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

        public async Task<IEnumerable<CrimeIncidentReportMediaModel>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<CrimeIncidentReportMediaModel> GetAsync(string id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateAsync(UpdateCrimeIncidentReportMediaBindingModel model)
        {
            throw new NotImplementedException();
        }
    }
}

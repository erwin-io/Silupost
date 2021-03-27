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
    public class CrimeIncidentReportService : ICrimeIncidentReportService
    {
        public async Task<bool> AddAsync(CreateCrimeIncidentReportBindingModel model)
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
                    foreach(var media in model.CrimeIncidentReportMedia)
                    {
                        HttpContent fileStreamContent = new StreamContent(new MemoryStream(media.File.FileContent));
                        fileStreamContent.Headers.ContentDisposition = new
                        System.Net.Http.Headers.ContentDispositionHeaderValue("form-data")
                        {
                            Name = "File",
                            FileName = media.File.FileName
                        };
                        fileStreamContent.Headers.ContentType = new
                        System.Net.Http.Headers.MediaTypeHeaderValue(media.File.MimeType);
                        multiContent.Add(fileStreamContent);
                    }
                    //end

                    //Add json in Form Data
                    model.CrimeIncidentReportMedia = new List<NewCrimeIncidentReportMediaBindingModel>();
                    var serializedObject = JsonConvert.SerializeObject(model);
                    multiContent.Add(new StringContent(serializedObject), "model");
                    //end

                    HttpResponseMessage _responseMessage = new HttpResponseMessage();
                    _responseMessage = await client.PostAsync(string.Format("{0}CrimeIncidentReport/createReport", AppSettingsHelper.goSILUPOST_WEBAPI_URI), multiContent);
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

        public async Task<bool> DeleteAsync(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<CrimeIncidentReportModel>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<CrimeIncidentReportModel> GetAsync(string id)
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
                    _responseMessage = await client.GetAsync(string.Format("{0}CrimeIncidentReport/{1}/detail", AppSettingsHelper.goSILUPOST_WEBAPI_URI, id));
                    string _json = await _responseMessage.Content.ReadAsStringAsync();

                    var response = new SillupostWebAPIResponseModel<CrimeIncidentReportModel>();
                    JObject obj = JsonConvert.DeserializeObject<JObject>(_json);
                    response = obj.ToObject<SillupostWebAPIResponseModel<CrimeIncidentReportModel>>();
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

        public async Task<bool> UpdateAsync(UpdateCrimeIncidentReportBindingModel model)
        {
            try
            {
                using (var client = new HttpClient())
                {

                    client.BaseAddress = new Uri(AppSettingsHelper.goSILUPOST_WEBAPI_URI);
                    string _paramString = AppSettingsHelper.AppSettings.AppToken.AccessToken;
                    client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _paramString);

                    var serializedObject = JsonConvert.SerializeObject(model);
                    HttpResponseMessage _responseMessage = new HttpResponseMessage();
                    var _content = new StringContent(serializedObject, Encoding.UTF8, "application/json");
                    _responseMessage = await client.PutAsync(string.Format("{0}CrimeIncidentReport", AppSettingsHelper.goSILUPOST_WEBAPI_URI), _content);
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
                        throw new Exception(response.Message);
                    }
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<CrimeIncidentReportModel>> GetPageReportBySystemUserIdAsync(string SystemUserId, long PageNo, long PageSize)
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
                    _responseMessage = await client.GetAsync(string.Format("{0}CrimeIncidentReport/GetPageByPostedBySystemUserId?PostedBySystemUserId={1}&PageNo={2}&PageSize={3}", AppSettingsHelper.goSILUPOST_WEBAPI_URI, SystemUserId, PageNo, PageSize));
                    string _json = await _responseMessage.Content.ReadAsStringAsync();

                    var response = new SillupostWebAPIResponseModel<List<CrimeIncidentReportModel>>();
                    JObject obj = JsonConvert.DeserializeObject<JObject>(_json);
                    response = obj.ToObject<SillupostWebAPIResponseModel<List<CrimeIncidentReportModel>>>();
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

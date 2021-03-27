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
    public class SystemUserService : ISystemUserService
    {
        public async Task<SystemUserModel> Get(string id)
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
                    _responseMessage = await client.GetAsync(string.Format("{0}SystemUser/{1}/detail", AppSettingsHelper.goSILUPOST_WEBAPI_URI, id));
                    string _json = await _responseMessage.Content.ReadAsStringAsync();

                    var response = new SillupostWebAPIResponseModel<SystemUserModel>();
                    JObject obj = JsonConvert.DeserializeObject<JObject>(_json);
                    response = obj.ToObject<SillupostWebAPIResponseModel<SystemUserModel>>();
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

        public async Task<SystemUserModel> GetByCredentials(string username, string password)
        {
            try
            {
                #region
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(AppSettingsHelper.goSILUPOST_WEBAPI_URI);
                    HttpResponseMessage _responseMessage = new HttpResponseMessage();
                    _responseMessage = await client.GetAsync(string.Format("{0}SystemUser/GetByCredentials?username={1}&password={2}", AppSettingsHelper.goSILUPOST_WEBAPI_URI, username, password));
                    string _json = await _responseMessage.Content.ReadAsStringAsync();

                    var response = new SillupostWebAPIResponseModel<SystemUserModel>();
                    JObject obj = JsonConvert.DeserializeObject<JObject>(_json);
                    response = obj.ToObject<SillupostWebAPIResponseModel<SystemUserModel>>();
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
        public async Task<SystemUserModel> CreateAccount(CreateAccountSystemUserBindingModel model)
        {
            try
            {
                #region
                using (var client = new HttpClient())
                {

                    client.BaseAddress = new Uri(AppSettingsHelper.goSILUPOST_WEBAPI_URI);

                    var serializedObject = JsonConvert.SerializeObject(model);
                    var _content = new StringContent(serializedObject, Encoding.UTF8, "application/json");
                    HttpResponseMessage _responseMessage = new HttpResponseMessage();
                    _responseMessage = await client.PostAsync(string.Format("{0}SystemUser/CreateAccount", AppSettingsHelper.goSILUPOST_WEBAPI_URI), _content);
                    string _json = await _responseMessage.Content.ReadAsStringAsync();

                    var response = new SillupostWebAPIResponseModel<SystemUserModel>();
                    JObject obj = JsonConvert.DeserializeObject<JObject>(_json);
                    response = obj.ToObject<SillupostWebAPIResponseModel<SystemUserModel>>();
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

        public async Task<SystemUserModel> UpdatePersonalDetails(UpdateSystemUserBindingModel model)
        {
            try
            {
                #region
                using (var client = new HttpClient())
                {

                    client.BaseAddress = new Uri(AppSettingsHelper.goSILUPOST_WEBAPI_URI);
                    string _paramString = AppSettingsHelper.AppSettings.AppToken.AccessToken;

                    client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _paramString);

                    var serializedObject = JsonConvert.SerializeObject(model);
                    var _content = new StringContent(serializedObject, Encoding.UTF8, "application/json");
                    HttpResponseMessage _responseMessage = new HttpResponseMessage();
                    _responseMessage = await client.PutAsync(string.Format("{0}SystemUser/UpdatePersonalDetails", AppSettingsHelper.goSILUPOST_WEBAPI_URI), _content);
                    string _json = await _responseMessage.Content.ReadAsStringAsync();

                    var response = new SillupostWebAPIResponseModel<SystemUserModel>();
                    JObject obj = JsonConvert.DeserializeObject<JObject>(_json);
                    response = obj.ToObject<SillupostWebAPIResponseModel<SystemUserModel>>();
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

        public async Task<SystemTokenModel> GetRefreshToken(string RefreshToken)
        {
            throw new NotImplementedException();
        }
        public async Task<SystemUserModel> ResetPassword(UpdateSystemResetPasswordBindingModel model)
        {
            try
            {
                #region
                using (var client = new HttpClient())
                {

                    client.BaseAddress = new Uri(AppSettingsHelper.goSILUPOST_WEBAPI_URI);

                    var serializedObject = JsonConvert.SerializeObject(model);
                    var _content = new StringContent(serializedObject, Encoding.UTF8, "application/json");
                    HttpResponseMessage _responseMessage = new HttpResponseMessage();
                    _responseMessage = await client.PutAsync(string.Format("{0}SystemUser/ResetPassword", AppSettingsHelper.goSILUPOST_WEBAPI_URI), _content);
                    string _json = await _responseMessage.Content.ReadAsStringAsync();

                    var response = new SillupostWebAPIResponseModel<SystemUserModel>();
                    JObject obj = JsonConvert.DeserializeObject<JObject>(_json);
                    response = obj.ToObject<SillupostWebAPIResponseModel<SystemUserModel>>();
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
        public async Task<SystemUserModel> UpdateUsername(UpdateSystemUserNameBindingModel model)
        {
            try
            {
                #region
                using (var client = new HttpClient())
                {

                    client.BaseAddress = new Uri(AppSettingsHelper.goSILUPOST_WEBAPI_URI);
                    string _paramString = AppSettingsHelper.AppSettings.AppToken.AccessToken;

                    client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _paramString);

                    var serializedObject = JsonConvert.SerializeObject(model);
                    var _content = new StringContent(serializedObject, Encoding.UTF8, "application/json");
                    HttpResponseMessage _responseMessage = new HttpResponseMessage();
                    _responseMessage = await client.PutAsync(string.Format("{0}SystemUser/UpdateUsername", AppSettingsHelper.goSILUPOST_WEBAPI_URI), _content);
                    string _json = await _responseMessage.Content.ReadAsStringAsync();

                    var response = new SillupostWebAPIResponseModel<SystemUserModel>();
                    JObject obj = JsonConvert.DeserializeObject<JObject>(_json);
                    response = obj.ToObject<SillupostWebAPIResponseModel<SystemUserModel>>();
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

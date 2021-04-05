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
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace SilupostMobileApp.Services
{
    public class SystemUserVerificationService : ISystemUserVerificationService
    {
        public async Task<SystemUserVerificationModel> GetBySender(string sender, string code)
        {
            try
            {
                #region
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(AppSettingsHelper.goSILUPOST_WEBAPI_URI);
                    HttpResponseMessage _responseMessage = new HttpResponseMessage();
                    _responseMessage = await client.GetAsync(string.Format("{0}SystemUserVerification/GetBySender?sender={1}&code={2}", AppSettingsHelper.goSILUPOST_WEBAPI_URI, sender, code));
                    string _json = await _responseMessage.Content.ReadAsStringAsync();

                    var response = new SillupostWebAPIResponseModel<SystemUserVerificationModel>();
                    JObject obj = JsonConvert.DeserializeObject<JObject>(_json);
                    response = obj.ToObject<SillupostWebAPIResponseModel<SystemUserVerificationModel>>();
                    return response.Data;
                }
                #endregion
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<SystemUserVerificationModel> SendEmailChangePassword(SystemUserVerificationBindingModel model)
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
                    _responseMessage = await client.PostAsync(string.Format("{0}SystemUserVerification/sendEmailChangePasswordRequest", AppSettingsHelper.goSILUPOST_WEBAPI_URI), _content);
                    string _json = await _responseMessage.Content.ReadAsStringAsync();

                    var response = new SillupostWebAPIResponseModel<SystemUserVerificationModel>();
                    JObject obj = JsonConvert.DeserializeObject<JObject>(_json);
                    response = obj.ToObject<SillupostWebAPIResponseModel<SystemUserVerificationModel>>();
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

        public async Task<SystemUserVerificationModel> SendEmailVerification(SystemUserVerificationBindingModel model)
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
                    _responseMessage = await client.PostAsync(string.Format("{0}SystemUserVerification", AppSettingsHelper.goSILUPOST_WEBAPI_URI), _content);
                    string _json = await _responseMessage.Content.ReadAsStringAsync();

                    var response = new SillupostWebAPIResponseModel<SystemUserVerificationModel>();
                    JObject obj = JsonConvert.DeserializeObject<JObject>(_json);
                    response = obj.ToObject<SillupostWebAPIResponseModel<SystemUserVerificationModel>>();
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

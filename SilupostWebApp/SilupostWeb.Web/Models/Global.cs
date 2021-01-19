using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SilupostWeb.Web.Providers;
using Newtonsoft.Json;
using System.Configuration;
using System.Net.Http;
using System.Text;

namespace SilupostWeb.Web.Models
{
    public static class ApplicationSettings
    {
        public static string gBranchId { get; set; }
        public static string gConnectionString { get; set; }
        public static string MD5ServicePrividerKey { get; set; }
    }

    public static class UserLogin
    {
        public static int UserId { get; set; }
        //public static UserViewModel User { get; set; }
    }

    public static class UserMenuRoles
    {
        //public static List<MenuRolesViewModel> MenuRoles { get; set; }
    }

    public static class ApplicationError
    {
        public static ExceptionModel Exception { get; set; }
    }

    public static class GlobalActionExcecutingContext
    {
        public static ActionExecutingContext ActionExecutingContext { get; set; }
    }

    public static class GlobalFunctions
    {
        //IItemGroupFacade _iItemGroupFacade;
        //public GlobalFunctions(IItemGroupFacade iItemGroupFacade)
        //{
        //    _iItemGroupFacade = iItemGroupFacade;
        //}
        public static string ConnectionString()
        {
            return ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString();
        }

        public static string GetApplicationConfig(string pConfigurationKey)
        {
            return ConfigurationManager.AppSettings[pConfigurationKey].ToString();
        }

        public static string GetAppStateUserId()
        {
            var jsonAppState = HttpContext.Current.Request.Cookies["ApplicationSate"];
            var appState = jsonAppState != null ? JsonConvert.DeserializeObject<ApplicationStateModel>(jsonAppState.Value) : null;
            var userId = appState != null ? MD5ServiceProvider.Decrypt(appState.User.UserId, ApplicationSettings.MD5ServicePrividerKey) : string.Empty;
            return userId;
        }

        public static ApplicationStateModel GetAppState()
        {
            var jsonAppState = HttpContext.Current.Request.Cookies["ApplicationSate"];
            var obj = new ApplicationStateModel();
            var hasObj = false;
            try
            {
                obj = jsonAppState != null ? JsonConvert.DeserializeObject<ApplicationStateModel>(jsonAppState.Value) : null;
                hasObj = true;
            }
            catch { }

            if (!hasObj)
            {
                obj = null;
            }

            ApplicationStateModel appState = new ApplicationStateModel();
            if (obj != null)
            {
                appState.User = new ApplicationUserModel
                {
                    UserId = MD5ServiceProvider.Decrypt(obj.User.UserId.ToString(), ApplicationSettings.MD5ServicePrividerKey),
                    LegalEntityId = MD5ServiceProvider.Decrypt(obj.User.LegalEntityId, ApplicationSettings.MD5ServicePrividerKey),
                    Username = MD5ServiceProvider.Decrypt(obj.User.Username, ApplicationSettings.MD5ServicePrividerKey),
                    Firstname = obj.User.Firstname,
                    Middlename = obj.User.Middlename,
                    Lastname = obj.User.Lastname,
                    FullName = obj.User.FullName
                };
            }
            return appState;
        }

        public static ApplicationSateUserViewAccess GetAppStateUserViewAccess()
        {
            var json = HttpContext.Current.Request.Cookies["ApplicationUserViewAccess"];
            var obj = new ApplicationSateUserViewAccess();
            var hasObj = false;
            try
            {
                obj = json != null ? JsonConvert.DeserializeObject<ApplicationSateUserViewAccess>(json.Value) : null;
                hasObj = true;
            }
            catch { }

            if (!hasObj)
            {
                obj = null;
            }

            var appSateView = new ApplicationSateUserViewAccess();
            if (obj != null)
            {
                if (obj.MenuRoles != null)
                {
                    appSateView.MenuRoles = new List<UserViewAccess>();
                    foreach (var menurole in obj.MenuRoles)
                    {
                        var menu = new UserViewAccess
                        {
                            MenuRoleId = MD5ServiceProvider.Decrypt(menurole.MenuRoleId, ApplicationSettings.MD5ServicePrividerKey),
                            MenuId = MD5ServiceProvider.Decrypt(menurole.MenuId, ApplicationSettings.MD5ServicePrividerKey),
                            RoleId = MD5ServiceProvider.Decrypt(menurole.RoleId, ApplicationSettings.MD5ServicePrividerKey),
                            ModuleName = MD5ServiceProvider.Decrypt(menurole.ModuleName, ApplicationSettings.MD5ServicePrividerKey),
                            PageName = MD5ServiceProvider.Decrypt(menurole.PageName, ApplicationSettings.MD5ServicePrividerKey),
                        };
                        appSateView.MenuRoles.Add(menu);
                    }
                }
            }
            return appSateView;
        }

        public static void SetAppState(ApplicationStateModel appState)
        {
            var json = appState != null ? JsonConvert.SerializeObject(appState) : null;
            HttpCookie cookie = new HttpCookie("ApplicationSate", json);
            cookie.Expires = DateTime.Now.AddYears(1);

            HttpContext.Current.Response.Cookies.Add(cookie);
        }

        public static void SetAppStateUserViewAccess(ApplicationSateUserViewAccess appSateUserView)
        {
            var json = appSateUserView != null ? JsonConvert.SerializeObject(appSateUserView) : null;
            HttpCookie cookie = new HttpCookie("ApplicationUserViewAccess", json);
            cookie.Expires = DateTime.Now.AddYears(1);

            HttpContext.Current.Response.Cookies.Add(cookie);
        }

        public static ApplicationActionExcecutingContextModel GetActionExecutingContext()
        {
            var json = HttpContext.Current.Request.Cookies["ActionExecutingContext"];
            var obj = new ApplicationActionExcecutingContextModel();
            var hasObj = false;
            try
            {
                obj = json != null ? JsonConvert.DeserializeObject<ApplicationActionExcecutingContextModel>(json.Value) : null;
                hasObj = true;
            }
            catch { }

            if (!hasObj)
            {
                obj = null;
            }

            var appActionExcecuting = new ApplicationActionExcecutingContextModel();
            if (obj != null)
            {
                appActionExcecuting.Action = MD5ServiceProvider.Decrypt(obj.Action, ApplicationSettings.MD5ServicePrividerKey);
            }
            return appActionExcecuting;
        }

        public static void SetActionExecutingContext(ApplicationActionExcecutingContextModel appActionExcecuting)
        {
            var json = appActionExcecuting != null ? JsonConvert.SerializeObject(appActionExcecuting) : null;
            HttpCookie cookie = new HttpCookie("ActionExecutingContext", json);
            cookie.Expires = DateTime.Now.AddYears(1);

            HttpContext.Current.Response.Cookies.Add(cookie);
        }

        public static ApplicationErrorModel GetApplicationError()
        {
            var appError = (ApplicationErrorModel)HttpContext.Current.Session["ApplicationError"];
            return appError;
        }

        public static void SetApplicationError(ApplicationErrorModel appError)
        {
            if (HttpContext.Current.Session["ApplicationError"] != null)
            {
                HttpContext.Current.Session["ApplicationError"] = appError;
            }
            else
            {
                HttpContext.Current.Session.Add("ApplicationError", appError);
            }
        }

        public static void ResetVisitors_Online()
        {
            HttpContext.Current.Application["visitors_online"] = 0;
        }

        public static int GetVisitors_Online()
        {
            int visitorsOnline = 0;
            if (HttpContext.Current.Application["visitors_online"] != null)
            {
                int.TryParse(HttpContext.Current.Application["visitors_online"].ToString(), out visitorsOnline);
            }
            return visitorsOnline;
        }

        public static void SetVisitors_Online()
        {
            int visitorsOnline = 0;
            if (HttpContext.Current.Application["visitors_online"] != null)
            {
                int.TryParse(HttpContext.Current.Application["visitors_online"].ToString(), out visitorsOnline);
                visitorsOnline = visitorsOnline + 1;
            }
            HttpContext.Current.Session.Timeout = 720; //12 hours
            HttpContext.Current.Application.Lock();
            HttpContext.Current.Application["visitors_online"] = visitorsOnline;
            HttpContext.Current.Application.UnLock();
        }

        public static void SetVisitors_Offline()
        {
            int visitorsOnline = 0;
            if (HttpContext.Current.Application["visitors_online"] != null)
            {
                int.TryParse(HttpContext.Current.Application["visitors_online"].ToString(), out visitorsOnline);
                visitorsOnline = visitorsOnline - 1;
            }
            HttpContext.Current.Application.Lock();
            HttpContext.Current.Application["visitors_online"] = visitorsOnline;
            HttpContext.Current.Application.UnLock();
        }

        public async static void SetCloseLog()
        {
            //using (var client = new HttpClient())
            //{
            //    var itemGroup = new ItemGroupBindingModel()
            //    {
            //        Name = DateTime.Now.ToString("F"),
            //        BranchId = ApplicationSettings.gBranchId,
            //        CreatedBy = "1",
            //    };
            //    string _serialized = JsonConvert.SerializeObject(itemGroup);
            //    StringContent _content = new StringContent(_serialized, Encoding.UTF8, "application/json");
            //    HttpResponseMessage _result = await client.PostAsync("http://localhost:57275/ItemGroup/Create", _content);
            //    if (!_result.IsSuccessStatusCode)
            //    {
            //        string _error = await _result.Content.ReadAsStringAsync();
            //    }
            //}
        }
    }
}
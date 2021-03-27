using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using SilupostMobileApp.Services;
using SilupostMobileApp.Views;
using SilupostMobileApp.Common;
using SilupostMobileApp.Common.Interface;
using SilupostMobileApp.Views.Account;
using System.IO;
using SilupostMobileApp.Models;
using Plugin.Toast;
using SilupostMobileApp.Services.Interface;
using System.Collections.Generic;

namespace SilupostMobileApp
{
    public partial class App : Application
    {
        [assembly: XamlCompilation(XamlCompilationOptions.Compile)]
        public App()
        {
            InitializeComponent();
            try
            {
                MainPage = new MainPage();
                AppSettingsHelper.goSILUPOST_WEBAPI_Authentication = "eyJhbGciOiJodHRwOi8vd3d3LnczLm9yZy8yMDAxLzA0L3htbGRzaWctbW9yZSNobWFjLXNoYTI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1lIjoiZXJ3aW5yYW1pcmV6MjIwQGdtYWlsLmNvbSIsIlN5c3RlbVVzZXJJZCI6IlNVLTAwMDAwMDAwMDEiLCJuYmYiOjE2MTUxMDI1NDEsImV4cCI6MTYxNTE0NTc0MSwiaXNzIjoiaHR0cDovL3d3dy5zaWx1cG9zdHdlYmxhbmRpbmdwYWdlLnNvbWVlLmNvbSIsImF1ZCI6IkFFNDlBMkNDQjJDNTNDMkFGMjE2NUI2MDg3NEQ3MTkxIn0.vJDOZenyKJhqIcfhQpxNO9hJ7tJoBZIr2t-S09afVCI";
                AppSettingsHelper.goMAP_BOX_TOKEN = SilupostAppSettings.MAP_BOX_TOKEN;
            }
            catch(Exception ex)
            {
                CrossToastPopUp.Current.ShowToastMessage(string.Format(" {0}", ex.Message));
            }
        }

        protected override void OnStart()
        {
            DependencyService.Register<MockDataStore>();
            DependencyService.Register<GeoCodeOpenCageDataService>();
            DependencyService.Register<CrimeIncidentReportService>();
            DependencyService.Register<CrimeIncidentReportMediaService>();
            DependencyService.Register<CrimeIncidentCategoryService>();
            DependencyService.Register<SystemLookupService>();
            DependencyService.Register<SystemUserService>();
            DependencyService.Register<SystemUserVerificationService>();

        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
        protected override async void OnAppLinkRequestReceived(Uri uri)
        {
            string originalPath = uri.OriginalString;
            AppSettingsHelper.goLaunchFromURLData = new AppUserSettingsLaunchFromURLDataModel();
            if (originalPath.Contains("emailverification"))
            {
                AppSettingsHelper.goLaunchFromURLData.Type = SilupostLaunchFromURLTypeEnums.EMAIL_CONFIRMATION;
            }
            else
            {
                AppSettingsHelper.goLaunchFromURLData.Type = SilupostLaunchFromURLTypeEnums.CHANGE_PASSWORD;
            }

            string queryString = uri.Query;
            var queryDictionary = System.Web.HttpUtility.ParseQueryString(queryString);

            var accessCode = queryDictionary.Get("code");
            var systemUserId = queryDictionary.Get("id");
            var email = queryDictionary.Get("email");
            AppSettingsHelper.goLaunchFromURLData.Code = accessCode ?? string.Empty;
            AppSettingsHelper.goLaunchFromURLData.SystemUserId = systemUserId ?? string.Empty;
            AppSettingsHelper.goLaunchFromURLData.EmailId = email ?? string.Empty;
            AppSettingsHelper.goIsLaunchFromURL = true;

            MessagingCenter.Send(this, "OpenWelcomePage");

            base.OnAppLinkRequestReceived(uri);
        }
    }
}

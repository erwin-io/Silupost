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
using Plugin.AppShortcuts;
using Plugin.AppShortcuts.Icons;
using System.Linq;
using SilupostMobileApp.ViewModels;

namespace SilupostMobileApp
{
    public partial class App : Application
    {
        [assembly: XamlCompilation(XamlCompilationOptions.Compile)]
        public const string AppShortcutUriBase = "appshortcut://SilupostAppShortcut/";
        public const string ShortcutNewReport = "NEW_REPORT";
        public const string ShortcutEmergencyCall = "EMERGENCY_CALL";
        public const string ShortcutTracker = "TRACKER";
        public const string ShortcutTimeline = "TIMELINE";
        public MainViewModel viewModel;
        public App()
        {
            AddShortcuts();
            InitializeComponent();
            viewModel = new MainViewModel();
            try
            {
                MainPage = new MainPage();
                //AppSettingsHelper.goSILUPOST_WEBAPI_Authentication = "eyJhbGciOiJodHRwOi8vd3d3LnczLm9yZy8yMDAxLzA0L3htbGRzaWctbW9yZSNobWFjLXNoYTI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1lIjoiZXJ3aW5yYW1pcmV6MjIwQGdtYWlsLmNvbSIsIlN5c3RlbVVzZXJJZCI6IlNVLTAwMDAwMDAwMDEiLCJuYmYiOjE2MTUxMDI1NDEsImV4cCI6MTYxNTE0NTc0MSwiaXNzIjoiaHR0cDovL3d3dy5zaWx1cG9zdHdlYmxhbmRpbmdwYWdlLnNvbWVlLmNvbSIsImF1ZCI6IkFFNDlBMkNDQjJDNTNDMkFGMjE2NUI2MDg3NEQ3MTkxIn0.vJDOZenyKJhqIcfhQpxNO9hJ7tJoBZIr2t-S09afVCI";
                //AppSettingsHelper.goMAP_BOX_TOKEN = SilupostAppSettings.MAP_BOX_TOKEN;
            }
            catch (Exception ex)
            {
                SilupostPopMessage.ShowToastMessage(string.Format(" {0}", ex.Message));
            }
        }

        protected override void OnStart()
        {
            DependencyService.Register<AppConnectService>();
            DependencyService.Register<MockDataStore>();
            DependencyService.Register<FileService>();
            DependencyService.Register<GeoCodeOpenCageDataService>();
            DependencyService.Register<CrimeIncidentReportService>();
            DependencyService.Register<CrimeIncidentReportMediaService>();
            DependencyService.Register<CrimeIncidentCategoryService>();
            DependencyService.Register<SystemLookupService>();
            DependencyService.Register<SystemUserService>();
            DependencyService.Register<SystemUserVerificationService>();
            DependencyService.Register<SystemConfigService>();
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
        protected override async void OnAppLinkRequestReceived(Uri uri)
        {
            var scheme = uri.Scheme;
            if (scheme.ToLower().Contains("appshortcut"))
            {
                string originalPath = uri.AbsolutePath.Replace("/", "");
                if (!string.IsNullOrEmpty(originalPath))
                {
                    if (originalPath.Contains(ShortcutNewReport))
                    {
                        MainPage = new MainPage(true, SilupostPageTitle.CRIMEINCIDENT_NEW_REPORT);
                    }
                    else if (originalPath.Contains(ShortcutEmergencyCall))
                    {
                        try
                        {
                            await this.viewModel.PhoneCall.Call(SilupostEmergency.EMERGENCY_CALL_NUMBER);
                        }
                        catch (Exception ex)
                        {
                            SilupostExceptionLogger.GetError(ex);
                        }
                    }
                    else if(originalPath.Contains(ShortcutTracker))
                    {
                        MainPage = new MainPage(true, SilupostPageTitle.CRIMEINCIDENT_MAP);
                    }
                    else
                    {
                        MainPage = new MainPage(true, SilupostPageTitle.TIMELINE);
                    }
                }
            }
            else
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

            }
            base.OnAppLinkRequestReceived(uri);
        }
        async void AddShortcuts()
        {
            if (CrossAppShortcuts.IsSupported)
            {
                var shortCurts = await CrossAppShortcuts.Current.GetShortcuts();

                if (shortCurts.FirstOrDefault(prop => prop.Label == "New Report") == null)
                {
                    var shortcut = new Shortcut()
                    {
                        Label = "New Report",
                        Description = "New Report",
                        Icon = new AddIcon(),
                        Uri = $"{AppShortcutUriBase}{ShortcutNewReport}"
                    };
                    await CrossAppShortcuts.Current.AddShortcut(shortcut);
                }

                if (shortCurts.FirstOrDefault(prop => prop.Label == "Emergency Call") == null)
                {
                    var shortcut = new Shortcut()
                    {
                        Label = "Emergency Call",
                        Description = "Emergency Call",
                        Icon = new CustomIcon("icons8_outgoing_call_96_GRAY.png"),
                        Uri = $"{AppShortcutUriBase}{ShortcutEmergencyCall}"
                    };
                    await CrossAppShortcuts.Current.AddShortcut(shortcut);
                }

                if (shortCurts.FirstOrDefault(prop => prop.Label == "Tracker") == null)
                {
                    var shortcut = new Shortcut()
                    {
                        Label = "Tracker",
                        Description = "Tracker",
                        Icon = new CustomIcon("icons8_marker_96_GRAY.png"),
                        Uri = $"{AppShortcutUriBase}{ShortcutTracker}"
                    };
                    await CrossAppShortcuts.Current.AddShortcut(shortcut);
                }

                if (shortCurts.FirstOrDefault(prop => prop.Label == "Timeline") == null)
                {
                    var shortcut = new Shortcut()
                    {
                        Label = "Timeline",
                        Description = "Timeline",
                        Icon = new CustomIcon("icons8_activity_feed_96_GRAY.png"),
                        Uri = $"{AppShortcutUriBase}{ShortcutTimeline}"
                    };
                    await CrossAppShortcuts.Current.AddShortcut(shortcut);
                }
            }
        }
    }
}

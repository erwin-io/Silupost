using Acr.UserDialogs;
using Plugin.Toast;
using SilupostMobileApp.Common;
using SilupostMobileApp.Models;
using SilupostMobileApp.ViewModels;
using SilupostMobileApp.Views.Account;
using SilupostMobileApp.Views.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SilupostMobileApp.Views.CrimeIncident
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CrimeIncidentMap : ContentPage
    {
        CrimeIncidentMapViewModel viewModel;
        public CrimeIncidentMap()
        {
            InitializeComponent();
            BindingContext = viewModel = new CrimeIncidentMapViewModel(this.Navigation);
            viewModel.Title = SilupostPageTitle.CRIMEINCIDENT_MAP;
            this.viewModel.WebViewURI = string.Format("{0}{1}", AppSettingsHelper.goSILUPOST_WEB_APP_URI, SilupostAppSettings.SILUPOST_WEB_CRIMEINCIDENT_MAP_URI_PATH);

            MessagingCenter.Subscribe<CrimeIncidentMapFilterPage, CrimeIncidentMapFilterModel>(this, "ApplyMapFilter", async (obj, crimeIncidentMapFilter) =>
            {
                this.viewModel.CrimeIncidentMapFilter = crimeIncidentMapFilter;
                await SearchMap();
            });
            MessagingCenter.Subscribe<UserProfilePage>(this, "ReloadProfile", async (obj) =>
            {
                try
                {
                    this.viewModel.ImageSource = ImageSource.FromStream(() => { return new MemoryStream(AppSettingsHelper.AppSettings.UserSettings.FileContent); });
                }
                catch (Exception ex)
                {
                    SilupostExceptionLogger.GetError(ex);
                }
            });

            MessagingCenter.Subscribe<Object>(this, "ApplicationIsOffline", async (obj) =>
            {
                try
                {
                    await this.viewModel.ShowInternetError();
                }
                catch (Exception ex)
                {
                    if (this.viewModel.ProgressDialog != null)
                        this.viewModel.ProgressDialog.Hide();
                }
            });
            MessagingCenter.Subscribe<Object>(this, "ApplicationIsOnline", async (obj) =>
            {
                try
                {
                    this.viewModel.HideInternetError();
                }
                catch (Exception ex)
                {
                    if (this.viewModel.ProgressDialog != null)
                        this.viewModel.ProgressDialog.Hide();
                }
            });
            try
            {
                this.viewModel.ImageSource = ImageSource.FromStream(() => { return new MemoryStream(AppSettingsHelper.AppSettings.UserSettings.FileContent); });
            }
            catch (Exception ex)
            {
                SilupostExceptionLogger.GetError(ex);
            }
        }
        async void WebView_Navigated(object sender, WebNavigatedEventArgs e)
        {
            try
            {
                await LoadMap();
            }
            catch(Exception ex)
            {
                SilupostExceptionLogger.GetError(ex);
            }
        }

        async Task LoadMap()
        {
            try
            {
                if (!AppSettingsHelper.CanAccessInternet())
                {
                    await this.viewModel.ShowInternetError();
                    throw new Exception(SilupostMessage.NO_INTERNET);
                }
                await MapWebView.EvaluateJavaScriptAsync($"reportTracker.appSettings.apiToken = '{AppSettingsHelper.AppSettings.AppToken.AccessToken}';");
                await MapWebView.EvaluateJavaScriptAsync($"reportTracker.init();");
                var gpsEnable = await AppSettingsHelper.GPSEnable();
                if (gpsEnable)
                {
                    var position = await this.viewModel.GetCurrentUserGeoLocation();
                    await this.viewModel.WaitAndExecute(2000, async () =>
                    {
                        await this.viewModel.HideGPSError();
                        await MapWebView.EvaluateJavaScriptAsync($"reportTracker.flyToLocation('{position.Latitude}','{position.Longitude}');");
                    });
                }
                else
                {
                    await this.viewModel.ShowGPSError();
                    await AppSettingsHelper.OpenGPSSettings();
                }
            }
            catch (Exception ex)
            {
                SilupostExceptionLogger.GetError(ex, string.Format("{0} {1}", "Oops! Error loading map please try again.", ex.Message));
            }
        }

        async Task SearchMap()
        {
            try
            {
                if (!AppSettingsHelper.CanAccessInternet())
                {
                    await this.viewModel.ShowInternetError();
                    throw new Exception(SilupostMessage.NO_INTERNET);
                }
                await MapWebView.EvaluateJavaScriptAsync($"reportTracker.appSettings.apiToken = '{AppSettingsHelper.AppSettings.AppToken.AccessToken}';");
                this.viewModel.CrimeIncidentMapFilter.CrimeIncidentCategoryIds = string.Join(",", this.viewModel.CrimeIncidentMapFilter.SelectedCrimeIncidentCategory.Select(x=>x.CrimeIncidentCategoryId).ToArray());
                await MapWebView.EvaluateJavaScriptAsync($"reportTracker.appSettings.trackerFilterMapModel.CrimeIncidentCategoryIds = '{this.viewModel.CrimeIncidentMapFilter.CrimeIncidentCategoryIds}'");
                await MapWebView.EvaluateJavaScriptAsync($"reportTracker.appSettings.trackerFilterMapModel.TrackerRadiusInKM = '{this.viewModel.CrimeIncidentMapFilter.TrackerRadiusInKM}'");
                await MapWebView.EvaluateJavaScriptAsync($"reportTracker.appSettings.trackerFilterMapModel.DateReportedFrom = '{this.viewModel.CrimeIncidentMapFilter.DateReportedFrom.ToString("MM/dd/yyyy")}'");
                await MapWebView.EvaluateJavaScriptAsync($"reportTracker.appSettings.trackerFilterMapModel.DateReportedTo = '{this.viewModel.CrimeIncidentMapFilter.DateReportedTo.ToString("MM/dd/yyyy")}'");
                await MapWebView.EvaluateJavaScriptAsync($"reportTracker.appSettings.trackerFilterMapModel.PossibleDateFrom = '{this.viewModel.CrimeIncidentMapFilter.PossibleDateFrom.ToString("MM/dd/yyyy")}'");
                await MapWebView.EvaluateJavaScriptAsync($"reportTracker.appSettings.trackerFilterMapModel.PossibleDateTo = '{this.viewModel.CrimeIncidentMapFilter.PossibleDateTo.ToString("MM/dd/yyyy")}'");
                await MapWebView.EvaluateJavaScriptAsync($"reportTracker.appSettings.trackerFilterMapModel.PossibleTimeFrom = '{this.viewModel.CrimeIncidentMapFilter.PossibleTimeFrom}'");
                await MapWebView.EvaluateJavaScriptAsync($"reportTracker.appSettings.trackerFilterMapModel.PossibleTimeTo = '{this.viewModel.CrimeIncidentMapFilter.PossibleTimeTo}'");
                await MapWebView.EvaluateJavaScriptAsync($"reportTracker.Search();");
            }
            catch (Exception ex)
            {
                SilupostExceptionLogger.GetError(ex, string.Format("{0} {1}", "Oops! Error loading map please try again.", ex.Message));
            }
        }
        protected override bool OnBackButtonPressed()
        {
            Device.BeginInvokeOnMainThread(async () => {
                var result = await Application.Current.MainPage.DisplayAlert("Exit!", "Do you want to continue?", "Yes", "No");
                if (result)
                {
                    this.viewModel.ApplicationActivity?.CloseApplication();
                }
            });
            return true;
        }

        async void ToggleFilter_CLiked(object sender, EventArgs e)
        {
            try
            {
                if (!AppSettingsHelper.CanAccessInternet())
                {
                    await this.viewModel.ShowInternetError();
                    throw new Exception(SilupostMessage.NO_INTERNET);
                }
                var _isMapLoaded = await MapWebView.EvaluateJavaScriptAsync($"reportTracker.appSettings.IsMapLoaded;");
                this.viewModel.IsMapLoaded = bool.Parse(_isMapLoaded);
                if (this.viewModel.IsMapLoaded)
                {
                    if (this.viewModel.IsExecuting)
                        return;
                    var _viewModel = new CrimeIncidentMapFilterViewModel(this.Navigation);
                    _viewModel.ProgressDialog = UserDialogs.Instance.Loading("Loading...", null, "OK", true, MaskType.Gradient);
                    if (this.viewModel.CrimeIncidentMapFilter == null)
                        this.viewModel.CrimeIncidentMapFilter = new CrimeIncidentMapFilterModel();
                    if(this.viewModel.CrimeIncidentMapFilter.SelectedCrimeIncidentCategory == null)
                        this.viewModel.CrimeIncidentMapFilter.SelectedCrimeIncidentCategory = new List<CrimeIncidentCategoryModel>();
                    var _selectedCrimeIncidentCategory = this.viewModel.CrimeIncidentMapFilter.SelectedCrimeIncidentCategory;
                    var _trackerRadiusInKM = await MapWebView.EvaluateJavaScriptAsync($"reportTracker.appSettings.trackerFilterMapModel.TrackerRadiusInKM;");
                    var _dateReportedFrom = await MapWebView.EvaluateJavaScriptAsync($"reportTracker.appSettings.trackerFilterMapModel.DateReportedFrom;");
                    var _dateReportedTo = await MapWebView.EvaluateJavaScriptAsync($"reportTracker.appSettings.trackerFilterMapModel.DateReportedTo;");
                    var _possibleDateFrom = await MapWebView.EvaluateJavaScriptAsync($"reportTracker.appSettings.trackerFilterMapModel.PossibleDateFrom;");
                    var _possibleDateTo = await MapWebView.EvaluateJavaScriptAsync($"reportTracker.appSettings.trackerFilterMapModel.PossibleDateTo;");
                    var _possibleTimeFrom = await MapWebView.EvaluateJavaScriptAsync($"reportTracker.appSettings.trackerFilterMapModel.PossibleTimeFrom;");
                    var _possibleTimeTo = await MapWebView.EvaluateJavaScriptAsync($"reportTracker.appSettings.trackerFilterMapModel.PossibleTimeTo;");
                    this.viewModel.CrimeIncidentMapFilter.SelectedCrimeIncidentCategory = _selectedCrimeIncidentCategory;
                    this.viewModel.CrimeIncidentMapFilter.TrackerRadiusInKM = double.Parse(_trackerRadiusInKM);
                    this.viewModel.CrimeIncidentMapFilter.DateReportedFrom = DateTime.ParseExact(_dateReportedFrom, "MM/dd/yyyy", null);
                    this.viewModel.CrimeIncidentMapFilter.DateReportedTo = DateTime.ParseExact(_dateReportedTo, "MM/dd/yyyy", null);
                    this.viewModel.CrimeIncidentMapFilter.PossibleDateFrom = DateTime.ParseExact(_possibleDateFrom, "MM/dd/yyyy", null);
                    this.viewModel.CrimeIncidentMapFilter.PossibleDateTo = DateTime.ParseExact(_possibleDateTo, "MM/dd/yyyy", null);
                    this.viewModel.CrimeIncidentMapFilter.PossibleTimeFrom = _possibleTimeFrom;
                    this.viewModel.CrimeIncidentMapFilter.PossibleTimeTo = _possibleTimeTo;
                    this.viewModel.IsExecuting = true;
                    _viewModel.CrimeIncidentMapFilter = this.viewModel.CrimeIncidentMapFilter;
                    await this.Navigation.PushModalAsync(new NavigationPage(new CrimeIncidentMapFilterPage(_viewModel)), true);
                    this.viewModel.IsExecuting = false;
                }
            }
            catch (Exception ex)
            {
                SilupostExceptionLogger.GetError(ex, string.Format("{0} {1}", "Oops! Error loading map please try again.", ex.Message));
            }
        }

        async void ContentPage_Appearing(object sender, EventArgs e)
        {
            try
            {

            }
            catch (Exception ex)
            {
                SilupostExceptionLogger.GetError(ex, string.Format("{0} {1}", "Oops! Error loading map please try again.", ex.Message));
            }
        }

        async void MapWebView_Navigating(object sender, WebNavigatingEventArgs e)
        {
            try
            {
                if (!AppSettingsHelper.CanAccessInternet())
                {
                    await this.viewModel.ShowInternetError();
                    throw new Exception(SilupostMessage.NO_INTERNET);
                }
                var soure = e.Url.ToLower();
                var mapUri = string.Format("{0}{1}", AppSettingsHelper.goSILUPOST_WEB_APP_URI, SilupostAppSettings.SILUPOST_WEB_CRIMEINCIDENT_MAP_URI_PATH).ToLower();
                if (!soure.Equals(mapUri))
                {
                    if (soure.Contains(SilupostAppSettings.SILUPOST_WEB_CRIMEINCIDENT_DETAILS_URI_PATH.ToLower()))
                    {
                        var id = soure.Replace(string.Format("{0}{1}", AppSettingsHelper.goSILUPOST_WEB_APP_URI, SilupostAppSettings.SILUPOST_WEB_CRIMEINCIDENT_DETAILS_URI_PATH).ToLower(), "");
                        OpenReport(id);
                    }
                    e.Cancel = true;
                }
            }
            catch (Exception ex)
            {
                SilupostExceptionLogger.GetError(ex, string.Format("{0} {1}", "Oops! Error loading map please try again.", ex.Message));
            }
        }

        async void OpenReport(string id)
        {
            try
            {
                if (!AppSettingsHelper.CanAccessInternet())
                {
                    await this.viewModel.ShowInternetError();
                    throw new Exception(SilupostMessage.NO_INTERNET);
                }
                if (this.viewModel.IsExecuting)
                    return;
                this.viewModel.IsExecuting = true;
                var _viewModel = new ViewCrimeIncidentReportViewModel(this.Navigation, id);
                _viewModel.ProgressDialog = UserDialogs.Instance.Loading("Loading...", null, "OK", true, MaskType.Gradient);
                await Navigation.PushModalAsync(new NavigationPage(new ViewCrimeIncidentReportPage(_viewModel)), true);
                this.viewModel.IsExecuting = false;
            }
            catch (Exception ex)
            {
                SilupostExceptionLogger.GetError(ex, string.Format("{0} {1}", "Oops! ", ex.Message));
            }
        }

        async void NewReport_CLiked(object sender, EventArgs e)
        {
            try
            {
                if (!AppSettingsHelper.CanAccessInternet())
                {
                    await this.viewModel.ShowInternetError();
                    throw new Exception(SilupostMessage.NO_INTERNET);
                }
                if (this.viewModel.IsExecuting)
                    return;
                this.viewModel.IsExecuting = true;
                var _viewModel = new NewCrimeIncidentReportViewModel(this.Navigation);
                _viewModel.ProgressDialog = UserDialogs.Instance.Loading("Loading...", null, "OK", true, MaskType.Gradient);
                await Navigation.PushModalAsync(new NavigationPage(new NewCrimeIncidentReportPage(_viewModel)), true);
                this.viewModel.IsExecuting = false;
                _viewModel.ProgressDialog.Hide();
            }
            catch (Exception ex)
            {
                this.viewModel.IsExecuting = false;
                SilupostExceptionLogger.GetError(ex);
            }
        }

        async void UserProfile_Clicked(object sender, EventArgs e)
        {
            try
            {
                if (!AppSettingsHelper.CanAccessInternet())
                {
                    await this.viewModel.ShowInternetError();
                    throw new Exception(SilupostMessage.NO_INTERNET);
                }
                if (this.viewModel.IsExecuting)
                    return;
                this.viewModel.IsExecuting = true;
                var _viewModel = new UserProfileViewModel(this.Navigation);
                _viewModel.ProgressDialog = UserDialogs.Instance.Loading("Loading...", null, "OK", true, MaskType.Gradient);
                await Navigation.PushModalAsync(new NavigationPage(new UserProfilePage(_viewModel)), true);
                this.viewModel.IsExecuting = false;
                _viewModel.ProgressDialog.Hide();
            }
            catch (Exception ex)
            {
                SilupostExceptionLogger.GetError(ex);
            }
        }

        async void Retry_Clicked(object sender, EventArgs e)
        {
            try
            {
                if (this.viewModel.HasError && this.viewModel.ErrorMessage.Equals(SilupostMessage.GPS_OR_LOCATION_ERROR))
                {
                    var gpsEnable = await AppSettingsHelper.GPSEnable();
                    if (!gpsEnable)
                    {
                        await this.viewModel.ShowGPSError();
                        await AppSettingsHelper.OpenGPSSettings();
                    }
                    else
                    {
                        var position = await AppSettingsHelper.GetCurrentUserGeoLocation();
                        await this.viewModel.WaitAndExecute(2000, async () =>
                        {
                            await MapWebView.EvaluateJavaScriptAsync($"reportTracker.flyToLocation('{position.Latitude}','{position.Longitude}');");
                            await this.viewModel.HideGPSError();
                        });
                    }
                }
                else if (this.viewModel.HasError && this.viewModel.ErrorMessage.Equals(SilupostMessage.NO_INTERNET))
                {
                    if (!AppSettingsHelper.CanAccessInternet())
                    {
                        await this.viewModel.ShowInternetError();
                    }
                    else
                    {
                        await this.viewModel.HideInternetError();
                    }
                }
            }
            catch (Exception ex)
            {
                SilupostExceptionLogger.GetError(ex);
            }
        }
        async void TrackerCurrentLocationButton_CLick(object sender, EventArgs e)
        {
            try
            {
                if (!AppSettingsHelper.CanAccessInternet())
                {
                    await this.viewModel.ShowInternetError();
                }
                else
                {
                    await this.viewModel.HideInternetError();
                }
                var gpsEnable = await AppSettingsHelper.GPSEnable();
                if (!gpsEnable)
                {
                    await this.viewModel.ShowGPSError();
                    await AppSettingsHelper.OpenGPSSettings();
                }
                else
                {
                    var position = await AppSettingsHelper.GetCurrentUserGeoLocation();
                    await this.viewModel.WaitAndExecute(2000, async () =>
                    {
                        await MapWebView.EvaluateJavaScriptAsync($"reportTracker.flyToLocation('{position.Latitude}','{position.Longitude}');");
                        await this.viewModel.HideGPSError();
                    });
                }
            }
            catch (Exception ex)
            {
                SilupostExceptionLogger.GetError(ex);
            }
        }
    }
}
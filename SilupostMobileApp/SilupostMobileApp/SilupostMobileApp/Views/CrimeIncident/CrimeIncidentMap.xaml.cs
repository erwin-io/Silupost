﻿using Acr.UserDialogs;
using Plugin.Toast;
using SilupostMobileApp.Common;
using SilupostMobileApp.Models;
using SilupostMobileApp.ViewModels;
using SilupostMobileApp.Views.Common;
using System;
using System.Collections.Generic;
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
            this.viewModel.WebViewURI = string.Format("{0}{1}", SilupostAppSettings.SILUPOST_WEB_APP_URI, SilupostAppSettings.SILUPOST_WEB_CRIMEINCIDENT_MAP_URI_PATH);
            MessagingCenter.Subscribe<CrimeIncidentMapFilterPage, CrimeIncidentMapFilterModel>(this, "ApplyMapFilter", async (obj, crimeIncidentMapFilter) =>
            {
                this.viewModel.CrimeIncidentMapFilter = crimeIncidentMapFilter;
                await SearchMap();
            });
        }
        async void WebView_Navigated(object sender, WebNavigatedEventArgs e)
        {
            await LoadMap();
        }

        async Task LoadMap()
        {
            try
            {
                await MapWebView.EvaluateJavaScriptAsync($"reportTracker.appSettings.apiToken = '{AppSettingsHelper.AppSettings.AppToken.AccessToken}';");
                await MapWebView.EvaluateJavaScriptAsync($"reportTracker.init();");

                await this.viewModel.GetCurrentUserGeoLocation();
                await this.viewModel.WaitAndExecute(2000, async () =>
                {
                    await MapWebView.EvaluateJavaScriptAsync($"reportTracker.flyToLocation('{this.viewModel.CurrentUserGeoLocation.Latitude}','{this.viewModel.CurrentUserGeoLocation.Longitude}');");
                });
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
                    this.viewModel.CrimeIncidentMapFilter.DateReportedFrom = DateTime.Parse(_dateReportedFrom);
                    this.viewModel.CrimeIncidentMapFilter.DateReportedTo = DateTime.Parse(_dateReportedTo);
                    this.viewModel.CrimeIncidentMapFilter.PossibleDateFrom = DateTime.Parse(_possibleDateFrom);
                    this.viewModel.CrimeIncidentMapFilter.PossibleDateTo = DateTime.Parse(_possibleDateTo);
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

        private void MapWebView_Navigating(object sender, WebNavigatingEventArgs e)
        {
            var soure = e.Url.ToLower();
            var mapUri = string.Format("{0}{1}", SilupostAppSettings.SILUPOST_WEB_APP_URI, SilupostAppSettings.SILUPOST_WEB_CRIMEINCIDENT_MAP_URI_PATH).ToLower();
            if (!soure.Equals(mapUri))
            {
                if (soure.Contains(SilupostAppSettings.SILUPOST_WEB_CRIMEINCIDENT_DETAILS_URI_PATH.ToLower()))
                {
                    var id = soure.Replace(string.Format("{0}{1}", SilupostAppSettings.SILUPOST_WEB_APP_URI, SilupostAppSettings.SILUPOST_WEB_CRIMEINCIDENT_DETAILS_URI_PATH).ToLower(), "");
                    OpenReport(id);
                }
                e.Cancel = true;
            }
        }

        async void OpenReport(string id)
        {
            if (this.viewModel.IsExecuting)
                return;
            this.viewModel.IsExecuting = true;
            var _viewModel = new ViewCrimeIncidentReportViewModel(this.Navigation, id);
            _viewModel.ProgressDialog = UserDialogs.Instance.Loading("Loading...", null, "OK", true, MaskType.Gradient);
            await Navigation.PushModalAsync(new NavigationPage(new ViewCrimeIncidentReportPage(_viewModel)), true);
            this.viewModel.IsExecuting = false;
        }
    }
}
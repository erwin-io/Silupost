using Acr.UserDialogs;
using Plugin.Toast;
using SilupostMobileApp.Common;
using SilupostMobileApp.Models;
using SilupostMobileApp.ViewModels;
using SilupostMobileApp.Views.Common;
using SilupostMobileApp.Views.Common.MapBox;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SilupostMobileApp.Views.CrimeIncident
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NewCrimeIncidentReportPage : ContentPage
    {
        NewCrimeIncidentReportViewModel viewModel;
        public NewCrimeIncidentReportPage(NewCrimeIncidentReportViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = this.viewModel = viewModel;
            viewModel.Title = SilupostPageTitle.CRIMEINCIDENT_NEW_REPORT;

            MessagingCenter.Subscribe<MapContentPage, GeoLocationModel>(this, "SelectLocation", async (obj, geoLocation) =>
            {
                this.viewModel.GeoLocation = geoLocation;
                await MapWebView.EvaluateJavaScriptAsync($"document.getElementById('Latitude').value = '{this.viewModel.GeoLocation.GeoLatitude}';");
                await MapWebView.EvaluateJavaScriptAsync($"document.getElementById('Longitude').value = '{this.viewModel.GeoLocation.GeoLongitude}';");
                await MapWebView.EvaluateJavaScriptAsync($"FlyToLocation();");
                this.viewModel.ProgressDialog.Hide();
            });

            this.viewModel.Init();
            MapWebView.Source = this.viewModel.WebViewSource;
            this.viewModel.ProgressDialog.Hide();
        }

        async void TitleBackButton_Clicked(object sender, EventArgs e)
        {
            try
            {
                var result = await Application.Current.MainPage.DisplayAlert("Discard Report!", "Do you want to continue?", "Yes", "No");
                if (!result)
                    return;
                if (this.viewModel.IsExecuting)
                    return;
                this.viewModel.IsExecuting = true;
                await this.Navigation.PopModalAsync();
                this.viewModel.IsExecuting = false;
            }
            catch(Exception ex)
            {
                this.viewModel.IsExecuting = false;
                CrossToastPopUp.Current.ShowToastMessage(SilupostMessage.APP_ERROR + string.Format(" {0}", ex.Message));
            }
        }

        async void Media_Clicked(object sender, EventArgs e)
        {
            try
            {
                if (viewModel.IsExecuting)
                    return;
                viewModel.IsExecuting = true;
                var layout = (BindableObject)sender;
                var media = (CrimeIncidentReportMediaModel)layout.BindingContext;
                SilupostDocReportMediaTypeEnums mediaType;
                switch (media.DocReportMediaType.DocReportMediaTypeId)
                {
                    case 1:
                        mediaType = SilupostDocReportMediaTypeEnums.IMAGE;
                        break;
                    case 2:
                        mediaType = SilupostDocReportMediaTypeEnums.VIDEO;
                        break;
                    default:
                        mediaType = SilupostDocReportMediaTypeEnums.AUDIO;
                        break;
                }
                await this.Navigation.PushAsync(new MediaViewerPage(new MediaViewerViewModel(this.Navigation, new SilupostMediaModel { MediaType = mediaType, SourceURL = media.File.SourceURL, ImageSource = media.File.ImageSource })), true);
                viewModel.IsExecuting = false;
            }
            catch(Exception ex)
            {
                this.viewModel.IsExecuting = false;
                CrossToastPopUp.Current.ShowToastMessage(SilupostMessage.APP_ERROR + string.Format(" {0}", ex.Message));
            }
        }

        async void NewMedia_Clicked(object sender, EventArgs e)
        {
            try
            {
                if (this.viewModel.IsExecuting)
                    return;
                this.viewModel.IsExecuting = true;
                await this.Navigation.PushAsync(new MediaUploadViewerPage(new MediaUploadViewerViewModel(this.Navigation)), true);
                this.viewModel.IsExecuting = false;
            }
            catch (Exception ex)
            {
                this.viewModel.IsExecuting = false;
                SilupostExceptionLogger.GetError(ex, SilupostMessage.APP_ERROR + string.Format(" {0}", ex.Message));
            }
        }

        async void RemoveMedia_Tapped(object sender, EventArgs e)
        {
            var result = await Application.Current.MainPage.DisplayAlert("Remove", "Do you want to continue?", "Yes", "No");
            if (!result)
                return;
            try
            {
                if (viewModel.IsExecuting)
                    return;
                viewModel.IsExecuting = true;
                var layout = (BindableObject)sender;
                var media = (CrimeIncidentReportMediaModel)layout.BindingContext;
                this.viewModel.CrimeIncidentReportMediaCollection.Remove(media);
                this.viewModel.MediaListControlHeight = this.viewModel.GetNewHeightAsync();
                this.viewModel.IsExecuting = false;
            }
            catch(Exception ex)
            {
                this.viewModel.IsExecuting = false;
                CrossToastPopUp.Current.ShowToastMessage(SilupostMessage.APP_ERROR + string.Format(" {0}", ex.Message));
            }
        }

        async void SelectCategory_Clicked(object sender, EventArgs e)
        {
            try
            {
                if (this.viewModel.IsExecuting)
                    return;
                this.viewModel.IsExecuting = true;
                var _viewModel = new CrimeIncidentCategoryLookupViewModel(this.Navigation);
                _viewModel.ProgressDialog = UserDialogs.Instance.Loading("Loading...", null, "OK", true, MaskType.Gradient);
                await Navigation.PushAsync(new CrimeIncidentCategoryLookupLookupPage(_viewModel), true);
                this.viewModel.IsExecuting = false;
            }
            catch (Exception ex)
            {
                this.viewModel.IsExecuting = false;
                SilupostExceptionLogger.GetError(ex, SilupostMessage.APP_ERROR + string.Format(" {0}", ex.Message));
            }
        }

        async void Cancel_Cliked(object sender, EventArgs e)
        {
            var result = await Application.Current.MainPage.DisplayAlert("Discard Report!", "Do you want to continue?", "Yes", "No");
            if (!result)
                return;
            if (this.viewModel.IsExecuting)
                return;
            this.viewModel.IsExecuting = true;
            await this.Navigation.PopModalAsync();
            this.viewModel.IsExecuting = false;
        }
        async void OpenLargeMap_Clicked(object sender, EventArgs e)
        {
            if (this.viewModel.IsExecuting)
                return;
            var mapConfig = new MapConfigModel()
            {
                Latitude = this.viewModel.GeoLocation.GeoLatitude,
                Longitude = this.viewModel.GeoLocation.GeoLongitude,
                Radius = 0,
                LookupType = SilupostMapLookupTypeEnums.ADDRESS,
            };
            var _viewModel = new MapContentViewModel(this.Navigation, mapConfig);
            this.viewModel.IsExecuting = true;
            await this.Navigation.PushAsync(new MapContentPage(_viewModel), true);
            this.viewModel.IsExecuting = false;
        }

        protected override bool OnBackButtonPressed()
        {
            Device.BeginInvokeOnMainThread(async () => {
                var result = await Application.Current.MainPage.DisplayAlert("Discard Changes!", "Do you want to continue?", "Yes", "No");
                if (result)
                {
                    await this.Navigation.PopModalAsync();
                }
            });

            return true;
        }

        async void WebView_Navigated(object sender, WebNavigatedEventArgs e)
        {
            try
            {
                await MapWebView.EvaluateJavaScriptAsync($"document.getElementById('AccessToken').value = '{this.viewModel.Token}';");
                await MapWebView.EvaluateJavaScriptAsync($"LoadMap();");

                await MapWebView.EvaluateJavaScriptAsync($"document.getElementById('Latitude').value = '{SilupostAppSettings.DEFAULT_LOCATION_LATITUDE}';");
                await MapWebView.EvaluateJavaScriptAsync($"document.getElementById('Longitude').value = '{SilupostAppSettings.DEFAULT_LOCATION_LONGITUDE}';");
                await MapWebView.EvaluateJavaScriptAsync($"FlyToLocation();");
                var placemarks = await this.viewModel.GeoCodeOpenCageDataService.GetGeoAddressAsync(SilupostAppSettings.DEFAULT_LOCATION_LATITUDE.ToString(), SilupostAppSettings.DEFAULT_LOCATION_LONGITUDE.ToString());
                var placemark = placemarks.Results?.FirstOrDefault();
                this.viewModel.GeoLocation = new GeoLocationModel()
                {
                    GeoLatitude = SilupostAppSettings.DEFAULT_LOCATION_LATITUDE,
                    GeoLongitude = SilupostAppSettings.DEFAULT_LOCATION_LONGITUDE,
                    GeoCountry = placemark.Components.Country,
                    GeoProvince = placemark.Components.State,
                    GeoCityMun = placemark.Components.City ?? placemark.Components.Town ?? placemark.Components.County ?? string.Empty,
                    GeoDistrict = placemark.Components.Village ?? string.Empty,
                    GeoStreet = placemark.Components.Road ?? string.Empty,
                    GeoAddress = placemark.Formatted ?? string.Empty,
                };
                this.viewModel.ProgressDialog.Hide();
            }
            catch (Exception ex)
            {
                this.viewModel.ProgressDialog.Hide();
                SilupostExceptionLogger.GetError(ex, string.Format("{0} {1}", "Oops! Error loading map please try again.", ex.Message));
            }
        }
    }
}
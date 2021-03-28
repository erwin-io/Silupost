using SilupostMobileApp.Common;
using SilupostMobileApp.ViewModels;
using SilupostMobileApp.Models;
using SilupostMobileApp.Views.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Acr.UserDialogs;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using SilupostMobileApp.Views.Common.MapBox;
using Xamarin.Essentials;
using Plugin.Toast;

namespace SilupostMobileApp.Views.CrimeIncident
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ViewCrimeIncidentReportPage : ContentPage
    {
        ViewCrimeIncidentReportViewModel viewModel;
        public ViewCrimeIncidentReportPage(ViewCrimeIncidentReportViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = this.viewModel = viewModel;
            this.viewModel.Title = SilupostPageTitle.CRIMEINCIDENT_VIEW_REPORT;
            MessagingCenter.Subscribe<MapContentPage, GeoLocationModel>(this, "SelectLocation", async (obj, geoLocation) =>
            {
                try
                {
                    this.viewModel.GeoLocation = geoLocation;
                    await MapWebView.EvaluateJavaScriptAsync($"document.getElementById('AccessToken').value = '{this.viewModel.Token}';");
                    await MapWebView.EvaluateJavaScriptAsync($"LoadMap();");

                    await MapWebView.EvaluateJavaScriptAsync($"document.getElementById('Latitude').value = '{this.viewModel.GeoLocation.GeoLatitude}';");
                    await MapWebView.EvaluateJavaScriptAsync($"document.getElementById('Longitude').value = '{this.viewModel.GeoLocation.GeoLongitude}';");
                    await MapWebView.EvaluateJavaScriptAsync($"FlyToLocation();");
                    this.viewModel.ProgressDialog.Hide();
                }
                catch(Exception ex)
                {
                    SilupostExceptionLogger.GetError(ex);
                }
            });
        }

        async void TitleBackButton_Clicked(object sender, EventArgs e)
        {
            try
            {
                if (this.viewModel.CanModifyReport)
                {
                    var result = await Application.Current.MainPage.DisplayAlert("Discard Changes!", "Do you want to continue?", "Yes", "No");
                    if (!result)
                        return;
                }
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
            catch (Exception ex)
            {
                this.viewModel.IsExecuting = false;
                SilupostExceptionLogger.GetError(ex, SilupostMessage.APP_ERROR + string.Format(" {0}", ex.Message));
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
            var result = await Application.Current.MainPage.DisplayAlert("Discard Changes!", "Do you want to continue?", "Yes", "No");
            if (!result)
                return;
            try
            {
                if (this.viewModel.IsExecuting)
                    return;
                this.viewModel.IsExecuting = true;
                await this.Navigation.PopModalAsync(true);
                this.viewModel.IsExecuting = false;
            }
            catch(Exception ex)
            {
                this.viewModel.IsExecuting = false;
                CrossToastPopUp.Current.ShowToastMessage(SilupostMessage.APP_ERROR + string.Format(" {0}", ex.Message));
            }
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

                await MapWebView.EvaluateJavaScriptAsync($"document.getElementById('Latitude').value = '{this.viewModel.CrimeIncidentReport.GeoTrackerLatitude}';");
                await MapWebView.EvaluateJavaScriptAsync($"document.getElementById('Longitude').value = '{this.viewModel.CrimeIncidentReport.GeoTrackerLongitude}';");
                await MapWebView.EvaluateJavaScriptAsync($"FlyToLocation();");
                var placemarks = await this.viewModel.GeoCodeOpenCageDataService.GetGeoAddressAsync(this.viewModel.CrimeIncidentReport.GeoTrackerLatitude.ToString(), this.viewModel.CrimeIncidentReport.GeoTrackerLatitude.ToString());
                var placemark = placemarks.Results?.FirstOrDefault();
                this.viewModel.GeoLocation = new GeoLocationModel()
                {
                    GeoLatitude = this.viewModel.CrimeIncidentReport.GeoTrackerLatitude,
                    GeoLongitude = this.viewModel.CrimeIncidentReport.GeoTrackerLongitude,
                    GeoCountry = this.viewModel.CrimeIncidentReport.GeoCountry,
                    GeoProvince = this.viewModel.CrimeIncidentReport.GeoProvince,
                    GeoCityMun = this.viewModel.CrimeIncidentReport.GeoCityMun,
                    GeoDistrict = this.viewModel.CrimeIncidentReport.GeoDistrict ?? string.Empty,
                    GeoStreet = this.viewModel.CrimeIncidentReport.GeoStreet ?? string.Empty,
                    GeoAddress = this.viewModel.CrimeIncidentReport.GeoAddress ?? string.Empty,
                };
                this.viewModel.ProgressDialog.Hide();
                this.viewModel.IsMapLoaded = true;
            }
            catch (Exception ex)
            {
                this.viewModel.ProgressDialog.Hide();
                SilupostExceptionLogger.GetError(ex, string.Format("{0} {1}", "Oops! Error loading map please try again.", ex.Message));
            }
        }

        async void OpenLargeMap_Clicked(object sender, EventArgs e)
        {
            try
            {
                if (!this.viewModel.IsMapLoaded)
                {
                    throw new Exception("Map is still loading please wait...");
                }
                if (!this.viewModel.CanModifyReport)
                    return;
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
            catch(Exception ex)
            {
                this.viewModel.IsExecuting = false;
                SilupostExceptionLogger.GetError(ex, string.Format("Oops! {0}", ex.Message));
            }
        }

        async void DeleteReport_Clicked(object sender, EventArgs e)
        {
            try
            {
                if (!this.viewModel.CanModifyReport)
                    return;
                var result = await Application.Current.MainPage.DisplayAlert("Delete this report", "Do you want to continue?", "Yes", "No");
                if (!result)
                    return;
                if (this.viewModel.IsExecuting)
                    return;
                await this.viewModel.DeleteReport();
                this.viewModel.IsExecuting = false;
                await this.Navigation.PopModalAsync(true);
                await this.viewModel.WaitAndExecute(1000, async () =>
                {
                    MessagingCenter.Send(this, "ReloadReportList");
                });
            }
            catch (Exception ex)
            {
                this.viewModel.IsExecuting = false;
                SilupostExceptionLogger.GetError(ex, string.Format("Oops! {0}", ex.Message));
            }
        }
    }
}
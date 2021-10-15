using Acr.UserDialogs;
using SilupostMobileApp.Common.Interface;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using SilupostMobileApp.Models;
using SilupostMobileApp.Common;
using SilupostMobileApp.ViewModels;
using Plugin.Geolocator;
using Plugin.Toast;

namespace SilupostMobileApp.Views.Common.MapBox
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MapContentPage : ContentPage
    {
        MapContentViewModel viewModel;
        public MapContentPage(MapContentViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = this.viewModel = viewModel;
            this.viewModel.Title = SilupostPageTitle.CRIMEINCIDENT_VIEW_REPORT;
        }

        async void WebView_Navigated(object sender, WebNavigatedEventArgs e)
        {
            try
            {
                DoneButton.IsEnabled = false;
                DoneButton.BorderColor = Color.LightGray;
                await MapWebView.EvaluateJavaScriptAsync($"document.getElementById('AccessToken').value = '{this.viewModel.Token}';");
                await MapWebView.EvaluateJavaScriptAsync($"document.getElementById('Radius').value = '{this.viewModel.MapConfig.Radius}';");
                await MapWebView.EvaluateJavaScriptAsync($"LoadMap();");
                await MapWebView.EvaluateJavaScriptAsync($"ShowGeoCoder();");

                switch (this.viewModel.MapConfig.LookupType)
                {
                    case SilupostMapLookupTypeEnums.RANGE_OR_DISTANCE:
                        await this.viewModel.WaitAndExecute(2000, async () =>
                        {
                            await MapWebView.EvaluateJavaScriptAsync($"ShowMarkerWithRadius();");
                            RadiusSlider.Value = double.Parse(this.viewModel.MapConfig.Radius.ToString());
                            this.viewModel.ShowRadius = true;
                            this.viewModel.IsMapLoaded = true;
                            this.viewModel.ShowGPS = true;
                        });
                        break;
                    default:
                        await this.viewModel.WaitAndExecute(2000, async () =>
                        {
                            await MapWebView.EvaluateJavaScriptAsync($"document.getElementById('Latitude').value = '{this.viewModel.MapConfig.Latitude}';");
                            await MapWebView.EvaluateJavaScriptAsync($"document.getElementById('Longitude').value = '{this.viewModel.MapConfig.Longitude}';");
                            await MapWebView.EvaluateJavaScriptAsync($"FlyToLocation();");
                            this.viewModel.IsMapLoaded = true;
                            this.viewModel.ShowGPS = true;
                        });
                        break;
                }
            }
            catch (Exception ex)
            {
                SilupostExceptionLogger.GetError(ex, string.Format("{0} {1}", "Oops! Error loading map please try again.", ex.Message));
                this.viewModel.IsMapLoaded = true;
                await this.Navigation.PopModalAsync(true);
            }

        }
        protected override bool OnBackButtonPressed()
        {
            Device.BeginInvokeOnMainThread(async () => {
                if (this.viewModel.IsMapLoaded)
                {
                    await this.Navigation.PopAsync(true);
                }
            });

            return true;
        }

        async void MarkLocation_Clicked(object sender, EventArgs e)
        {
            try
            {
                var RadiusInKM = this.viewModel.MapConfig.Radius;
                if (this.viewModel.ShowRadius)
                {
                    await MapWebView.EvaluateJavaScriptAsync($"document.getElementById('Radius').value = '{RadiusInKM}';");
                    await MapWebView.EvaluateJavaScriptAsync($"ShowMarkerWithRadius();");
                }
                else
                {
                    await MapWebView.EvaluateJavaScriptAsync($"ShowMarker();");
                }

                DoneButton.BorderColor = Color.FromHex("006db3");
                DoneButton.IsEnabled = true;
            }
            catch(Exception ex)
            {
                SilupostPopMessage.ShowToastMessage("Error loading map please try again." + string.Format(" {0}", ex.Message));
            }

        }

        async void RadiusSlider_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            try
            {
                var silder = (Slider)sender;
                var newStep = Math.Round(e.NewValue / 1);
                silder.Value = newStep * 1;
                this.viewModel.MapConfig.Radius = (decimal)silder.Value;
                RadiusLabeValue.FormattedText = string.Format("{0} Km", silder.Value.ToString());
                await RadiusLabeValue.TranslateTo(silder.Value * ((silder.Width - 40) / silder.Maximum), 0, 1);
            }
            catch (Exception ex)
            {
                SilupostExceptionLogger.GetError(ex, string.Format("{0} {1}", "Oops! Error loading map please try again.", ex.Message));
            }
        }

        async void GpsButton_Clicked(object sender, EventArgs e)
        {
            try
            {
                this.viewModel.GpsEnable = this.viewModel.GpsEnable ? false : true;
                if (this.viewModel.GpsEnable)
                    this.viewModel.GpsButtonIconSource = SilupostMapControlIconSource.CURRENT_LOCATION_ON;
                else
                    this.viewModel.GpsButtonIconSource = SilupostMapControlIconSource.CURRENT_LOCATION_OFF;
                var locator = CrossGeolocator.Current;
                var position = await locator.GetLastKnownLocationAsync();
                await MapWebView.EvaluateJavaScriptAsync($"GetCurrentLocation();");

            }
            catch (Exception ex)
            {
                SilupostExceptionLogger.GetError(ex, string.Format("{0} {1}", "Oops! Error loading map please try again.", ex.Message));
            }
        }

        async void DoneButton_Clicked(object sender, EventArgs e)
        {
            try
            {
                if (!this.viewModel.IsMapLoaded)
                    return;
                if (this.viewModel.IsExecuting)
                    return;
                this.viewModel.IsExecuting = true;
                this.viewModel.ProgressDialog = UserDialogs.Instance.Loading("Loading...", null, "OK", true, MaskType.Gradient);
                var _latitude = await MapWebView.EvaluateJavaScriptAsync($"document.getElementById('Latitude').value;");
                var _longitude = await MapWebView.EvaluateJavaScriptAsync($"document.getElementById('Longitude').value;");

                var placemarks = await this.viewModel.GeoCodeOpenCageDataService.GetGeoAddressAsync(_latitude ?? "0", _longitude ?? "0");
                var _radius = this.viewModel.MapConfig.Radius;
                var placemark = placemarks.Results?.FirstOrDefault();
                if (placemark != null)
                {

                    this.viewModel.GeoLocation = new GeoLocationModel()
                    {
                        GeoLatitude = float.Parse(_latitude),
                        GeoLongitude = float.Parse(_longitude),
                        Radius = _radius,
                        GeoCountry = placemark.Components.Country??string.Empty,
                        GeoProvince = placemark.Components.State ?? string.Empty,
                        GeoCityMun = placemark.Components.City ?? placemark.Components.Town ?? placemark.Components.County ?? string.Empty,
                        GeoDistrict = placemark.Components.Village ?? string.Empty,
                        GeoStreet = placemark.Components.Road ?? string.Empty,
                        GeoAddress = placemark.Formatted ?? string.Empty,
                    };

                    MessagingCenter.Send(this, "SelectLocation", this.viewModel.GeoLocation);
                }
                this.viewModel.IsExecuting = false;
                this.viewModel.ProgressDialog.Hide();
                await this.Navigation.PopAsync();
            }
            catch(Exception ex)
            {
                this.viewModel.IsExecuting = false;
                SilupostPopMessage.ShowToastMessage("Error loading map please try again." + string.Format(" {0}", ex.Message));
            }
        }
    }
}
using System;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;
using SilupostMobileApp.Common;
using SilupostMobileApp.Models;
using SilupostMobileApp.Common.Interface;
using System.IO;

namespace SilupostMobileApp.ViewModels
{
    public class MapContentViewModel : BaseViewModel
    {
        #region MODEL PROPERTIES
        MapConfigModel mapConfig;
        public MapConfigModel MapConfig
        {
            get => mapConfig;
            set => SetProperty(ref mapConfig, value);
        }

        GeoLocationModel geoLocation;
        public GeoLocationModel GeoLocation
        {
            get => geoLocation;
            set => SetProperty(ref geoLocation, value);
        }
        UrlWebViewSource webViewSource;
        public UrlWebViewSource WebViewSource
        {
            get => webViewSource;
            set => SetProperty(ref webViewSource, value);
        }

        string token;
        public string Token
        {
            get => token;
            set => SetProperty(ref token, value);
        }

        bool showRadius;
        public bool ShowRadius
        {
            get => showRadius;
            set => SetProperty(ref showRadius, value);
        }

        bool showGPS;
        public bool ShowGPS
        {
            get => showGPS;
            set => SetProperty(ref showGPS, value);
        }

        bool isMapLoaded;
        public bool IsMapLoaded
        {
            get => isMapLoaded;
            set => SetProperty(ref isMapLoaded, value);
        }

        bool gpsEnable;
        public bool GpsEnable
        {
            get => gpsEnable;
            set => SetProperty(ref gpsEnable, value);
        }

        string gpsButtonIconSource;
        public string GpsButtonIconSource
        {
            get => gpsButtonIconSource;
            set => SetProperty(ref gpsButtonIconSource, value);
        }

        #endregion

        #region UI PROPERTIES
        public INavigation Navigation { get; set; }

        #endregion

        #region COMMANDS
        #endregion

        public MapContentViewModel(INavigation pNavigation, MapConfigModel MapConfig)
        {
           this.Title = "Map";
            this.Navigation = pNavigation;
            this.MapConfig = MapConfig;
            this.Token = AppSettingsHelper.goMAP_BOX_TOKEN;
            this.GpsButtonIconSource = SilupostMapControlIconSource.CURRENT_LOCATION_OFF;
            string url = BaseUrl.Get();
            string TempUrl = Path.Combine(url, "mapbox.html");
            WebViewSource = new UrlWebViewSource();
            WebViewSource.Url = TempUrl;
        }
    }
}
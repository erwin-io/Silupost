using Plugin.Toast;
using SilupostMobileApp.Common;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using SilupostMobileApp.Models;
using Plugin.Geolocator.Abstractions;

namespace SilupostMobileApp.ViewModels
{

    public class CrimeIncidentMapViewModel : BaseViewModel
    {
        public INavigation Navigation { get; set; }
        public Command LoginCommand { get; set; }
        public SystemUserModel SystemUser { get; set; }
        public bool IsMapLoaded { get; set; }
        public bool IsFilterOpen { get; set; }
        public Position CurrentUserGeoLocation { get; set; }
        string _webViewURI;
        public string WebViewURI
        {
            get => _webViewURI;
            set => SetProperty(ref _webViewURI, value);
        }
        CrimeIncidentMapFilterModel _crimeIncidentMapFilter;
        public CrimeIncidentMapFilterModel CrimeIncidentMapFilter
        {
            get => _crimeIncidentMapFilter;
            set => SetProperty(ref _crimeIncidentMapFilter, value);
        }

        public CrimeIncidentMapViewModel(INavigation pNavigation)
        {
            this.Navigation = pNavigation;
        }

        public async Task GetCurrentUserGeoLocation()
        {
            try
            {
                this.CurrentUserGeoLocation = await AppSettingsHelper.GetCurrentUserGeoLocation();
            }
            catch (Exception ex)
            {
                CrossToastPopUp.Current.ShowToastMessage("Oops! Error loading map please try again." + string.Format(" {0}", ex.Message));
            }
        }
    }
}

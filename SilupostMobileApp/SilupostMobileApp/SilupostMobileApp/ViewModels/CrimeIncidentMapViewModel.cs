using Plugin.Toast;
using SilupostMobileApp.Common;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using SilupostMobileApp.Models;
using Plugin.Geolocator.Abstractions;
using Acr.UserDialogs;

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

        ImageSource _imageSource;
        public ImageSource ImageSource
        {
            get => _imageSource;
            set => SetProperty(ref _imageSource, value);
        }

        public CrimeIncidentMapViewModel(INavigation pNavigation)
        {
            this.Navigation = pNavigation;
        }

        public async Task<Position> GetCurrentUserGeoLocation()
        {
            var position = new Position();
            try
            {
                position = await AppSettingsHelper.GetCurrentUserGeoLocation();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return position;
        }

        public async Task ShowInternetError()
        {
            try
            {
                if (this.HasError && !string.IsNullOrEmpty(this.ErrorMessage) && this.ErrorMessage.Equals(SilupostMessage.NO_INTERNET))
                    return;
                this.ProgressDialog = UserDialogs.Instance.Loading("Loading...", null, "OK", true, MaskType.Gradient);

                await this.WaitAndExecute(1000, async () =>
                {
                    this.HasError = true;
                    this.NoRecordFound = true;
                    this.IsExecuting = false;
                    this.IsBusy = false;
                    this.ErrorMessage = string.Format("{0}", SilupostMessage.NO_INTERNET);
                    this.ErrorImageSource = "icons8_without_internet_96.png";
                    if (this.ProgressDialog != null)
                        this.ProgressDialog.Hide();
                });
            }
            catch (Exception ex)
            {
                SilupostExceptionLogger.GetError(ex);
            }
        }

        public async Task ShowGPSError()
        {
            try
            {
                if (this.HasError && !string.IsNullOrEmpty(this.ErrorMessage) && this.ErrorMessage.Equals(SilupostMessage.GPS_OR_LOCATION_ERROR))
                    return;
                this.ProgressDialog = UserDialogs.Instance.Loading("Loading...", null, "OK", true, MaskType.Gradient);

                await this.WaitAndExecute(1000, async () =>
                {
                    this.HasError = true;
                    this.NoRecordFound = true;
                    this.IsExecuting = false;
                    this.IsBusy = false;
                    this.ErrorMessage = string.Format("{0}", SilupostMessage.GPS_OR_LOCATION_ERROR);
                    this.ErrorImageSource = "icons8_location_80.png";
                    if (this.ProgressDialog != null)
                        this.ProgressDialog.Hide();
                });
            }
            catch (Exception ex)
            {
                SilupostExceptionLogger.GetError(ex);
            }
        }
        public async Task HideInternetError()
        {
            try
            {
                if (this.HasError && !string.IsNullOrEmpty(this.ErrorMessage) && this.ErrorMessage.Equals(SilupostMessage.NO_INTERNET))
                {
                    this.HasError = false;
                }
            }
            catch (Exception ex)
            {
                SilupostExceptionLogger.GetError(ex);
            }
        }
        public async Task HideGPSError()
        {
            try
            {
                if (this.HasError && !string.IsNullOrEmpty(this.ErrorMessage) && this.ErrorMessage.Equals(SilupostMessage.GPS_OR_LOCATION_ERROR))
                {
                    this.HasError = false;
                }
            }
            catch (Exception ex)
            {
                SilupostExceptionLogger.GetError(ex);
            }
        }
    }
}

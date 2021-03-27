using Plugin.Toast;
using SilupostMobileApp.Common;
using SilupostMobileApp.Views.Common;
using SilupostMobileApp.Views.Common.MapBox;
using SilupostMobileApp.Common.Interface;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using SilupostMobileApp.Models;
using System.IO;
using System.Collections.ObjectModel;
using SilupostMobileApp.BindingModels;
using Acr.UserDialogs;
using Plugin.Geolocator;
using Xamarin.Essentials;
using SilupostMobileApp.CustomRender;

namespace SilupostMobileApp.ViewModels
{
    public class NewCrimeIncidentReportViewModel : BaseViewModel
    {
        #region MODEL PROPERTIES
        CrimeIncidentReportModel _crimeIncidentReport;
        public CrimeIncidentReportModel CrimeIncidentReport
        {
            get => _crimeIncidentReport;
            set => SetProperty(ref _crimeIncidentReport, value);
        }
        CrimeIncidentCategoryModel _crimeIncidentCategory;
        public CrimeIncidentCategoryModel CrimeIncidentCategory
        {
            get => _crimeIncidentCategory;
            set => SetProperty(ref _crimeIncidentCategory, value);
        }

        ObservableCollection<CrimeIncidentReportMediaModel> _crimeIncidentReportMediaCollection;
        public ObservableCollection<CrimeIncidentReportMediaModel> CrimeIncidentReportMediaCollection
        {
            get => _crimeIncidentReportMediaCollection;
            set => SetProperty(ref _crimeIncidentReportMediaCollection, value);
        }

        decimal _mediaListControlHeight;
        public decimal MediaListControlHeight
        {
            get => _mediaListControlHeight;
            set => SetProperty(ref _mediaListControlHeight, value);
        }
        //for MAP

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

        //end for MAP
        #endregion

        #region UI PROPERTIES
        public INavigation Navigation { get; set; }


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

        bool mapViewFocused;
        public bool MapViewFocused
        {
            get => mapViewFocused;
            set => SetProperty(ref mapViewFocused, value);
        }
        #endregion

        #region COMMANDS
        public Command SaveCommand { get; set; }
        #endregion

        public NewCrimeIncidentReportViewModel(INavigation pNavigation)
        {
            this.Navigation = pNavigation;
            //this.CrimeIncidentReportId = CrimeIncidentReportId;
            this.SaveCommand = new Command(async () => await Save());
            this.Token = AppSettingsHelper.goMAP_BOX_TOKEN;

            MessagingCenter.Subscribe<MediaUploadViewerPage, SilupostMediaModel>(this, "UploadMedia", async (obj, item) =>
            {
                var newItem = item as SilupostMediaModel;
                this.CrimeIncidentReportMediaCollection.Add(new CrimeIncidentReportMediaModel() 
                {
                    DocReportMediaType = new DocReportMediaTypeModel() { DocReportMediaTypeId = (int)newItem.MediaType, IconSource = newItem.IconSource },
                    File = newItem,
                    CrimeIncidentReport = this.CrimeIncidentReport,
                    Caption = string.Empty,
                    IsNew = newItem.IsNew,
                });
                this.MediaListControlHeight = GetNewHeightAsync();
            });
            MessagingCenter.Subscribe<CrimeIncidentCategoryLookupLookupPage, CrimeIncidentCategoryModel>(this, "SelectCategory", async (obj, category) =>
            {
                this.CrimeIncidentCategory = new CrimeIncidentCategoryModel()
                {
                    CrimeIncidentCategoryId = category.CrimeIncidentCategoryId,
                    CrimeIncidentCategoryName = category.CrimeIncidentCategoryName,
                    CrimeIncidentCategoryDescription = category.CrimeIncidentCategoryDescription,
                    CrimeIncidentType = category.CrimeIncidentType
                };
            });
        }

        public async Task Init()
        {
            try
            {
                string url = BaseUrl.Get();
                string TempUrl = Path.Combine(url, "mapbox.html");
                WebViewSource = new UrlWebViewSource();
                WebViewSource.Url = TempUrl;

                this.CrimeIncidentReport = new CrimeIncidentReportModel()
                {
                    PossibleDate = DateTime.Now,
                    PossibleTime = DateTime.Now.ToString("hh:mm tt")
                };
                this.CrimeIncidentCategory = new CrimeIncidentCategoryModel()
                {
                    CrimeIncidentCategoryName = "Select Category"
                };
                this.CrimeIncidentReportMediaCollection = new ObservableCollection<CrimeIncidentReportMediaModel>();

            }
            catch(Exception ex)
            {
                ProgressDialog.Hide();
                CrossToastPopUp.Current.ShowToastMessage(ex.Message);
            }
        }

        public decimal GetNewHeightAsync()
        {
            int itemCount = CrimeIncidentReportMediaCollection.Count;
            decimal rows = (decimal.Parse(itemCount.ToString()) + decimal.Parse("0.5")) / 2;
            decimal rounded = Math.Round(rows);
            return rounded * 150;
        }

        public async Task Save()
        {
            if (string.IsNullOrEmpty(this.CrimeIncidentCategory.CrimeIncidentCategoryId))
            {
                CrossToastPopUp.Current.ShowToastMessage("Please select category!");
                return;
            }
            var result = await Application.Current.MainPage.DisplayAlert("Submit Report", "Do you want to continue?", "Yes", "No");
            if (!result)
                return;
            if (this.IsExecuting)
                return;
            ProgressDialog = UserDialogs.Instance.Loading("Saving please wait...", null, "OK", true, MaskType.Gradient);
            this.IsExecuting = true;
            try
            {
                var _crimeIncidentReportMedia = new List<NewCrimeIncidentReportMediaBindingModel>();
                foreach (var media in this.CrimeIncidentReportMediaCollection)
                {
                    _crimeIncidentReportMedia.Add(new NewCrimeIncidentReportMediaBindingModel()
                    {
                        DocReportMediaTypeId = int.Parse(media.DocReportMediaType.DocReportMediaTypeId.ToString()),
                        Caption = media.Caption,
                        File = new FileBindingModel()
                        {
                            IsDefault = false,
                            FileName = media.File.FileName,
                            MimeType = media.File.MimeType,
                            FileContent = media.File.FileContent,
                            FileSize = media.File.FileSize
                        }
                    });
                }
                var model = new CreateCrimeIncidentReportBindingModel()
                {
                    PostedBySystemUserId = AppSettingsHelper.AppSettings.UserSettings.SystemUserId,
                    DateReported = DateTime.Now,
                    CrimeIncidentReportMedia = _crimeIncidentReportMedia,
                    CrimeIncidentCategoryId = this.CrimeIncidentCategory.CrimeIncidentCategoryId,
                    PossibleDate = this.CrimeIncidentReport.PossibleDate,
                    PossibleTime = this.CrimeIncidentReport.PossibleTime,
                    Description = this.CrimeIncidentReport.Description,
                    GeoStreet = this.GeoLocation.GeoStreet,
                    GeoDistrict = this.GeoLocation.GeoDistrict,
                    GeoCityMun = this.GeoLocation.GeoCityMun,
                    GeoProvince = this.GeoLocation.GeoProvince,
                    GeoCountry = this.GeoLocation.GeoCountry,
                    GeoTrackerLatitude = this.GeoLocation.GeoLatitude.ToString(),
                    GeoTrackerLongitude = this.GeoLocation.GeoLongitude.ToString(),
                    IsReviewActionEnable = this.CrimeIncidentReport.IsReviewActionEnable,
                    IsReviewCommentEnable = this.CrimeIncidentReport.IsReviewCommentEnable
                };
                bool success = await CrimeIncidentReportService.AddAsync(model);
                if (success)
                {
                    ProgressDialog.Hide();
                    CrossToastPopUp.Current.ShowToastMessage(string.Format(SilupostMessage.SUCCESS_SAVED, "Report"));
                    this.IsExecuting = false;
                    await this.Navigation.PopModalAsync(true);
                }
                else
                {
                    CrossToastPopUp.Current.ShowToastMessage(SilupostMessage.SERVER_ERROR);
                    this.IsExecuting = false;
                    return;
                }
            }
            catch(Exception ex)
            {
                CrossToastPopUp.Current.ShowToastMessage(ex.Message);
                this.IsExecuting = false;
                return;
            }
        }
    }
}

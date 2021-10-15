using Plugin.Toast;
using SilupostMobileApp.Common;
using SilupostMobileApp.Views.Common;
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

namespace SilupostMobileApp.ViewModels
{

    public class ViewCrimeIncidentReportViewModel : BaseViewModel
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
        public string CrimeIncidentReportId { get; set; }


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

        bool canModifyReport;
        public bool CanModifyReport
        {
            get => canModifyReport;
            set => SetProperty(ref canModifyReport, value);
        }
        #endregion

        #region COMMANDS
        public Command SaveCommand { get; set; }
        #endregion

        public ViewCrimeIncidentReportViewModel(INavigation pNavigation, string CrimeIncidentReportId)
        {
            this.Navigation = pNavigation;
            this.CrimeIncidentReportId = CrimeIncidentReportId;
            this.SaveCommand = new Command(async () => await Save());
            this.Token = AppSettingsHelper.goMAP_BOX_TOKEN;
            this.IsBusy = true;

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
            InitViewCrimeIncidentReport();
        }

        public async Task InitViewCrimeIncidentReport()
        {
            await this.WaitAndExecute(1000, async () =>
            {
                try
                {
                    this.HasError = false;
                    var result = await CrimeIncidentReportService.GetAsync(this.CrimeIncidentReportId);
                    if(result != null && !result.IsSuccess)
                    {
                        if (!string.IsNullOrEmpty(result.Message))
                        {
                            throw new Exception(result.Message);
                        }
                        else
                        {
                            this.ErrorMessage = SilupostMessage.APP_ERROR;
                            throw new Exception(SilupostMessage.APP_ERROR);
                        }
                    }
                    this.CrimeIncidentReport = result.Data;
                    this.CanModifyReport = this.CrimeIncidentReport.PostedBySystemUser.SystemUserId.Equals(AppSettingsHelper.AppSettings.UserSettings.SystemUserId);
                    this.CrimeIncidentCategory = this.CrimeIncidentReport.CrimeIncidentCategory;
                    this.CrimeIncidentReportMediaCollection = new ObservableCollection<CrimeIncidentReportMediaModel>();
                    foreach (var media in CrimeIncidentReport.CrimeIncidentReportMedia)
                    {
                        var fileUrl = string.Format("{0}File/getFile?FileId={1}", AppSettingsHelper.goSILUPOST_WEBAPI_URI, media.File.FileId);
                        media.File.SourceURL = fileUrl;
                        if (media.DocReportMediaType.DocReportMediaTypeId == (int)SilupostDocReportMediaTypeEnums.VIDEO)
                        {
                            var thumbnail = MediaHelpers.GenerateThumbImageWeb(fileUrl, 1);
                            media.File.ImageSource = thumbnail;
                        }
                        else
                        {
                            var image = new Image { Source = fileUrl };
                            media.File.ImageSource = image.Source;
                        }
                        switch ((SilupostDocReportMediaTypeEnums)media.DocReportMediaType.DocReportMediaTypeId)
                        {
                            case SilupostDocReportMediaTypeEnums.AUDIO:
                                media.DocReportMediaType.IconSource = SilupostMediaTypeIconSource.AUDIO;
                                break;
                            case SilupostDocReportMediaTypeEnums.VIDEO:
                                media.DocReportMediaType.IconSource = SilupostMediaTypeIconSource.VIDEO;
                                break;
                            case SilupostDocReportMediaTypeEnums.IMAGE:
                                media.DocReportMediaType.IconSource = SilupostMediaTypeIconSource.IMAGE;
                                break;
                        }
                        CrimeIncidentReportMediaCollection.Add(media);
                    }
                    this.MediaListControlHeight = GetNewHeightAsync();

                    string url = BaseUrl.Get();
                    string TempUrl = Path.Combine(url, "mapbox.html");
                    WebViewSource = new UrlWebViewSource();
                    WebViewSource.Url = TempUrl;

                    await this.WaitAndExecute(1000, async () =>
                    {
                        ProgressDialog.Hide();
                        this.IsBusy = false;
                    });
                }
                catch(Exception ex)
                {
                    this.HasError = true;
                    this.NoRecordFound = true;
                    this.IsExecuting = false;
                    this.IsBusy = false;
                    if (ex.Message.ToLower().Contains("no record"))
                    {
                        this.ErrorMessage = string.Format("{0}", ex.Message);
                        this.ErrorImageSource = "icons8_nothing_found_96.png";
                    }
                    else if (ex.Message.ToLower().Contains("problem occurs while proccessing"))
                    {
                        this.ErrorMessage = string.Format("{0}", ex.Message);
                        this.ErrorImageSource = "icons8_online_maintenance_portal_96.png";
                    }
                    else
                    {
                        this.ErrorMessage = string.Format("{0}", SilupostMessage.APP_ERROR);
                        this.ErrorImageSource = "icons8_error_80.png";
                    }
                    SilupostExceptionLogger.GetError(ex);
                    this.ProgressDialog.Hide();
                }
            });
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
            try
            {
                if (!this.IsMapLoaded)
                {
                    throw new Exception("Map is still loading please wait...");
                }
                if (this.IsBusy)
                    return;
                var result = await Application.Current.MainPage.DisplayAlert("Update", "Do you want to continue?", "Yes", "No");
                if (!result)
                    return;
                if (this.IsExecuting)
                    return;
                ProgressDialog = UserDialogs.Instance.Loading("Saving please wait...", null, "OK", true, MaskType.Gradient);
                this.IsExecuting = true;
                var model = new UpdateCrimeIncidentReportBindingModel()
                {
                    CrimeIncidentReportId = this.CrimeIncidentReport.CrimeIncidentReportId,
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
                bool success = await CrimeIncidentReportService.UpdateAsync(model);
                if (success)
                {
                    var newMediaCollection = this.CrimeIncidentReportMediaCollection.Where(x => x.IsNew);
                    var oldMediaCollection = this.CrimeIncidentReport.CrimeIncidentReportMedia.ToList();
                    var toRemoveMediaCollection = oldMediaCollection.Where(x => !this.CrimeIncidentReportMediaCollection.Any(y=>y.CrimeIncidentReportMediaId == x.CrimeIncidentReportMediaId)).ToList();
                    foreach (var media in newMediaCollection)
                    {
                        var mediaModel = new CreateCrimeIncidentReportMediaBindingModel()
                        {
                            CrimeIncidentReportId = this.CrimeIncidentReport.CrimeIncidentReportId,
                            File = new FileBindingModel()
                            {
                                IsDefault = false,
                                FileName = media.File.FileName,
                                MimeType = media.File.MimeType,
                                FileContent = media.File.FileContent,
                                FileSize = media.File.FileSize
                            },
                            DocReportMediaTypeId = int.Parse(media.DocReportMediaType.DocReportMediaTypeId.ToString()),
                            Caption = media.Caption,
                        };
                        success = await CrimeIncidentReportMedia.AddAsync(mediaModel);
                        if (!success)
                        {
                            SilupostPopMessage.ShowToastMessage(SilupostMessage.SERVER_ERROR);
                            this.IsExecuting = false;
                            return;
                        }
                        File.Delete(media.File.FileName);
                    }
                    foreach (var media in toRemoveMediaCollection)
                    {
                        success = await CrimeIncidentReportMedia.DeleteAsync(media.CrimeIncidentReportMediaId);
                        if (!success)
                        {
                            SilupostPopMessage.ShowToastMessage(SilupostMessage.SERVER_ERROR);
                            this.IsExecuting = false;
                            return;
                        }
                    }
                    ProgressDialog.Hide();
                    SilupostPopMessage.ShowToastMessage(string.Format(SilupostMessage.SUCCESS_SAVED, "Report"));
                    this.IsExecuting = false;
                    await this.Navigation.PopModalAsync(true);
                    await this.WaitAndExecute(1000, async () =>
                    {
                        MessagingCenter.Send(this, "ReloadReportList");
                    });
                }
                else
                {
                    ProgressDialog.Hide();
                    SilupostPopMessage.ShowToastMessage(SilupostMessage.SERVER_ERROR);
                    this.IsExecuting = false;
                    return;
                }
            }
            catch(Exception ex)
            {
                ProgressDialog.Hide();
                SilupostExceptionLogger.GetError(ex, string.Format("Oops!  {0}", ex.Message));
                this.IsExecuting = false;
                return;
            }
        }

        public async Task<bool> DeleteReport()
        {
            try
            {
                return await CrimeIncidentReportService.DeleteAsync(this.CrimeIncidentReportId);
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
    }
}

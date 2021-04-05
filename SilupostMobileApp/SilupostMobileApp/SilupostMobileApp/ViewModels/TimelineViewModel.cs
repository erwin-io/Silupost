using Plugin.Toast;
using SilupostMobileApp.Common;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using SilupostMobileApp.Models;
using System.Collections.ObjectModel;
using System.Linq;
using System.IO;

namespace SilupostMobileApp.ViewModels
{

    public class TimelineViewModel : BaseViewModel
    {

        #region MODEL PROPERTIES
        private long CurrentPageNo { get; set; }
        private long PageSize { get; set; }


        private ObservableCollection<CrimeIncidentReportModel> _crimeIncidentReport;
        public ObservableCollection<CrimeIncidentReportModel> CrimeIncidentReport
        {
            get => _crimeIncidentReport;
            set => SetProperty(ref _crimeIncidentReport, value);
        }

        ObservableCollection<GroupingModel<DateTime, CrimeIncidentReportModel>> _groupedCrimeIncidentReport;
        public ObservableCollection<GroupingModel<DateTime, CrimeIncidentReportModel>> GroupedCrimeIncidentReport
        {
            get => _groupedCrimeIncidentReport;
            set => SetProperty(ref _groupedCrimeIncidentReport, value);
        }
        
        #endregion

        #region UI PROPERTIES
        public INavigation Navigation { get; set; }

        private bool _isProcessingRefresh;
        public bool IsProcessingRefresh
        {
            get => _isProcessingRefresh;
            set => SetProperty(ref _isProcessingRefresh, value);
        }

        ImageSource _imageSource;
        public ImageSource ImageSource
        {
            get => _imageSource;
            set => SetProperty(ref _imageSource, value);
        }
        bool _showCrimeReportList;
        public bool ShowCrimeReportList
        {
            get => _showCrimeReportList;
            set => SetProperty(ref _showCrimeReportList, value);
        }

        #endregion

        #region COMMANDS
        public Command RefreshCommand { get; set; }
        #endregion

        public TimelineViewModel(INavigation pNavigation)
        {
            this.Navigation = pNavigation;
            CrimeIncidentReport = new ObservableCollection<CrimeIncidentReportModel>();
            RefreshCommand = new Command(async () => await InitSystemUserTimeline());
        }

        public async Task InitSystemUserTimeline()
        {
            try
            {
                IsProcessingRefresh = true;
                this.CurrentPageNo = 1;
                this.PageSize = 5;
                this.NoRecordFound = true;
                this.HasError = false;

                if (!AppSettingsHelper.CanAccessInternet())
                {
                    throw new Exception(SilupostMessage.NO_INTERNET);
                }

                CrimeIncidentReport = new ObservableCollection<CrimeIncidentReportModel>();
                var result = await CrimeIncidentReportService.GetPageReportBySystemUserIdAsync(AppSettingsHelper.AppSettings.UserSettings.SystemUserId, CurrentPageNo, PageSize);
                if (result != null && !result.IsSuccess)
                {
                    if (!string.IsNullOrEmpty(result.Message))
                    {
                        throw new Exception(result.Message);
                    }
                    else
                    {
                        throw new Exception(SilupostMessage.APP_ERROR);
                    }
                }
                if (result != null && result.Data.Count == 0)
                {
                    throw new Exception(string.Format(SilupostMessage.NO_RECORDS_FOUND, "report"));
                }
                else
                {
                    this.NoRecordFound = false;
                }
                foreach (var report in result.Data)
                {
                    if (!CrimeIncidentReport.Any(x => x.CrimeIncidentReportId == report.CrimeIncidentReportId))
                    {
                        report.CrimeIncidentCategory.CrimeIncidentType.IconFile.ImageSource = ImageSource.FromStream(() => { return new MemoryStream(report.CrimeIncidentCategory.CrimeIncidentType.IconFile.FileContent); });
                        CrimeIncidentReport.Add(report);
                    }
                    else
                    {
                        int i = CrimeIncidentReport.ToList().FindIndex(x => x.CrimeIncidentReportId == report.CrimeIncidentReportId);
                        report.CrimeIncidentCategory.CrimeIncidentType.IconFile.ImageSource = ImageSource.FromStream(() => { return new MemoryStream(report.CrimeIncidentCategory.CrimeIncidentType.IconFile.FileContent); });
                        CrimeIncidentReport[i] = report;
                    }
                }

                GroupedCrimeIncidentReport = new ObservableCollection<GroupingModel<DateTime, CrimeIncidentReportModel>>();
                var sorted = from report in CrimeIncidentReport
                             orderby report.DateReported descending
                             group report by report.DateReported.ToString("MMMM/yyyy") into reportGroup
                             select new GroupingModel<DateTime, CrimeIncidentReportModel>(DateTime.Parse(reportGroup.Key), reportGroup);

                //create a new collection of groups
                GroupedCrimeIncidentReport = new ObservableCollection<GroupingModel<DateTime, CrimeIncidentReportModel>>(sorted);
                this.ShowCrimeReportList = this.GroupedCrimeIncidentReport.Count > 0;
                this.IsProcessingRefresh = false;
                this.HasError = false;
            }
            catch (Exception ex)
            {
                CrimeIncidentReport = new ObservableCollection<CrimeIncidentReportModel>();
                GroupedCrimeIncidentReport = new ObservableCollection<GroupingModel<DateTime, CrimeIncidentReportModel>>();

                this.HasError = true;
                this.NoRecordFound = true;
                this.IsExecuting = false;
                this.IsBusy = false;
                if (ex.Message.ToLower().Contains("no report record found") || ex.Message.ToLower().Contains("no record found"))
                {
                    this.ErrorMessage = string.Format("{0}", ex.Message);
                    this.ErrorImageSource = "icons8_nothing_found_96.png";
                }
                else if (ex.Message.ToLower().Contains("problem occurs while proccessing"))
                {
                    this.ErrorMessage = string.Format("{0}", ex.Message);
                    this.ErrorImageSource = "icons8_online_maintenance_portal_96.png";
                }
                else if (ex.Message.Contains(SilupostMessage.NO_INTERNET))
                {
                    this.ErrorMessage = string.Format("{0}", ex.Message);
                    this.ErrorImageSource = "icons8_without_internet_96.png";
                }
                else
                {
                    this.ErrorMessage = string.Format("{0}", SilupostMessage.APP_ERROR);
                    this.ErrorImageSource = "icons8_error_80.png";
                }
                SilupostExceptionLogger.GetError(ex);
                if(this.ProgressDialog!= null)
                    this.ProgressDialog.Hide();
            }
        }
        public async Task LoadMore()
        {
            try
            {
                IsProcessingRefresh = true;
                this.CurrentPageNo++;
                this.PageSize = 5;
                this.NoRecordFound = true;
                this.HasError = false;


                if (!AppSettingsHelper.CanAccessInternet())
                {
                    throw new Exception(SilupostMessage.NO_INTERNET);
                }

                var result = await CrimeIncidentReportService.GetPageReportBySystemUserIdAsync(AppSettingsHelper.AppSettings.UserSettings.SystemUserId, CurrentPageNo, PageSize);
                if (result != null && !result.IsSuccess)
                {
                    if (!string.IsNullOrEmpty(result.Message))
                    {
                        throw new Exception(result.Message);
                    }
                    else
                    {
                        throw new Exception(SilupostMessage.APP_ERROR);
                    }
                }
                if (result != null && result.Data.Count == 0)
                {
                    this.HasError = false;
                    this.NoRecordFound = false;
                    IsProcessingRefresh = false;
                    return;
                }

                foreach (var report in result.Data)
                {
                    if(!CrimeIncidentReport.Any(x=>x.CrimeIncidentReportId == report.CrimeIncidentReportId))
                    {
                        report.CrimeIncidentCategory.CrimeIncidentType.IconFile.ImageSource = ImageSource.FromStream(() => { return new MemoryStream(report.CrimeIncidentCategory.CrimeIncidentType.IconFile.FileContent); });
                        CrimeIncidentReport.Add(report);
                    }
                    else
                    {
                        int i = CrimeIncidentReport.ToList().FindIndex(x => x.CrimeIncidentReportId == report.CrimeIncidentReportId);
                        CrimeIncidentReport[i] = report;
                        report.CrimeIncidentCategory.CrimeIncidentType.IconFile.ImageSource = ImageSource.FromStream(() => { return new MemoryStream(report.CrimeIncidentCategory.CrimeIncidentType.IconFile.FileContent); });
                    }
                }

                GroupedCrimeIncidentReport = new ObservableCollection<GroupingModel<DateTime, CrimeIncidentReportModel>>();
                var sorted = from report in CrimeIncidentReport
                             orderby report.DateReported descending
                             group report by report.DateReported.ToString("MMMM/yyyy") into reportGroup
                             select new GroupingModel<DateTime, CrimeIncidentReportModel>(DateTime.Parse(reportGroup.Key), reportGroup);

                //create a new collection of groups
                GroupedCrimeIncidentReport = new ObservableCollection<GroupingModel<DateTime, CrimeIncidentReportModel>>(sorted);
                this.ShowCrimeReportList = this.GroupedCrimeIncidentReport.Count > 0;
                this.IsProcessingRefresh = false;
                this.HasError = false;
            }
            catch (Exception ex)
            {
                CrimeIncidentReport = new ObservableCollection<CrimeIncidentReportModel>();
                GroupedCrimeIncidentReport = new ObservableCollection<GroupingModel<DateTime, CrimeIncidentReportModel>>();

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
        }
    }
}

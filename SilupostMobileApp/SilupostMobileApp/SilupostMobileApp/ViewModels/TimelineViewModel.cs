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

                CrimeIncidentReport = new ObservableCollection<CrimeIncidentReportModel>();
                var result = await GetPageReportBySystemUserIdAsync();

                foreach (var report in result)
                {
                    report.PostedBySystemUser.ProfilePicture.ImageSource = ImageSource.FromStream(() => { return new MemoryStream(report.PostedBySystemUser.ProfilePicture.FileContent); });
                    CrimeIncidentReport.Add(report);
                }

                GroupedCrimeIncidentReport = new ObservableCollection<GroupingModel<DateTime, CrimeIncidentReportModel>>();
                var sorted = from report in CrimeIncidentReport
                             orderby report.DateReported descending
                             group report by report.DateReported.ToString("MMMM/yyyy") into reportGroup
                             select new GroupingModel<DateTime, CrimeIncidentReportModel>(DateTime.Parse(reportGroup.Key), reportGroup);

                //create a new collection of groups
                GroupedCrimeIncidentReport = new ObservableCollection<GroupingModel<DateTime, CrimeIncidentReportModel>>(sorted);
                IsProcessingRefresh = false;
            }
            catch (Exception ex)
            {
                CrossToastPopUp.Current.ShowToastMessage(ex.Message);
            }
        }
        public async Task LoadMore()
        {
            try
            {
                IsProcessingRefresh = true;
                this.CurrentPageNo++;
                this.PageSize = 5;
                var result = await GetPageReportBySystemUserIdAsync();

                foreach (var report in result)
                {
                    if(!CrimeIncidentReport.Any(x=>x.CrimeIncidentReportId == report.CrimeIncidentReportId))
                    {
                        report.PostedBySystemUser.ProfilePicture.ImageSource = ImageSource.FromStream(() => { return new MemoryStream(report.PostedBySystemUser.ProfilePicture.FileContent); });
                        CrimeIncidentReport.Add(report);
                    }
                }

                GroupedCrimeIncidentReport = new ObservableCollection<GroupingModel<DateTime, CrimeIncidentReportModel>>();
                var sorted = from report in CrimeIncidentReport
                             orderby report.DateReported descending
                             group report by report.DateReported.ToString("MMMM/yyyy") into reportGroup
                             select new GroupingModel<DateTime, CrimeIncidentReportModel>(DateTime.Parse(reportGroup.Key), reportGroup);

                //create a new collection of groups
                GroupedCrimeIncidentReport = new ObservableCollection<GroupingModel<DateTime, CrimeIncidentReportModel>>(sorted);
                IsProcessingRefresh = false;
            }
            catch (Exception ex)
            {
                CrossToastPopUp.Current.ShowToastMessage(ex.Message);
            }
        }
        public async Task<CrimeIncidentReportModel> InitViewCrimeIncidentReport(string CrimeIncidentReportId)
        {
            return await GetCrimeIncidentReportByIdAsync(CrimeIncidentReportId);
        }

        public async Task<List<CrimeIncidentReportModel>> GetPageReportBySystemUserIdAsync()
        {
            try
            {
                List<CrimeIncidentReportModel> result = await CrimeIncidentReportService.GetPageReportBySystemUserIdAsync(AppSettingsHelper.AppSettings.UserSettings.SystemUserId, CurrentPageNo, PageSize);
                return result;
            }
            catch(SilupostServiceException ex)
            {
                if(ex.Type == SilupostServiceExceptionTypeEnums.NOT_FOUND)
                {
                    this.NoRecordFound = true;
                }
                throw new Exception(ex.ExceptionMessage);
            }
        }

        public async Task<CrimeIncidentReportModel> GetCrimeIncidentReportByIdAsync(string id)
        {
            try
            {
                CrimeIncidentReportModel result = await CrimeIncidentReportService.GetAsync(id);
                return result;
            }
            catch (SilupostServiceException ex)
            {
                if (ex.Type == SilupostServiceExceptionTypeEnums.NOT_FOUND)
                {
                    this.NoRecordFound = true;
                }
                throw new Exception(ex.ExceptionMessage);
            }
        }
    }
}

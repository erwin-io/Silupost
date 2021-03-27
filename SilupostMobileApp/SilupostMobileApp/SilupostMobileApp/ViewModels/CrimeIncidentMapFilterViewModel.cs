using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;
using SilupostMobileApp.Models;
using SilupostMobileApp.Common;

namespace SilupostMobileApp.ViewModels
{
    public class CrimeIncidentMapFilterViewModel : BaseViewModel
    {
        #region MODEL PROPERTIES
        ObservableCollection<CrimeIncidentCategoryModel> _crimeIncidentCategorySource;
        public ObservableCollection<CrimeIncidentCategoryModel> CrimeIncidentCategorySource
        {
            get => _crimeIncidentCategorySource;
            set => SetProperty(ref _crimeIncidentCategorySource, value);
        }

        double _trackerRadiusInKM;
        public double TrackerRadiusInKM
        {
            get => _trackerRadiusInKM;
            set => SetProperty(ref _trackerRadiusInKM, value);
        }
        DateTime _dateReportedFrom;
        public DateTime DateReportedFrom
        {
            get => _dateReportedFrom;
            set => SetProperty(ref _dateReportedFrom, value);
        }
        DateTime _dateReportedTo;
        public DateTime DateReportedTo
        {
            get => _dateReportedTo;
            set => SetProperty(ref _dateReportedTo, value);
        }
        DateTime _possibleDateFrom;
        public DateTime PossibleDateFrom
        {
            get => _possibleDateFrom;
            set => SetProperty(ref _possibleDateFrom, value);
        }
        DateTime _possibleDateTo;
        public DateTime PossibleDateTo
        {
            get => _possibleDateTo;
            set => SetProperty(ref _possibleDateTo, value);
        }
        TimeSpan _possibleTimeFrom;
        public TimeSpan PossibleTimeFrom
        {
            get => _possibleTimeFrom;
            set => SetProperty(ref _possibleTimeFrom, value);
        }
        TimeSpan _possibleTimeTo;
        public TimeSpan PossibleTimeTo
        {
            get => _possibleTimeTo;
            set => SetProperty(ref _possibleTimeTo, value);
        }

        public CrimeIncidentMapFilterModel CrimeIncidentMapFilter { get; set; }
        #endregion

        #region UI PROPERTIES
        public INavigation Navigation { get; set; }

        private bool _isProcessingRefresh;
        public bool IsProcessingRefresh
        {
            get => _isProcessingRefresh;
            set => SetProperty(ref _isProcessingRefresh, value);
        }
        #endregion

        #region COMMANDS
        #endregion

        public CrimeIncidentMapFilterViewModel(INavigation pNavigation)
        {
            this.Navigation = pNavigation;
        }

        public async Task InitSelectedCrimeIncidentCategory()
        {
            if (this.CrimeIncidentMapFilter.SelectedCrimeIncidentCategory == null)
                this.CrimeIncidentMapFilter.SelectedCrimeIncidentCategory = new List<CrimeIncidentCategoryModel>();
            this.CrimeIncidentCategorySource = new ObservableCollection<CrimeIncidentCategoryModel>();
            foreach (var item in this.CrimeIncidentMapFilter.SelectedCrimeIncidentCategory)
            {
                CrimeIncidentCategorySource.Add(item);
            }
        }

        public async Task InitFilter()
        {
            await InitSelectedCrimeIncidentCategory();
            this.TrackerRadiusInKM = CrimeIncidentMapFilter.TrackerRadiusInKM;
            this.DateReportedFrom = CrimeIncidentMapFilter.DateReportedFrom;
            this.DateReportedTo = CrimeIncidentMapFilter.DateReportedTo;
            this.PossibleDateFrom = CrimeIncidentMapFilter.PossibleDateFrom;
            this.PossibleDateTo = CrimeIncidentMapFilter.PossibleDateTo;
            this.PossibleTimeFrom = DateTime.Parse(string.Format("{0} {1}", CrimeIncidentMapFilter.PossibleDateFrom.ToString("MM/dd/yyyy"), CrimeIncidentMapFilter.PossibleTimeFrom)).TimeOfDay;
            this.PossibleTimeTo = DateTime.Parse(string.Format("{0} {1}", CrimeIncidentMapFilter.PossibleDateTo.ToString("MM/dd/yyyy"), CrimeIncidentMapFilter.PossibleTimeTo)).TimeOfDay;
        }

        public async Task RemoveSelectedCrimeIncidentCategoryItem(string Id)
        {
            if (this.CrimeIncidentMapFilter.SelectedCrimeIncidentCategory.Count > 0)
            {
                var itemSelected = this.CrimeIncidentMapFilter.SelectedCrimeIncidentCategory;
                for (int i = 0; i < itemSelected.Count; i++)
                {
                    if (itemSelected[i].CrimeIncidentCategoryId == Id)
                    {
                        this.CrimeIncidentMapFilter.SelectedCrimeIncidentCategory.RemoveAt(i);
                        break;
                    }
                }
            }
            await InitSelectedCrimeIncidentCategory();
        }
    }
}

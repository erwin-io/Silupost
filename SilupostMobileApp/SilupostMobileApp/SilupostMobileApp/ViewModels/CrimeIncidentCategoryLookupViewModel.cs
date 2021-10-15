using Plugin.Toast;
using SilupostMobileApp.Common;
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

namespace SilupostMobileApp.ViewModels
{
    public class CrimeIncidentCategoryLookupViewModel : BaseViewModel
    {
        #region MODEL PROPERTIES
        ObservableCollection<CrimeIncidentCategoryModel> _crimeIncidentCategory;
        public ObservableCollection<CrimeIncidentCategoryModel> CrimeIncidentCategory
        {
            get => _crimeIncidentCategory;
            set => SetProperty(ref _crimeIncidentCategory, value);
        }

        ObservableCollection<GroupingModel<CrimeIncidentTypeModel, CrimeIncidentCategoryModel>> _groupedCrimeIncidentCategory;
        public ObservableCollection<GroupingModel<CrimeIncidentTypeModel, CrimeIncidentCategoryModel>> GroupedCrimeIncidentCategory
        {
            get => _groupedCrimeIncidentCategory;
            set => SetProperty(ref _groupedCrimeIncidentCategory, value);
        }

        public List<CrimeIncidentCategoryModel> SelectedCrimeIncidentCategory { get; set; }
        #endregion

        #region UI PROPERTIES
        public INavigation Navigation { get; set; }

        private bool _isProcessingRefresh;
        public bool IsProcessingRefresh
        {
            get => _isProcessingRefresh;
            set => SetProperty(ref _isProcessingRefresh, value);
        }

        private bool _isSelectMultiple;
        public bool IsSelectMultiple
        {
            get => _isSelectMultiple;
            set => SetProperty(ref _isSelectMultiple, value);
        }


        #endregion

        #region COMMANDS
        public Command RefreshCommand { get; set; }
        #endregion

        public CrimeIncidentCategoryLookupViewModel(INavigation pNavigation)
        {
            this.Navigation = pNavigation;
            RefreshCommand = new Command(async () => await Init());
        }

        public async Task Init()
        {
            try
            {
                IsProcessingRefresh = true;
                if (this.SelectedCrimeIncidentCategory == null)
                    this.SelectedCrimeIncidentCategory = new List<CrimeIncidentCategoryModel>();
                CrimeIncidentCategory = new ObservableCollection<CrimeIncidentCategoryModel>();
                var result = await CrimeIncidentCategoryService.GetAllAsync();

                foreach (var category in result)
                {
                    if (this.IsSelectMultiple && this.SelectedCrimeIncidentCategory.Any(x=>x.CrimeIncidentCategoryId == category.CrimeIncidentCategoryId))
                    {
                        category.IsSelected = true;
                    }
                    CrimeIncidentCategory.Add(category);
                }

                GroupedCrimeIncidentCategory = new ObservableCollection<GroupingModel<CrimeIncidentTypeModel, CrimeIncidentCategoryModel>>();
                var sorted = from category in CrimeIncidentCategory
                             orderby category.CrimeIncidentCategoryId descending
                             group category by category.CrimeIncidentType.CrimeIncidentTypeId into categoryGroup
                             select new GroupingModel<CrimeIncidentTypeModel, CrimeIncidentCategoryModel>(categoryGroup.Where(x=>x.CrimeIncidentType.CrimeIncidentTypeId == categoryGroup.Key).FirstOrDefault().CrimeIncidentType, categoryGroup);

                //create a new collection of groups
                GroupedCrimeIncidentCategory = new ObservableCollection<GroupingModel<CrimeIncidentTypeModel, CrimeIncidentCategoryModel>>(sorted);
                foreach (var type in GroupedCrimeIncidentCategory)
                {
                    type.Key.IconFile.ImageSource = ImageSource.FromStream(() => { return new MemoryStream(type.Key.IconFile.FileContent); });
                }
                IsProcessingRefresh = false;
            }
            catch (Exception ex)
            {
                SilupostPopMessage.ShowToastMessage(ex.Message);
            }
        }

    }
}

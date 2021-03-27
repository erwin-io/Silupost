using Acr.UserDialogs;
using Plugin.Toast;
using SilupostMobileApp.Common;
using SilupostMobileApp.Models;
using SilupostMobileApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SilupostMobileApp.Views.Common
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CrimeIncidentMapFilterPage : ContentPage
    {
        CrimeIncidentMapFilterViewModel viewModel;
        public CrimeIncidentMapFilterPage(CrimeIncidentMapFilterViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = this.viewModel = viewModel;
            this.viewModel.Title = "Filter Map";
            MessagingCenter.Subscribe<CrimeIncidentCategoryLookupLookupPage, List<CrimeIncidentCategoryModel>>(this, "SelectCategoryMultiple", async (obj, selectedCrimeIncidentCategoryModel) =>
            {
                this.viewModel.CrimeIncidentMapFilter.SelectedCrimeIncidentCategory = selectedCrimeIncidentCategoryModel;
                await this.viewModel.InitSelectedCrimeIncidentCategory();
            });
        }

        async void ContentPage_Appearing(object sender, EventArgs e)
        {
            try
            {
                await this.viewModel.InitFilter();
                this.viewModel.ProgressDialog.Hide();
            }
            catch (Exception ex)
            {
                SilupostExceptionLogger.GetError(ex, SilupostMessage.APP_ERROR + string.Format(" {0}", ex.Message));
            }
        }

        async void SelectCategory_Clicked(object sender, EventArgs e)
        {
            try
            {
                if (this.viewModel.IsExecuting)
                    return;
                this.viewModel.IsExecuting = true;
                var _viewModel = new CrimeIncidentCategoryLookupViewModel(this.Navigation);
                _viewModel.IsSelectMultiple = true;
                _viewModel.SelectedCrimeIncidentCategory = this.viewModel.CrimeIncidentMapFilter.SelectedCrimeIncidentCategory;
                _viewModel.ProgressDialog = UserDialogs.Instance.Loading("Loading...", null, "OK", true, MaskType.Gradient);
                await this.Navigation.PushAsync(new CrimeIncidentCategoryLookupLookupPage(_viewModel), true);
                this.viewModel.IsExecuting = false;
            }
            catch(Exception ex)
            {
                CrossToastPopUp.Current.ShowToastMessage(SilupostMessage.APP_ERROR + string.Format(" {0}", ex.Message));
            }
        }

        async void OnCrimeIncidentCategoryItemSelected(object sender, EventArgs e)
        {
            try
            {
                if (viewModel.IsExecuting)
                    return;
                viewModel.IsExecuting = true;
                var layout = (BindableObject)sender;
                var category = (CrimeIncidentCategoryModel)layout.BindingContext;
                await this.viewModel.RemoveSelectedCrimeIncidentCategoryItem(category.CrimeIncidentCategoryId);
                this.viewModel.IsExecuting = false;
            }
            catch (Exception ex)
            {
                this.viewModel.IsExecuting = false;
                SilupostExceptionLogger.GetError(ex, SilupostMessage.APP_ERROR + string.Format(" {0}", ex.Message));
            }
        }
        async void Slider_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            var silder = (Slider)sender;
            var newStep = Math.Round(e.NewValue / 1);
            silder.Value = newStep * 1;
            this.viewModel.TrackerRadiusInKM = silder.Value;
            RadiusLabeValue.FormattedText = string.Format("{0} Km", silder.Value.ToString());
            await RadiusLabeValue.TranslateTo(silder.Value * ((silder.Width - 40) / silder.Maximum), 0, 1);
        }

        async void ButtonApply_Clicked(object sender, EventArgs e)
        {
            if (viewModel.IsExecuting)
                return;
            viewModel.IsExecuting = true;
            this.viewModel.CrimeIncidentMapFilter = new CrimeIncidentMapFilterModel()
            {
                SelectedCrimeIncidentCategory = this.viewModel.CrimeIncidentCategorySource.ToList(),
                TrackerRadiusInKM = this.viewModel.TrackerRadiusInKM,
                DateReportedFrom = this.viewModel.DateReportedFrom,
                DateReportedTo = this.viewModel.DateReportedTo,
                PossibleDateFrom = this.viewModel.PossibleDateFrom,
                PossibleDateTo = this.viewModel.PossibleDateTo,
                PossibleTimeFrom = this.viewModel.PossibleTimeFrom.ToString().Remove(5),
                PossibleTimeTo = this.viewModel.PossibleTimeTo.ToString().Remove(5),

            };
            MessagingCenter.Send(this, "ApplyMapFilter", this.viewModel.CrimeIncidentMapFilter);
            await this.Navigation.PopModalAsync();
            this.viewModel.IsExecuting = false;
        }

        async void TitleBackButton_Clicked(object sender, EventArgs e)
        {
            try
            {
                if (this.viewModel.IsExecuting)
                    return;
                this.viewModel.IsExecuting = true;
                await this.Navigation.PopModalAsync();
                this.viewModel.IsExecuting = false;
            }
            catch (Exception ex)
            {
                this.viewModel.IsExecuting = false;
                SilupostExceptionLogger.GetError(ex, SilupostMessage.APP_ERROR + string.Format(" {0}", ex.Message));
            }
        }
    }
}
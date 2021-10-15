using Plugin.Toast;
using SilupostMobileApp.Common;
using SilupostMobileApp.Models;
using SilupostMobileApp.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SilupostMobileApp.Views.Common
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CrimeIncidentCategoryLookupLookupPage : ContentPage
    {
        CrimeIncidentCategoryLookupViewModel viewModel;
        public CrimeIncidentCategoryLookupLookupPage(CrimeIncidentCategoryLookupViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = this.viewModel = viewModel;
            InitToolbar();
            this.viewModel.Title = "Select Crime/Incident Category";
            this.viewModel.ProgressDialog.Hide();
        }

        public void InitToolbar()
        {
            if (!this.viewModel.IsSelectMultiple && this.ToolbarItems.Count > 0)
            {
                //var toolbarItemsCount = this.ToolbarItems.Count;
                //for (int i = 0; i < toolbarItemsCount; i++)
                //{
                //    this.ToolbarItems.Clear();
                //}
                this.ToolbarItems.Clear();
            }
        }

        async void OnItemSelected(object sender, EventArgs e)
        {
            try
            {
                if (!this.viewModel.IsSelectMultiple)
                {
                    if (viewModel.IsExecuting)
                        return;
                    viewModel.IsExecuting = true;
                    var layout = (BindableObject)sender;
                    var category = (CrimeIncidentCategoryModel)layout.BindingContext;
                    MessagingCenter.Send(this, "SelectCategory", category);
                    await this.Navigation.PopAsync();
                    this.viewModel.IsExecuting = false;
                }
            }
            catch(Exception ex)
            {
                this.viewModel.IsExecuting = false;
                SilupostPopMessage.ShowToastMessage(SilupostMessage.APP_ERROR + string.Format(" {0}", ex.Message));
            }
        }

        async void ContentPage_Appearing(object sender, EventArgs e)
        {
            await this.viewModel.Init();
        }

        async void OnCheckBoxCheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            try
            {
                if (viewModel.IsExecuting)
                    return;
                var cb = (CheckBox)sender;
                var item = (CrimeIncidentCategoryModel)cb.BindingContext;
                var id = item.CrimeIncidentCategoryId;
                if (this.viewModel.SelectedCrimeIncidentCategory.Count > 0)
                {
                    if (this.viewModel.SelectedCrimeIncidentCategory.Any(c => c.CrimeIncidentCategoryId == item.CrimeIncidentCategoryId))
                    {
                        if (!item.IsSelected)
                        {
                            var itemToRemove = this.viewModel.SelectedCrimeIncidentCategory.Single(r => r.CrimeIncidentCategoryId == item.CrimeIncidentCategoryId);
                            this.viewModel.SelectedCrimeIncidentCategory.Remove(itemToRemove);
                        }
                    }
                    else
                    {
                        this.viewModel.SelectedCrimeIncidentCategory.Add(item);
                    }
                }
                else if (cb.IsChecked)
                {
                    this.viewModel.SelectedCrimeIncidentCategory.Add(item);
                }
            }
            catch(Exception ex)
            {
                SilupostPopMessage.ShowToastMessage(SilupostMessage.APP_ERROR + string.Format(" {0}", ex.Message));
            }
        }

        async void UnSelectAll_Clicked(object sender, EventArgs e)
        {
            this.viewModel.SelectedCrimeIncidentCategory = new List<CrimeIncidentCategoryModel>();
            await this.viewModel.Init();
        }

        async void Ok_Clicked(object sender, EventArgs e)
        {
            if (viewModel.IsExecuting)
                return;
            viewModel.IsExecuting = true;
            MessagingCenter.Send(this, "SelectCategoryMultiple", this.viewModel.SelectedCrimeIncidentCategory);
            await this.Navigation.PopAsync();
            this.viewModel.IsExecuting = false;
        }
    }
}
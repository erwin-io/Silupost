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
using SilupostMobileApp.Views.CrimeIncident;
using SilupostMobileApp.Views.Account;
using System.ComponentModel;
using Acr.UserDialogs;

namespace SilupostMobileApp.Views.Timeline
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TimelinePage : ContentPage
    {
        TimelineViewModel viewModel;

        public TimelinePage()
        {
            InitializeComponent();
            BindingContext = viewModel = new TimelineViewModel(this.Navigation);
            viewModel.Title = SilupostPageTitle.TIMELINE;
            InitReportList();
        }

        public async Task InitReportList()
        {
            await this.viewModel.WaitAndExecute(1000, async () =>
            {
                await viewModel.InitSystemUserTimeline();
            });
        }

        async void ContentPage_Appearing(object sender, EventArgs e)
        {
            MessagingCenter.Subscribe<NewCrimeIncidentReportViewModel>(this, "ReloadReportList", async (obj) =>
            {
                try
                {
                    await viewModel.InitSystemUserTimeline();
                }
                catch (Exception ex)
                {
                    SilupostExceptionLogger.GetError(ex);
                }
            });
            MessagingCenter.Subscribe<ViewCrimeIncidentReportViewModel>(this, "ReloadReportList", async (obj) =>
            {
                try
                {
                    await viewModel.InitSystemUserTimeline();
                }
                catch (Exception ex)
                {
                    SilupostExceptionLogger.GetError(ex);
                }
            });
            MessagingCenter.Subscribe<ViewCrimeIncidentReportPage>(this, "ReloadReportList", async (obj) =>
            {
                try
                {
                    await viewModel.InitSystemUserTimeline();
                }
                catch (Exception ex)
                {
                    SilupostExceptionLogger.GetError(ex);
                }
            });

            this.viewModel.ImageSource = string.Format("{0}File/getFile?FileId={1}", AppSettingsHelper.goSILUPOST_WEBAPI_URI, AppSettingsHelper.AppSettings.UserSettings.ProfilePictureFileId ?? string.Empty);
        }

        async void LoadMore_Clicked(object sender, EventArgs e)
        {
            await viewModel.LoadMore();
        }

        async void OnItemSelected(object sender, EventArgs args)
        {
            try
            {
                if (this.viewModel.IsExecuting)
                    return;
                this.viewModel.IsExecuting = true;
                var layout = (BindableObject)sender;
                var report = (CrimeIncidentReportModel)layout.BindingContext;
                var _viewModel = new ViewCrimeIncidentReportViewModel(this.Navigation, report.CrimeIncidentReportId);
                _viewModel.ProgressDialog = UserDialogs.Instance.Loading("Loading...", null, "OK", true, MaskType.Gradient);
                await Navigation.PushModalAsync(new NavigationPage(new ViewCrimeIncidentReportPage(_viewModel)), true);
                this.viewModel.IsExecuting = false;
            }
            catch(Exception ex)
            {
                this.viewModel.IsExecuting = false;
                SilupostExceptionLogger.GetError(ex);
            }
        }

        async void Add(object sender, EventArgs args)
        {
            try
            {
                if (this.viewModel.IsExecuting)
                    return;
                this.viewModel.IsExecuting = true;
                var _viewModel = new NewCrimeIncidentReportViewModel(this.Navigation);
                _viewModel.ProgressDialog = UserDialogs.Instance.Loading("Loading...", null, "OK", true, MaskType.Gradient);
                await Navigation.PushModalAsync(new NavigationPage(new NewCrimeIncidentReportPage(_viewModel)), true);
                this.viewModel.IsExecuting = false;
                _viewModel.ProgressDialog.Hide();
            }
            catch(Exception ex)
            {
                this.viewModel.IsExecuting = false;
                SilupostExceptionLogger.GetError(ex);
            }
        }
        protected override bool OnBackButtonPressed()
        {
            Device.BeginInvokeOnMainThread(async () => {
                var result = await Application.Current.MainPage.DisplayAlert("Exit!", "Do you want to continue?", "Yes", "No");
                if (result)
                {
                    this.viewModel.ApplicationActivity?.CloseApplication();
                }
            });
            return true;
        }

        async void UserProfile_Clicked(object sender, EventArgs e)
        {
            try
            {
                if (this.viewModel.IsExecuting)
                    return;
                this.viewModel.IsExecuting = true;
                var _viewModel = new UserProfileViewModel(this.Navigation);
                _viewModel.ProgressDialog = UserDialogs.Instance.Loading("Loading...", null, "OK", true, MaskType.Gradient);
                await Navigation.PushModalAsync(new NavigationPage(new UserProfilePage(_viewModel)), true);
                this.viewModel.IsExecuting = false;
                _viewModel.ProgressDialog.Hide();
            }
            catch(Exception ex)
            {
                SilupostExceptionLogger.GetError(ex);
            }
        }
    }
}
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
using System.IO;
using System.Collections.ObjectModel;

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
            this.viewModel.ImageSource = ImageSource.FromStream(() => { return new MemoryStream(AppSettingsHelper.AppSettings.UserSettings.FileContent); });

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
                    if(!this.viewModel.IsProcessingRefresh)
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
            MessagingCenter.Subscribe<UserProfilePage>(this, "ReloadProfile", async (obj) =>
            {
                try
                {
                    if (!AppSettingsHelper.CanAccessInternet())
                    {
                        MessagingCenter.Send<System.Object>(new System.Object(), "ApplicationIsOffline");
                        throw new Exception(SilupostMessage.NO_INTERNET);
                    }
                    this.viewModel.ImageSource = ImageSource.FromStream(() => { return new MemoryStream(AppSettingsHelper.AppSettings.UserSettings.FileContent); });
                }
                catch (Exception ex)
                {
                    SilupostExceptionLogger.GetError(ex);
                }
            });
            MessagingCenter.Subscribe<Object>(this, "ApplicationIsOffline", async (obj) =>
            {
                try
                {
                    if (this.viewModel.HasError && !string.IsNullOrEmpty(this.viewModel.ErrorMessage) && this.viewModel.ErrorMessage.Equals(SilupostMessage.NO_INTERNET))
                        return;
                    this.viewModel.ProgressDialog = UserDialogs.Instance.Loading("Loading...", null, "OK", true, MaskType.Gradient);

                    await this.viewModel.WaitAndExecute(1000, async () =>
                    {
                        this.viewModel.CrimeIncidentReport = new ObservableCollection<CrimeIncidentReportModel>();
                        this.viewModel.GroupedCrimeIncidentReport = new ObservableCollection<GroupingModel<DateTime, CrimeIncidentReportModel>>();

                        this.viewModel.HasError = true;
                        this.viewModel.NoRecordFound = true;
                        this.viewModel.IsExecuting = false;
                        this.viewModel.IsBusy = false;
                        this.viewModel.ErrorMessage = string.Format("{0}", SilupostMessage.NO_INTERNET);
                        this.viewModel.ErrorImageSource = "icons8_without_internet_96.png";
                        if (this.viewModel.ProgressDialog != null)
                            this.viewModel.ProgressDialog.Hide();
                    });
                }
                catch(Exception ex)
                {
                    if(this.viewModel.ProgressDialog != null)
                        this.viewModel.ProgressDialog.Hide();
                }
            });
            MessagingCenter.Subscribe<Object>(this, "ApplicationIsOnline", async (obj) =>
            {
                try
                {
                    if (this.viewModel.HasError && !string.IsNullOrEmpty(this.viewModel.ErrorMessage) && this.viewModel.ErrorMessage.Equals(SilupostMessage.NO_INTERNET))
                    {
                        await viewModel.InitSystemUserTimeline();
                    }
                }
                catch (Exception ex)
                {
                    if (this.viewModel.ProgressDialog != null)
                        this.viewModel.ProgressDialog.Hide();
                }
            });
        }

        async void LoadMore_Clicked(object sender, EventArgs e)
        {
            try
            {
                if (!AppSettingsHelper.CanAccessInternet())
                {
                    MessagingCenter.Send<System.Object>(new System.Object(), "ApplicationIsOffline");
                    throw new Exception(SilupostMessage.NO_INTERNET);
                }
                await viewModel.LoadMore();
            }
            catch (Exception ex)
            {
                SilupostExceptionLogger.GetError(ex);
            }
        }

        async void OnItemSelected(object sender, EventArgs args)
        {
            try
            {
                if (!AppSettingsHelper.CanAccessInternet())
                {
                    MessagingCenter.Send<System.Object>(new System.Object(), "ApplicationIsOffline");
                    throw new Exception(SilupostMessage.NO_INTERNET);
                }
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
                if (!AppSettingsHelper.CanAccessInternet())
                {
                    MessagingCenter.Send<System.Object>(new System.Object(), "ApplicationIsOffline");
                    throw new Exception(SilupostMessage.NO_INTERNET);
                }
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
                if (!AppSettingsHelper.CanAccessInternet())
                {
                    MessagingCenter.Send<System.Object>(new System.Object(), "ApplicationIsOffline");
                    throw new Exception(SilupostMessage.NO_INTERNET);
                }
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

        async void Retry_Clicked(object sender, EventArgs e)
        {
            try
            {
                if (!AppSettingsHelper.CanAccessInternet())
                {
                    MessagingCenter.Send<System.Object>(new System.Object(), "ApplicationIsOffline");
                    throw new Exception(SilupostMessage.NO_INTERNET);
                }
                await viewModel.InitSystemUserTimeline();
            }
            catch (Exception ex)
            {
                SilupostExceptionLogger.GetError(ex);
            }
        }
    }
}
using Plugin.Messaging;
using SilupostMobileApp.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using SilupostMobileApp.CustomRender;
using SilupostMobileApp.Common;
using Acr.UserDialogs;
using SilupostMobileApp.Views.Account;
using System.IO;

namespace SilupostMobileApp.Views.Emergency
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EmergencyCallPage : ContentPage
    {
        EmergencyCallViewModel viewModel;
        public EmergencyCallPage()
        {
            InitializeComponent();
            BindingContext = viewModel = new EmergencyCallViewModel(this.Navigation);
            viewModel.Title = SilupostPageTitle.EMERGENCY_CALL;

            MessagingCenter.Subscribe<UserProfilePage>(this, "ReloadProfile", async (obj) =>
            {
                try
                {
                    this.viewModel.ImageSource = ImageSource.FromStream(() => { return new MemoryStream(AppSettingsHelper.AppSettings.UserSettings.FileContent); });
                }
                catch (Exception ex)
                {
                    SilupostExceptionLogger.GetError(ex);
                }
            });

            try
            {
                this.viewModel.ImageSource = ImageSource.FromStream(() => { return new MemoryStream(AppSettingsHelper.AppSettings.UserSettings.FileContent); });
            }
            catch (Exception ex)
            {
                SilupostExceptionLogger.GetError(ex);
            }
        }
        async void CallHistory_Clicked(object sender, EventArgs e)
        {
            try
            {
                if (viewModel.IsExecuting)
                    return;
                viewModel.IsExecuting = true;
                await Navigation.PushModalAsync(new NavigationPage(new CallHistoryPage()), true);
                viewModel.IsExecuting = false;
            }
            catch(Exception ex)
            {
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
            catch (Exception ex)
            {
                SilupostExceptionLogger.GetError(ex);
            }
        }

        private void ContentPage_Appearing(object sender, EventArgs e)
        {
            try
            {
            }
            catch(Exception ex)
            {
                SilupostExceptionLogger.GetError(ex);
            }
        }

        async void EmergencyCall_Clicked(object sender, EventArgs e)
        {
            try
            {

                await this.viewModel.EmergencyCall();
            }
            catch (Exception ex)
            {
                SilupostExceptionLogger.GetError(ex);
            }
        }
    }
}
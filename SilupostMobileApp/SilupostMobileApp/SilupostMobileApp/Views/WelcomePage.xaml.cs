using Acr.UserDialogs;
using Plugin.Toast;
using SilupostMobileApp.Common;
using SilupostMobileApp.Models;
using SilupostMobileApp.BindingModels;
using SilupostMobileApp.ViewModels;
using SilupostMobileApp.Views.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using SilupostMobileApp.Views.Account.Register;

namespace SilupostMobileApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class WelcomePage : ContentPage
    {
        WelcomeViewModel viewModel;
        public WelcomePage(WelcomeViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = this.viewModel = viewModel;
            this.viewModel.Title = "Welcome";
            Init();
        }

        public async void Init()
        {
            this.viewModel.ShowTitle = true;
            this.viewModel.ShowAuthControls = true;
            MessagingCenter.Subscribe<RegisterPage, SystemUserModel>(this, "RegisterSuccess", async (obj, model) =>
            {
                try
                {
                    this.viewModel.ShowAuthControls = false;
                    this.viewModel.ShowTitle = true;
                    this.viewModel.ShowSuccessMessage = true;
                }
                catch (Exception ex)
                {
                    SilupostExceptionLogger.GetError(ex);
                }
            });
            MessagingCenter.Subscribe<App>(this, "OpenWelcomePage", async (obj) =>
            {
                try
                {
                    if (AppSettingsHelper.goIsLaunchFromURL && AppSettingsHelper.goLaunchFromURLData != null)
                    {
                        try
                        {
                            if (AppSettingsHelper.goLaunchFromURLData.Type == SilupostLaunchFromURLTypeEnums.EMAIL_CONFIRMATION)
                            {
                                var registerViewModel = new RegisterViewModel(this.Navigation);
                                registerViewModel.ProgressDialog = UserDialogs.Instance.Loading("Loading...", null, "OK", true, MaskType.Gradient);
                                registerViewModel.Email.Value = AppSettingsHelper.goLaunchFromURLData.EmailId;
                                registerViewModel.VerificationCode.Value = AppSettingsHelper.goLaunchFromURLData.Code;
                                await App.Current.MainPage.Navigation.PushModalAsync(new RegisterPage(registerViewModel), true);
                            }
                            else
                            {
                                var changePasswordViewModel = new ChangePasswordViewModel(this.Navigation);
                                changePasswordViewModel.ProgressDialog = UserDialogs.Instance.Loading("Loading...", null, "OK", true, MaskType.Gradient);
                                changePasswordViewModel.Email.Value = AppSettingsHelper.goLaunchFromURLData.EmailId;
                                changePasswordViewModel.VerificationCode = AppSettingsHelper.goLaunchFromURLData.Code;
                                changePasswordViewModel.SystemUserId = AppSettingsHelper.goLaunchFromURLData.SystemUserId;
                                await App.Current.MainPage.Navigation.PushModalAsync(new ChangePasswordPage(changePasswordViewModel), true);
                            }
                        }
                        catch (Exception ex)
                        {
                            SilupostExceptionLogger.GetError(ex);
                            this.viewModel.ProgressDialog.Hide();
                        }
                    }
                }
                catch (Exception ex)
                {
                    SilupostExceptionLogger.GetError(ex);
                    this.viewModel.ProgressDialog.Hide();
                }
            });
        }

        async void ButtonGotoLogin_Clicked(object sender, EventArgs e)
        {
            if (this.viewModel.IsExecuting)
                return;
            this.viewModel.IsExecuting = true;
            var _viewModel = new LoginViewModel(this.Navigation);
            _viewModel.ProgressDialog = UserDialogs.Instance.Loading("Loading...", null, "OK", true, MaskType.Gradient);
            await Navigation.PushModalAsync(new LoginPage(_viewModel), true);
            this.viewModel.IsExecuting = false;
        }

        async void ButtonGotoRegister_Clicked(object sender, EventArgs e)
        {
            try
            {
                var _viewModel = new WelcomeViewModel(this.Navigation);
                _viewModel.ShowAuthControls = false;
                _viewModel.ShowTitle = false;
                _viewModel.ShowSuccessMessage = false;
                App.Current.MainPage = new WelcomePage(_viewModel);
                var registerViewModel = new RegisterViewModel(this.Navigation);
                registerViewModel.ProgressDialog = UserDialogs.Instance.Loading("Loading...", null, "OK", true, MaskType.Gradient);
                await App.Current.MainPage.Navigation.PushModalAsync(new NavigationPage(new RegisterPage(registerViewModel)), true);
            }
            catch (Exception ex)
            {
                SilupostExceptionLogger.GetError(ex);
            }
        }
        async void ButtonOpenModule_Clicked(object sender, EventArgs e)
        {
            App.Current.MainPage = new MainPage();
            MessagingCenter.Send(this, "OpenModule");
        }

        async void ContentPage_Appearing(object sender, EventArgs e)
        {
        }

        async void ContentPage_Disappearing(object sender, EventArgs e)
        {
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
    }
}
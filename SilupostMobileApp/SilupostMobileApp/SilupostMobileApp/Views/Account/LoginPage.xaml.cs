using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using SilupostMobileApp.ViewModels;
using SilupostMobileApp.Common.Interface;
using Plugin.Toast;
using Acr.UserDialogs;
using SilupostMobileApp.Common;
using SilupostMobileApp.Models;
using SilupostMobileApp.Views.Common.Error;

namespace SilupostMobileApp.Views.Account
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : ContentPage
    {
        LoginViewModel viewModel;
        public LoginPage(LoginViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = this.viewModel = viewModel;
            viewModel.Title = "Login";
            this.viewModel.ProgressDialog.Hide();
        }
        protected override bool OnBackButtonPressed()
        {
            Device.BeginInvokeOnMainThread(async () => {
                AppSettingsHelper.goIsLaunchFromURL = false;
                AppSettingsHelper.goLaunchFromURLData = null;
                await this.Navigation.PopModalAsync();
                //MessagingCenter.Send(this, "OpenWelcomePage");
            });
            return true;
        }

        async void ButtonLogin_Clicked(object sender, EventArgs e)
        {

            try
            {
                if (await this.viewModel.AreFieldsValid())
                {
                    this.viewModel.ProgressDialog = UserDialogs.Instance.Loading("Loading...", null, "OK", true, MaskType.Gradient);
                    var result = await this.viewModel.Login();
                    if (result != null)
                    {
                        await AppSettingsHelper.SetAppSetting(new AppSettingsModel()
                        {
                            AppToken = new AppTokenModel()
                            {
                                AccessToken = result.Token.AccessToken,
                                RefreshToken = result.Token.RefreshToken
                            },
                            UserSettings = new AppUserSettingsModel()
                            {
                                SystemUserId = result.SystemUserId,
                                UserName = result.UserName,
                                Password = this.viewModel.Password.Value,
                                FullName = string.Format("{0} {1}", result.LegalEntity.FirstName, result.LegalEntity.LastName),
                                ProfilePictureFileId = result.ProfilePicture.FileId,
                                FileContent = result.ProfilePicture.FileContent,
                            },
                            IsAuthenticated = true
                        });
                        AppSettingsHelper.AppSettings = await AppSettingsHelper.GetAppSetting();
                        App.Current.MainPage = new MainPage();
                        MessagingCenter.Send(this, "AuthenticateUser", result);
                    }
                    else
                    {
                        throw new Exception(string.Format("Username or password is incorrect!"));
                    }
                }
            }
            catch (Exception ex)
            {
                //this.viewModel.ProgressDialog.Hide();
                //SilupostExceptionLogger.GetError(ex);

                this.viewModel.HasError = true;
                this.viewModel.IsExecuting = false;
                this.IsBusy = false;
                if (ex.Message.Contains(SilupostMessage.SERVER_INACTIVE))
                {
                    App.Current.MainPage = new ErrorMainPage(SilupostErrorPageTypeEnums.SERVER_ERROR);
                }
                else if (ex.Message.Contains(SilupostMessage.NO_INTERNET))
                {
                    App.Current.MainPage = new ErrorMainPage(SilupostErrorPageTypeEnums.INTERNET_ERROR);
                }
                else if (ex.Message.ToLower().Contains("unexpected character encountered"))
                {
                    App.Current.MainPage = new ErrorMainPage(SilupostErrorPageTypeEnums.SERVER_ERROR);
                }
                else if (ex.Message.ToLower().Contains("problem occurs while proccessing"))
                {
                    App.Current.MainPage = new ErrorMainPage(SilupostErrorPageTypeEnums.SERVER_ERROR);
                }
                else if (ex.Message.ToLower().Contains("unable to resolve host"))
                {
                    App.Current.MainPage = new ErrorMainPage(SilupostErrorPageTypeEnums.INTERNET_ERROR);
                }
                else if (ex.Message.ToLower().Contains("no address associated with hostname"))
                {
                    App.Current.MainPage = new ErrorMainPage(SilupostErrorPageTypeEnums.INTERNET_ERROR);
                }
                else
                {
                    this.viewModel.ErrorMessage = string.Format("{0}", SilupostMessage.APP_ERROR);
                    this.viewModel.ErrorImageSource = "icons8_error_80.png";
                    this.viewModel.ProgressDialog.Hide();
                    SilupostExceptionLogger.GetError(ex, SilupostMessage.APP_ERROR);
                }
                if (this.viewModel.ProgressDialog != null)
                    this.viewModel.ProgressDialog.Hide();
            }
        }

        async void ForgotPassword_Tapped(object sender, EventArgs e)
        {
            var _viewModel = new WelcomeViewModel(this.Navigation);
            _viewModel.ShowAuthControls = false;
            _viewModel.ShowTitle = false;
            _viewModel.ShowSuccessMessage = false;
            App.Current.MainPage = new WelcomePage(_viewModel);
            var changePasswordViewModel = new ChangePasswordViewModel(this.Navigation);
            changePasswordViewModel.ProgressDialog = UserDialogs.Instance.Loading("Loading...", null, "OK", true, MaskType.Gradient);
            await App.Current.MainPage.Navigation.PushModalAsync(new NavigationPage(new ChangePasswordPage(changePasswordViewModel)), true);
        }

        async void ButtonGotRegister_Tapped(object sender, EventArgs e)
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
        async void ContentPage_Appearing(object sender, EventArgs e)
        {
        }
    }
}
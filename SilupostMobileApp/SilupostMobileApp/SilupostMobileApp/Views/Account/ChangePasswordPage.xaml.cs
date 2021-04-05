using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using SilupostMobileApp.BindingModels;
using SilupostMobileApp.ViewModels;
using SilupostMobileApp.Common;
using SilupostMobileApp.Common.Interface;
using SilupostMobileApp.Views.Account.ChangePassword;
using Plugin.Toast;
using Acr.UserDialogs;

namespace SilupostMobileApp.Views.Account
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ChangePasswordPage : ContentPage
    {
        ChangePasswordViewModel viewModel;
        public ChangePasswordPage(ChangePasswordViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = this.viewModel = viewModel;
            viewModel.Title = "Change Password";
            this.viewModel.ProgressDialog.Hide();
        }

        async void ContentPage_Appearing(object sender, EventArgs e)
        {
            MessagingCenter.Subscribe<EmailRegistrationContentView>(this, "SubmitEmailChangePassword", async (obj) =>
            {
                try
                {
                    this.viewModel.ProgressDialog.Show();
                    await this.viewModel.WaitAndExecute(1000, async () =>
                    {
                        try
                        {
                            var model = new SystemUserVerificationBindingModel()
                            {
                                VerificationSender = this.viewModel.Email.Value,
                                VerificationTypeId = "1"
                            };

                            var result = await this.viewModel.SendEmailChangePassword(model);
                            if (result != null)
                            {
                                this.viewModel.IsEmailSubmitted = true;
                                this.viewModel.ProgressDialog.Hide();
                            }
                            else
                            {
                                this.viewModel.IsExecuting = false;
                                this.viewModel.ProgressDialog.Hide();
                                throw new Exception(SilupostMessage.SERVER_ERROR);
                            }
                        }
                        catch (Exception ex)
                        {
                            this.viewModel.IsExecuting = false;
                            SilupostExceptionLogger.GetError(ex);
                            this.viewModel.ProgressDialog.Hide();
                        }
                    });
                }
                catch (Exception ex)
                {
                    this.viewModel.IsExecuting = false;
                    SilupostExceptionLogger.GetError(ex);
                    this.viewModel.ProgressDialog.Hide();
                }
            });
            MessagingCenter.Subscribe<CredentialsContentView>(this, "ResetPassword", async (obj) =>
            {
                try
                {
                    this.viewModel.ProgressDialog.Show();
                    await this.viewModel.WaitAndExecute(1000, async () =>
                    {
                        try
                        {
                            if (this.viewModel.IsExecuting)
                                return;
                            this.viewModel.IsExecuting = true;
                            var model = new UpdateSystemResetPasswordBindingModel()
                            {
                                SystemUserId = this.viewModel.SystemUserId,
                                NewPassword = this.viewModel.Password.Value
                            };
                            var result = await this.viewModel.ResetPassword(model);
                            if (result != null)
                            {
                                this.viewModel.IsExecuting = false;
                                AppSettingsHelper.goIsLaunchFromURL = false;
                                AppSettingsHelper.goLaunchFromURLData = null;
                                await this.viewModel.ResetEmailRegistration();
                                await this.viewModel.ResetCredentials();
                                this.viewModel.IsResetPasswordSuccess = true;
                                this.viewModel.ProgressDialog.Hide();
                                NavigationPage.SetHasNavigationBar(this, false);
                            }
                            else
                            {
                                this.viewModel.IsExecuting = false;
                                this.viewModel.ProgressDialog.Hide();
                                throw new Exception(SilupostMessage.APP_ERROR);
                            }
                        }
                        catch (Exception ex)
                        {
                            this.viewModel.IsExecuting = false;
                            SilupostExceptionLogger.GetError(ex);
                            this.viewModel.ProgressDialog.Hide();
                        }
                    });
                }
                catch (Exception ex)
                {
                    SilupostExceptionLogger.GetError(ex);
                }
            });
            MessagingCenter.Subscribe<CredentialsContentView>(this, "GotoLogin", async (obj) =>
            {
                try
                {
                    var _viewModel = new WelcomeViewModel(this.Navigation);
                    _viewModel.ShowAuthControls = false;
                    _viewModel.ShowTitle = false;
                    _viewModel.ShowSuccessMessage = false;
                    App.Current.MainPage = new WelcomePage(_viewModel);

                    var loginViewModel = new LoginViewModel(this.Navigation);
                    loginViewModel.ProgressDialog = UserDialogs.Instance.Loading("Loading...", null, "OK", true, MaskType.Gradient);
                    await App.Current.MainPage.Navigation.PushModalAsync(new NavigationPage(new LoginPage(loginViewModel)), true);
                }
                catch (Exception ex)
                {
                    SilupostExceptionLogger.GetError(ex);
                }
            });
            this.viewModel.ProgressDialog.Show();
            await this.viewModel.WaitAndExecute(1000, async () =>
            {
                try
                {
                    if (this.viewModel.IsFromProfile)
                    {
                        NavigationPage.SetHasNavigationBar(this, true);
                        this.Content = new EmailRegistrationContentView(this.viewModel);
                    }
                    else
                    {
                        if (AppSettingsHelper.goIsLaunchFromURL && AppSettingsHelper.goLaunchFromURLData != null)
                        {
                            this.Content = new CredentialsContentView(this.viewModel);
                        }
                        else
                        {
                            this.Content = new EmailRegistrationContentView(this.viewModel);
                        }
                    }
                    this.viewModel.ProgressDialog.Hide();
                }
                catch (Exception ex)
                {
                    SilupostExceptionLogger.GetError(ex);
                    this.viewModel.ProgressDialog.Hide();
                }
            });
        }
        protected override bool OnBackButtonPressed()
        {
            Device.BeginInvokeOnMainThread(async () => {
                if (this.viewModel.IsFromProfile)
                {
                    if (this.viewModel.IsResetPasswordSuccess)
                    {
                        await this.Navigation.PopToRootAsync();
                        MessagingCenter.Send(this, "Logout");
                    }
                    else
                    {
                        await this.Navigation.PopAsync();
                    }
                }
                else
                {
                    var result = await Application.Current.MainPage.DisplayAlert("Exit!", "Do you want to continue?", "Yes", "No");
                    if (result)
                    {
                        AppSettingsHelper.goIsLaunchFromURL = false;
                        AppSettingsHelper.goLaunchFromURLData = null;
                        await this.Navigation.PopModalAsync();
                    }
                }
            });
            return true;
        }
        async void ContentPage_Disappearing(object sender, EventArgs e)
        {
            MessagingCenter.Unsubscribe<EmailRegistrationContentView>(this, "SubmitEmailChangePassword");
            MessagingCenter.Unsubscribe<CredentialsContentView>(this, "ResetPassword");
            MessagingCenter.Unsubscribe<CredentialsContentView>(this, "GotoLogin");
        }
    }
}
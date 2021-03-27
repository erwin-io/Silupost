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
using SilupostMobileApp.Views.Account.Register;
using Plugin.Toast;
using Xamarin.Forms.PlatformConfiguration;
using SilupostMobileApp.Models;

namespace SilupostMobileApp.Views.Account
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RegisterPage : ContentPage
    {
        RegisterViewModel viewModel;
        public RegisterPage(RegisterViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = this.viewModel = viewModel;
            viewModel.Title = "Register";
            this.viewModel.ProgressDialog.Hide();
        }

        async void ContentPage_Appearing(object sender, EventArgs e)
        {
            MessagingCenter.Subscribe<EmailRegistrationContentView>(this, "SubmitEmail", async (obj) =>
            {
                try
                {
                    this.viewModel.ProgressDialog.Show();
                    await this.viewModel.ResetVerification();
                    await this.viewModel.WaitAndExecute(1000, async () =>
                    {
                        try
                        {
                            var model = new SystemUserVerificationBindingModel()
                            {
                                VerificationSender = this.viewModel.Email.Value,
                                VerificationTypeId = "1"
                            };
                            var result = await this.viewModel.SendEmailVerification(model);
                            if (result != null)
                            {
                                this.Content = new VerifyContentView(this.viewModel);
                                this.viewModel.ProgressDialog.Hide();
                            }
                            else
                            {
                                this.viewModel.ProgressDialog.Hide();
                                CrossToastPopUp.Current.ShowToastMessage(SilupostMessage.SERVER_ERROR);
                            }
                        }
                        catch (Exception ex)
                        {
                            SilupostExceptionLogger.GetError(ex);
                            this.viewModel.ProgressDialog.Hide();
                            this.viewModel.IsExecuting = false;
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
            MessagingCenter.Subscribe<VerifyContentView>(this, "ResendEmail", async (obj) =>
            {
                try
                {
                    this.viewModel.ProgressDialog.Show();
                    await this.viewModel.ResetVerification();
                    await this.viewModel.WaitAndExecute(1000, async () =>
                    {
                        try
                        {
                            this.Content = new EmailRegistrationContentView(this.viewModel);
                            this.viewModel.ProgressDialog.Hide();
                        }
                        catch (Exception ex)
                        {
                            SilupostExceptionLogger.GetError(ex);
                            this.viewModel.ProgressDialog.Hide();
                            this.viewModel.IsExecuting = false;
                        }
                    });
                }
                catch (Exception ex)
                {
                    SilupostExceptionLogger.GetError(ex);
                    this.viewModel.ProgressDialog.Hide();
                    this.viewModel.IsExecuting = false;
                }
            });
            MessagingCenter.Subscribe<VerifyContentView>(this, "VerifyCode", async (obj) =>
            {
                try
                {
                    this.viewModel.ProgressDialog.Show();
                    await this.viewModel.ResetCredentials();
                    await this.viewModel.WaitAndExecute(1000, async () =>
                    {
                        try
                        {
                            var result = await this.viewModel.GetBySender(this.viewModel.Email.Value, this.viewModel.VerificationCode.Value);
                            if (result != null)
                            {
                                this.viewModel.VerificationCode.HasCustomError = false;
                                this.viewModel.VerificationCode.Errors = new List<string>();
                                this.Content = new CredentialsContentView(this.viewModel);
                                this.viewModel.VerificationCode.Validate();
                                this.viewModel.ProgressDialog.Hide();
                            }
                            else
                            {
                                this.viewModel.ProgressDialog.Hide();
                                this.viewModel.VerificationCode.HasCustomError = true;
                                CrossToastPopUp.Current.ShowToastMessage("Invalid Verification code!");
                                this.viewModel.VerificationCode.Errors.Add("Invalid Verification code!");
                                this.viewModel.VerificationCode.Validate();
                                this.viewModel.IsExecuting = false;
                            }
                        }
                        catch (Exception ex)
                        {
                            SilupostExceptionLogger.GetError(ex);
                            this.viewModel.ProgressDialog.Hide();
                            this.viewModel.IsExecuting = false;
                        }
                    });
                }
                catch (Exception ex)
                {
                    SilupostExceptionLogger.GetError(ex);
                    this.viewModel.ProgressDialog.Hide();
                    this.viewModel.IsExecuting = false;
                }
            });
            MessagingCenter.Subscribe<CredentialsContentView>(this, "SaveCredentials", async (obj) =>
            {
                try
                {
                    this.viewModel.ProgressDialog.Show();
                    await this.viewModel.ResetUserInfo();
                    this.Content = new UserInfoContentView(this.viewModel);
                    this.viewModel.ProgressDialog.Hide();
                }
                catch (Exception ex)
                {
                    SilupostExceptionLogger.GetError(ex);
                }
            });
            MessagingCenter.Subscribe<UserInfoContentView>(this, "BackToCredentials", async (obj) =>
            {
                try
                {
                    this.viewModel.ProgressDialog.Show();
                    await this.viewModel.ResetCredentials();
                    await this.viewModel.WaitAndExecute(1000, async () =>
                    {
                        try
                        {
                            this.Content = new CredentialsContentView(this.viewModel);
                            this.viewModel.ProgressDialog.Hide();
                        }
                        catch (Exception ex)
                        {
                            SilupostExceptionLogger.GetError(ex);
                            this.viewModel.ProgressDialog.Hide();
                        }
                    });
                }
                catch (Exception ex)
                {
                    SilupostExceptionLogger.GetError(ex);
                    this.viewModel.ProgressDialog.Hide();
                }
            });
            MessagingCenter.Subscribe<UserInfoContentView>(this, "SaveUserInfo", async (obj) =>
            {
                try
                {
                    this.viewModel.ProgressDialog.Show();
                    await this.viewModel.WaitAndExecute(1000, async () =>
                    {
                        try
                        {
                            if (!await this.viewModel.UserInfoValid())
                                return;
                            var model = new CreateAccountSystemUserBindingModel()
                            {
                                VerificationCode = this.viewModel.VerificationCode.Value,
                                EmailAddress = this.viewModel.Email.Value,
                                Password = this.viewModel.Password.Value,
                                FirstName = this.viewModel.FirstName.Value,
                                MiddleName = this.viewModel?.MiddleName?.Value,
                                LastName = this.viewModel.LastName.Value,
                                GenderId = this.viewModel.SelectedGender.Value.GenderId,
                                BirthDate = this.viewModel.BirthDay.Value
                            };
                            var result = await this.viewModel.CreateAccount(model);
                            if (result != null)
                            {
                                AppSettingsHelper.goIsLaunchFromURL = false;
                                AppSettingsHelper.goLaunchFromURLData = null;
                                result = await this.viewModel.Login();
                                await this.viewModel.ResetEmailRegistration();
                                await this.viewModel.ResetVerification();
                                await this.viewModel.ResetCredentials();
                                await this.viewModel.ResetUserInfo();
                                this.viewModel.ProgressDialog.Hide();

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
                                        FullName = string.Format("{0} {1}", result.LegalEntity.FirstName, result.LegalEntity.LastName),
                                        ProfilePictureFileId = result.ProfilePicture.FileId
                                    },
                                    IsAuthenticated = true
                                });
                                AppSettingsHelper.AppSettings = await AppSettingsHelper.GetAppSetting();
                                await this.viewModel.WaitAndExecute(1000, async () =>
                                {
                                    var _viewModel = new WelcomeViewModel(this.Navigation);
                                    _viewModel.ShowAuthControls = false;
                                    _viewModel.ShowTitle = true;
                                    _viewModel.ShowSuccessMessage = true;
                                    App.Current.MainPage = new WelcomePage(_viewModel);
                                    MessagingCenter.Send(this, "RegisterSuccess", result);
                                });
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
                            if(!ex.Message.ToLower().Contains("object"))
                                SilupostExceptionLogger.GetError(ex);
                            this.viewModel.ProgressDialog.Hide();
                        }
                    });
                }
                catch (Exception ex)
                {
                    this.viewModel.IsExecuting = false;
                    this.viewModel.ProgressDialog.Hide();
                    SilupostExceptionLogger.GetError(ex);
                }
            });
            this.viewModel.ProgressDialog.Show();
            await this.viewModel.WaitAndExecute(1000, async () =>
            {
                try
                {
                    if (AppSettingsHelper.goIsLaunchFromURL && AppSettingsHelper.goLaunchFromURLData != null)
                    {
                        this.Content = new CredentialsContentView(this.viewModel);
                    }
                    else
                    {
                        this.Content = new EmailRegistrationContentView(this.viewModel);
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
                var result = await Application.Current.MainPage.DisplayAlert("Exit!", "Do you want to continue?", "Yes", "No");
                if (result)
                {
                    AppSettingsHelper.goIsLaunchFromURL = false;
                    AppSettingsHelper.goLaunchFromURLData = null;
                    await this.Navigation.PopModalAsync();
                }
            });
            return true;
        }
        async void ContentPage_Disappearing(object sender, EventArgs e)
        {
            MessagingCenter.Unsubscribe<EmailRegistrationContentView>(this, "SubmitEmail");
            MessagingCenter.Unsubscribe<VerifyContentView>(this, "ResendEmail");
            MessagingCenter.Unsubscribe<VerifyContentView>(this, "VerifyCode");
            MessagingCenter.Unsubscribe<CredentialsContentView>(this, "SaveCredentials");
            MessagingCenter.Unsubscribe<UserInfoContentView>(this, "BackToCredentials");
            MessagingCenter.Unsubscribe<UserInfoContentView>(this, "SaveUserInfo");
        }
    }
}
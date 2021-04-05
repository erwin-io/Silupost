using System;
using System.Linq;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration;
using Xamarin.Forms.PlatformConfiguration.AndroidSpecific;
using Xamarin.Forms.Xaml;
using SilupostMobileApp.Views.Account;
using SilupostMobileApp.Views.Account.UserProfile;
using SilupostMobileApp.Views.Account.UpdateEmail;
using SilupostMobileApp.Common;
using SilupostMobileApp.ViewModels;
using SilupostMobileApp.Views.Emergency;
using SilupostMobileApp.Views.Timeline;
using SilupostMobileApp.Views.CrimeIncident;
using SilupostMobileApp.CustomRender;
using Plugin.Toast;
using System.Threading.Tasks;
using System.Collections.Generic;
using SilupostMobileApp.Models;
using SilupostMobileApp.Views.Account.Register;
using Acr.UserDialogs;
using SilupostMobileApp.Views.Common.Error;

namespace SilupostMobileApp.Views
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MainPage : Xamarin.Forms.TabbedPage
    {
        MainViewModel viewModel;
        bool IsLaunchFromShortcut = false;
        string Page = null;
        public MainPage(bool IsLaunchFromShortcut = false, string Page = null)
        {
            InitializeComponent();
            BindingContext = this.viewModel = new MainViewModel();
            this.IsLaunchFromShortcut = IsLaunchFromShortcut;
            this.Page = Page;
            On<Android>().SetToolbarPlacement(ToolbarPlacement.Bottom);
            CurrentPageChanged += CurrentPageHasChanged;

            MessagingCenter.Subscribe<NewCrimeIncidentReportViewModel, int>(this, "SelectTab", async (obj, tabIndex) =>
            {
                try
                {
                    var appSettings = await AppSettingsHelper.GetAppSetting();
                    if (appSettings != null && appSettings.IsAuthenticated && Children != null && Children.Count() > 0)
                        CurrentPage = Children[tabIndex];
                }
                catch (Exception ex)
                {
                    SilupostExceptionLogger.GetError(ex);
                }
            });


            MessagingCenter.Subscribe<Object, int>(this, "SelectTab", async (obj, tabIndex) =>
            {
                try
                {
                    var appSettings = await AppSettingsHelper.GetAppSetting();
                    if (appSettings != null && appSettings.IsAuthenticated && Children != null && Children.Count() > 0)
                        CurrentPage = Children[tabIndex];
                }
                catch (Exception ex)
                {
                    SilupostExceptionLogger.GetError(ex);
                }
            });

            MessagingCenter.Subscribe<LoginPage, SystemUserModel>(this, "AuthenticateUser", async (obj, model) =>
            {
                try
                {
                    MessagingCenter.Send<Object>(new Object(), "StartBackgroundService");
                    await InitTab();
                }
                catch (Exception ex)
                {
                    SilupostExceptionLogger.GetError(ex);
                }
            });

            MessagingCenter.Subscribe<WelcomePage, SystemUserModel>(this, "OpenModule", async (obj, model) =>
            {
                try
                {
                    MessagingCenter.Send<Object>(new Object(), "StartBackgroundService");
                    await InitTab();
                }
                catch (Exception ex)
                {
                    SilupostExceptionLogger.GetError(ex);
                }
            });

            MessagingCenter.Subscribe<UserProfileSettingsPage>(this, "Logout", async (obj) =>
            {
                MessagingCenter.Send<Object>(new Object(), "StopBackgroundService");
                await AppSettingsHelper.SetAppSetting(null);
                await this.viewModel.WaitAndExecute(1000, async () =>
                {
                    App.Current.MainPage = new MainPage();
                    await InitUser();
                });
            });

            MessagingCenter.Subscribe<Account.UpdateEmail.EmailRegistrationContentView>(this, "Logout", async (obj) =>
            {
                MessagingCenter.Send<Object>(new Object(), "StopBackgroundService");
                await AppSettingsHelper.SetAppSetting(null);
                await this.viewModel.WaitAndExecute(1000, async () =>
                {
                    App.Current.MainPage = new MainPage();
                    await InitUser();
                });
            });

            MessagingCenter.Subscribe<UpdateEmailPage>(this, "Logout", async (obj) =>
            {
                MessagingCenter.Send<Object>(new Object(), "StopBackgroundService");
                await AppSettingsHelper.SetAppSetting(null);
                await this.viewModel.WaitAndExecute(1000, async () =>
                {
                    App.Current.MainPage = new MainPage();
                    await InitUser();
                });
            });

            MessagingCenter.Subscribe<Object>(this, "Logout", async (obj) =>
            {
                MessagingCenter.Send<Object>(new Object(), "StopBackgroundService");
                await AppSettingsHelper.SetAppSetting(null);
                await this.viewModel.WaitAndExecute(1000, async () =>
                {
                    App.Current.MainPage = new MainPage();
                    await InitUser();
                });
            });

            MessagingCenter.Subscribe<SilupostServiceException>(this, "Logout", async (obj) =>
            {
                MessagingCenter.Send<Object>(new Object(), "StopBackgroundService");
                await AppSettingsHelper.SetAppSetting(null); 
                await this.viewModel.WaitAndExecute(1000, async () =>
                {
                    App.Current.MainPage = new MainPage();
                    await InitUser();
                });
            });
            
            MessagingCenter.Subscribe<App>(this, "OpenWelcomePage", async (obj) =>
            {
                try
                {
                    if (AppSettingsHelper.goIsLaunchFromURL && AppSettingsHelper.goLaunchFromURLData != null)
                    {
                        try
                        {
                            var _viewModel = new WelcomeViewModel(this.Navigation);
                            _viewModel.ShowAuthControls = true;
                            _viewModel.ShowTitle = true;
                            _viewModel.ShowSuccessMessage = false;
                            App.Current.MainPage = new WelcomePage(_viewModel);
                            await this.viewModel.WaitAndExecute(1000, async () =>
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
                            });
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

        async Task InitUser()
        {
            try
            {
                var appSettings = await AppSettingsHelper.GetAppSetting();
                if (appSettings == null)
                {
                    if (!AppSettingsHelper.goIsLaunchFromURL)
                    {
                        var _viewModel = new WelcomeViewModel(this.Navigation);
                        _viewModel.ShowAuthControls = true;
                        _viewModel.ShowTitle = true;
                        _viewModel.ShowSuccessMessage = false;
                        App.Current.MainPage = new WelcomePage(_viewModel);
                    }
                    else
                    {
                        var _viewModel = new WelcomeViewModel(this.Navigation);
                        _viewModel.ShowAuthControls = false;
                        _viewModel.ShowTitle = false;
                        _viewModel.ShowSuccessMessage = false;
                        App.Current.MainPage = new WelcomePage(_viewModel);

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
                }
                else
                {
                    if (appSettings != null && appSettings.UserSettings != null && string.IsNullOrEmpty(appSettings.UserSettings.SystemUserId))
                    {
                        var _viewModel = new WelcomeViewModel(this.Navigation);
                        _viewModel.ShowAuthControls = false;
                        _viewModel.ShowTitle = false;
                        _viewModel.ShowSuccessMessage = false;
                        App.Current.MainPage = new WelcomePage(_viewModel);
                        var loginViewModel = new LoginViewModel(this.Navigation);
                        _viewModel.ProgressDialog = UserDialogs.Instance.Loading("Loading...", null, "OK", true, MaskType.Gradient);
                        await App.Current.MainPage.Navigation.PushModalAsync(new NavigationPage(new LoginPage(loginViewModel)), true);
                    }
                    else if (appSettings != null && appSettings.UserSettings != null && !appSettings.IsAuthenticated)
                    {
                        var _viewModel = new WelcomeViewModel(this.Navigation);
                        _viewModel.ShowAuthControls = false;
                        _viewModel.ShowTitle = false;
                        _viewModel.ShowSuccessMessage = false;
                        App.Current.MainPage = new WelcomePage(_viewModel);
                        await App.Current.MainPage.Navigation.PushModalAsync(new NavigationPage(new LoginPage(new LoginViewModel(this.Navigation))), true);
                    }
                    else
                    {
                        AppSettingsHelper.AppSettings = appSettings;
                        try
                        {
                            var newToken = await this.viewModel.SystemUserService.GetRefreshToken(AppSettingsHelper.AppSettings.AppToken.RefreshToken);

                            appSettings.AppToken = new Models.AppTokenModel
                            {
                                AccessToken = newToken.AccessToken,
                                RefreshToken = newToken.RefreshToken
                            };
                        }
                        catch(Exception ex)
                        {
                            if (ex.Message.ToLower().Contains("invalid_grant"))
                            {
                                appSettings.IsAuthenticated = false;
                                await AppSettingsHelper.SetAppSetting(appSettings);
                                AppSettingsHelper.AppSettings = await AppSettingsHelper.GetAppSetting();

                                var user = await this.viewModel.SystemUserService.GetByCredentials(AppSettingsHelper.AppSettings.UserSettings.UserName, AppSettingsHelper.AppSettings.UserSettings.Password);
                                if (!user.IsSuccess)
                                    throw new Exception(user.Message);
                                appSettings.AppToken = new Models.AppTokenModel
                                {
                                    AccessToken = user.Data.Token.AccessToken,
                                    RefreshToken = user.Data.Token.RefreshToken
                                };
                            }
                            else
                            {
                                throw ex;
                            }
                        }
                        await AppSettingsHelper.SetAppSetting(appSettings);
                        AppSettingsHelper.AppSettings = await AppSettingsHelper.GetAppSetting();
                        MessagingCenter.Send<Object>(new Object(), "StartBackgroundService");
                        await InitTab();
                        if (this.IsLaunchFromShortcut)
                        {
                            switch (this.Page)
                            {
                                case SilupostPageTitle.EMERGENCY_CALL:
                                    CurrentPage = Children[0];
                                    break;
                                case SilupostPageTitle.CRIMEINCIDENT_MAP:
                                    CurrentPage = Children[2];
                                    break;
                                case SilupostPageTitle.TIMELINE:
                                    CurrentPage = Children[2];
                                    break;
                                case SilupostPageTitle.CRIMEINCIDENT_NEW_REPORT:
                                    CurrentPage = Children[2];
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
                                    catch (Exception ex)
                                    {
                                        this.viewModel.IsExecuting = false;
                                        SilupostExceptionLogger.GetError(ex);
                                    }
                                    break;
                            }
                            this.IsLaunchFromShortcut = false;
                            this.Page = null;
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                await AppSettingsHelper.SetAppSetting(null);
                CrossToastPopUp.Current.ShowToastMessage(string.Format(" {0}", ex.Message));
                await this.viewModel.WaitAndExecute(1000, async () =>
                {
                    await InitUser();
                });
            }
        }

        async Task InitTab()
        {
            if(!this.Children.Any(x=>x.Title == SilupostPageTitle.EMERGENCY_CALL))
                this.Children.Add(new NavigationPage(new EmergencyCallPage()) { IconImageSource = "icons8_police_station_96_BLACK.png", Title = SilupostPageTitle.EMERGENCY_CALL });

            if (!this.Children.Any(x => x.Title == SilupostPageTitle.CRIMEINCIDENT_MAP))
                this.Children.Add(new NavigationPage(new CrimeIncidentMap()) { IconImageSource = "icons8_marker_96_BLACK.png", Title = SilupostPageTitle.CRIMEINCIDENT_MAP });

            if (!this.Children.Any(x => x.Title == SilupostPageTitle.TIMELINE))
                this.Children.Add(new NavigationPage(new TimelinePage()) { IconImageSource = "icons8_activity_feed_96_BLACK.png", Title = SilupostPageTitle.TIMELINE });
        }

        async Task ShowInternetErrorWindow()
        {
            App.Current.MainPage = new ErrorMainPage(SilupostErrorPageTypeEnums.INTERNET_ERROR);
        }

        async Task ShowServerErrorWindow()
        {
            App.Current.MainPage = new ErrorMainPage(SilupostErrorPageTypeEnums.SERVER_ERROR);
        }

        async void CurrentPageHasChanged(object sender, EventArgs e)
        {
            var tabbedPage = (Xamarin.Forms.TabbedPage)sender;
            var title = tabbedPage.CurrentPage.Title;
            if (title.Equals(SilupostPageTitle.CRIMEINCIDENT_MAP))
            {
                this.On<Xamarin.Forms.PlatformConfiguration.Android>().SetIsSwipePagingEnabled(false);
            }
            else
            {
                this.On<Xamarin.Forms.PlatformConfiguration.Android>().SetIsSwipePagingEnabled(true);
            }
        }

        async void TabbedPage_Appearing(object sender, EventArgs e)
        {
            try
            {
                if (AppSettingsHelper.CanAccessInternet())
                {
                    var serverStatus = await this.viewModel.SystemConfigService.GetServerStatus();
                    if (serverStatus == SilupostServerStatusEnums.ACTIVE)
                    {
                        this.viewModel.ProgressDialog = UserDialogs.Instance.Loading("Loading...", null, "OK", true, MaskType.Gradient);
                        await InitUser();
                        await this.viewModel.InitLookup();
                        this.viewModel.ProgressDialog.Hide();
                    }
                    else
                    {
                        await ShowServerErrorWindow();
                    }
                }
                else
                {
                    await ShowInternetErrorWindow();
                }
            }
            catch(Exception ex)
            {
                this.viewModel.HasError = true;
                this.viewModel.IsExecuting = false;
                this.IsBusy = false; 
                if (ex.Message.ToLower().Contains("unexpected character encountered"))
                {
                    await ShowServerErrorWindow();
                }
                else if (ex.Message.ToLower().Contains("problem occurs while proccessing"))
                {
                    await ShowServerErrorWindow();
                }
                else if (ex.Message.ToLower().Contains("unable to resolve host"))
                {
                    await ShowInternetErrorWindow();
                }
                else if (ex.Message.ToLower().Contains("no address associated with hostname"))
                {
                    await ShowInternetErrorWindow();
                }
                else if (ex.Message.Contains(SilupostMessage.NO_INTERNET))
                {
                    await ShowInternetErrorWindow();
                }
                else
                {
                    this.viewModel.ErrorMessage = string.Format("{0}", SilupostMessage.APP_ERROR);
                    this.viewModel.ErrorImageSource = "icons8_error_80.png";
                }
                if (this.viewModel.ProgressDialog != null)
                    this.viewModel.ProgressDialog.Hide();
            }
        }
    }
}
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

namespace SilupostMobileApp.Views
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MainPage : Xamarin.Forms.TabbedPage
    {
        MainViewModel viewModel;
        public MainPage()
        {
            InitializeComponent();
            BindingContext = this.viewModel = new MainViewModel();
            On<Android>().SetToolbarPlacement(ToolbarPlacement.Bottom);
            CurrentPageChanged += CurrentPageHasChanged;

            MessagingCenter.Subscribe<NewCrimeIncidentReportViewModel, int>(this, "SelectTab", async (obj, tabIndex) =>
            {
                try
                {
                    //this.CurrentPage.TabIndex = tabIndex;
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
                    await InitTab();
                }
                catch (Exception ex)
                {
                    SilupostExceptionLogger.GetError(ex);
                }
            });
            MessagingCenter.Subscribe<UserProfileSettingsPage>(this, "Logout", async (obj) =>
            {
                await AppSettingsHelper.SetAppSetting(null);
                App.Current.MainPage = new MainPage();
                await InitUser();
            });
            MessagingCenter.Subscribe<Account.UpdateEmail.EmailRegistrationContentView>(this, "Logout", async (obj) =>
            {
                await AppSettingsHelper.SetAppSetting(null);
                App.Current.MainPage = new MainPage();
                await InitUser();
            });
            MessagingCenter.Subscribe<UpdateEmailPage>(this, "Logout", async (obj) =>
            {
                await AppSettingsHelper.SetAppSetting(null);
                App.Current.MainPage = new MainPage();
                await InitUser();
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
                var appsSettings = await AppSettingsHelper.GetAppSetting();
                if (appsSettings == null)
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
                    if (appsSettings != null && appsSettings.UserSettings != null && string.IsNullOrEmpty(appsSettings.UserSettings.SystemUserId))
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
                    else if (appsSettings != null && appsSettings.UserSettings != null && !appsSettings.IsAuthenticated)
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
                        AppSettingsHelper.AppSettings = appsSettings;
                        await InitTab();
                    }
                }
            }
            catch(Exception ex)
            {
                CrossToastPopUp.Current.ShowToastMessage(string.Format(" {0}", ex.Message));
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
            var dialog = UserDialogs.Instance.Loading("Loading...", null, "OK", true, MaskType.Gradient);
            InitUser();
            this.viewModel.InitLookup();
            dialog.Hide();
        }
    }
}
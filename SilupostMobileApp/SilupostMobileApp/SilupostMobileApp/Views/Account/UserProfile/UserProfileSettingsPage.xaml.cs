using SilupostMobileApp.ViewModels;
using SilupostMobileApp.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using SilupostMobileApp.Models;
using Acr.UserDialogs;

namespace SilupostMobileApp.Views.Account.UserProfile
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class UserProfileSettingsPage : ContentPage
    {
        UserProfileViewModel viewModel;
        public UserProfileSettingsPage(UserProfileViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = this.viewModel = viewModel;
            this.viewModel.ProgressDialog.Hide();
            MessagingCenter.Subscribe<UpdateUserInfoPage, SystemUserModel>(this, "UpdateSuccess", async (obj, model) =>
            {
                try
                {
                    this.viewModel.SystemUser = model;
                    await this.viewModel.InitUserProfileSettings();
                }
                catch (Exception ex)
                {
                    SilupostExceptionLogger.GetError(ex);
                }
            });
        }

        async void ContentPage_Appearing(object sender, EventArgs e)
        {
            this.viewModel.Title = "Settings";
        }
        async void OnItemSelected(object sender, EventArgs e)
        {
            try
            {
                var layout = (BindableObject)sender;
                var settings = (UserProfileSettingModel)layout.BindingContext;
                switch (settings.SettingName)
                {
                    case SilupostUserProfileSettingsMenu.BASIC_INFO:
                        if (this.viewModel.IsExecuting)
                            return;
                        this.viewModel.IsExecuting = true;
                        var registerViewModel = new UpdateUserInfoViewModel(this.Navigation);
                        registerViewModel.ProgressDialog = UserDialogs.Instance.Loading("Loading...", null, "OK", true, MaskType.Gradient);
                        registerViewModel.IsFromProfile = true;
                        registerViewModel.SystemUser = this.viewModel.SystemUser;
                        registerViewModel.FirstName.Value = this.viewModel.SystemUser.LegalEntity.FirstName;
                        registerViewModel.MiddleName.Value = this.viewModel.SystemUser.LegalEntity.MiddleName ?? string.Empty;
                        registerViewModel.LastName.Value = this.viewModel.SystemUser.LegalEntity.LastName;
                        registerViewModel.SelectedGender.Value = this.viewModel.GenderList.Where(x => x.GenderId == this.viewModel.SystemUser.LegalEntity.Gender.GenderId).FirstOrDefault();
                        registerViewModel.BirthDay.Value = this.viewModel.SystemUser.LegalEntity.BirthDate.Value;
                        await this.viewModel.WaitAndExecute(1000, async () =>
                        {
                            try
                            {
                                await Navigation.PushAsync(new UpdateUserInfoPage(registerViewModel), true);
                                this.viewModel.IsExecuting = false;
                            }
                            catch (Exception ex)
                            {
                                registerViewModel.ProgressDialog.Hide();
                                SilupostExceptionLogger.GetError(ex);
                            }
                        });
                        break;
                    case SilupostUserProfileSettingsMenu.EMAIL:
                        if (this.viewModel.IsExecuting)
                            return;
                        this.viewModel.IsExecuting = true;
                        var updateEmailViewModel = new UpdateEmailViewModel(this.Navigation);
                        updateEmailViewModel.ProgressDialog = UserDialogs.Instance.Loading("Loading...", null, "OK", true, MaskType.Gradient);
                        await this.viewModel.WaitAndExecute(1000, async () =>
                        {
                            try
                            {
                                
                                await Navigation.PushAsync(new UpdateEmailPage(updateEmailViewModel), true);
                                this.viewModel.IsExecuting = false;
                            }
                            catch (Exception ex)
                            {
                                SilupostExceptionLogger.GetError(ex);
                            }
                        });
                        break;
                    case SilupostUserProfileSettingsMenu.CHANGE_PASSWORD:
                        
                        if (this.viewModel.IsExecuting)
                            return;
                        this.viewModel.IsExecuting = true;
                        var changePasswordViewModel = new ChangePasswordViewModel(this.Navigation);
                        changePasswordViewModel.IsFromProfile = true;
                        changePasswordViewModel.ProgressDialog = UserDialogs.Instance.Loading("Loading...", null, "OK", true, MaskType.Gradient);
                        await this.viewModel.WaitAndExecute(1000, async () =>
                        {
                            try
                            {
                                await Navigation.PushAsync(new ChangePasswordPage(changePasswordViewModel), true);
                                this.viewModel.IsExecuting = false;
                            }
                            catch (Exception ex)
                            {
                                SilupostExceptionLogger.GetError(ex);
                            }
                        });
                        break;
                    case SilupostUserProfileSettingsMenu.REPORT_SETTINGS:
                        break;
                    case SilupostUserProfileSettingsMenu.LOGOUT:
                        this.viewModel.ProgressDialog = UserDialogs.Instance.Loading("Loading...", null, "OK", true, MaskType.Gradient);
                        Device.BeginInvokeOnMainThread(async () =>
                        {
                            await this.Navigation.PopToRootAsync();
                            MessagingCenter.Send(this, "Logout");
                        });
                        break;
                }
            }
            catch(Exception ex)
            {
                SilupostExceptionLogger.GetError(ex);
            }
        }
    }
}
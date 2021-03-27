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
using SilupostMobileApp.Views.Account.UserProfile;
using Plugin.Toast;
using Xamarin.Forms.PlatformConfiguration;
using SilupostMobileApp.Models;
using Acr.UserDialogs;

namespace SilupostMobileApp.Views.Account
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class UserProfilePage : ContentPage
    {
        UserProfileViewModel viewModel;
        public UserProfilePage(UserProfileViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = this.viewModel = viewModel;
            this.viewModel.ProgressDialog.Hide();
            MessagingCenter.Subscribe<UpdateUserInfoPage, SystemUserModel>(this, "UpdateSuccess", async (obj, model) =>
            {
                try
                {
                    this.viewModel.SystemUser = model;
                    await AppSettingsHelper.SetAppSetting(new AppSettingsModel()
                    {
                        AppToken = new AppTokenModel()
                        {
                            AccessToken = AppSettingsHelper.AppSettings.AppToken.AccessToken,
                            RefreshToken = AppSettingsHelper.AppSettings.AppToken.RefreshToken,
                        },
                        UserSettings = new AppUserSettingsModel()
                        {
                            SystemUserId = model.SystemUserId,
                            UserName = model.UserName,
                            FullName = string.Format("{0} {1}", model.LegalEntity.FirstName, model.LegalEntity.LastName),
                            ProfilePictureFileId = model.ProfilePicture.FileId
                        },
                        IsAuthenticated = true
                    });
                    AppSettingsHelper.AppSettings = await AppSettingsHelper.GetAppSetting();
                    await this.viewModel.InitUserProfile();
                }
                catch (Exception ex)
                {
                    SilupostExceptionLogger.GetError(ex);
                }
            });
        }

        async void ContentPage_Appearing(object sender, EventArgs e)
        {
            viewModel.Title = "Profile";
            this.viewModel.ProgressDialog.Show();
            await this.viewModel.WaitAndExecute(1000, async () =>
            {
                try
                {
                    await this.viewModel.InitUserProfile();
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
                await this.viewModel.Navigation.PopToRootAsync();
            });
            return true;
        }
        async void ContentPage_Disappearing(object sender, EventArgs e)
        {
        }

        async void TitleBackButton_Clicked(object sender, EventArgs e)
        {
            try
            {
                if (this.viewModel.IsExecuting)
                    return;
                this.viewModel.IsExecuting = true;
                await this.Navigation.PopModalAsync();
                this.viewModel.IsExecuting = false;
            }
            catch (Exception ex)
            {
                this.viewModel.IsExecuting = false;
                SilupostExceptionLogger.GetError(ex);
            }
        }

        async void TitleSettingsButton_Clicked(object sender, EventArgs e)
        {
            try
            {
                if (this.viewModel.IsExecuting)
                    return;
                this.viewModel.IsExecuting = true;
                this.viewModel.ProgressDialog = UserDialogs.Instance.Loading("Loading...", null, "OK", true, MaskType.Gradient);
                await Navigation.PushAsync(new UserProfileSettingsPage(this.viewModel), true);
                this.viewModel.IsExecuting = false;
                this.viewModel.ProgressDialog.Hide();
            }
            catch (Exception ex)
            {
                SilupostExceptionLogger.GetError(ex);
            }
        }
    }
}
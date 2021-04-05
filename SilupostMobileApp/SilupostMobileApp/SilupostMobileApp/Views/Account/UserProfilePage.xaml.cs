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
using SilupostMobileApp.Views.Common;
using System.IO;

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
                            ProfilePictureFileId = model.ProfilePicture.FileId,
                            FileContent = model.ProfilePicture.FileContent,
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
            MessagingCenter.Subscribe<MediaUploadViewerPage, SilupostMediaModel>(this, "UploadMedia", async (obj, item) =>
            {
                try
                {
                    this.viewModel.ProgressDialog = UserDialogs.Instance.Loading("Loading...", null, "OK", true, MaskType.Gradient);
                    await this.viewModel.WaitAndExecute(1000, async () =>
                    {
                        try
                        {
                            this.viewModel.SystemUser = await this.viewModel.SystemUserService.Get(AppSettingsHelper.AppSettings.UserSettings.SystemUserId);
                            this.viewModel.ProgressDialog.Show();
                            this.viewModel.SystemUser.ProfilePicture = new FileModel()
                            {
                                FileId = item.FileId,
                                FileName = item.FileName,
                                MimeType = item.MimeType,
                                FileSize = item.FileSize,
                                FileContent = item.FileContent,
                                ImageSource = ImageSource.FromStream(() => { return new MemoryStream(item.FileContent); })
                            };
                            var model = new UpdateSystemUserBindingModel()
                            {
                                SystemUserId = this.viewModel.SystemUser.SystemUserId,
                                UserName = this.viewModel.SystemUser.UserName,
                                FirstName = this.viewModel.SystemUser.LegalEntity.FirstName ?? string.Empty,
                                LastName = this.viewModel.SystemUser.LegalEntity.LastName ?? string.Empty,
                                MiddleName = this.viewModel.SystemUser.LegalEntity.MiddleName ?? string.Empty,
                                GenderId = this.viewModel.SystemUser.LegalEntity.Gender.GenderId,
                                EmailAddress = this.viewModel.SystemUser.UserName,
                                MobileNumber = this.viewModel.SystemUser.LegalEntity.MobileNumber ?? string.Format("{0}", 0),
                                BirthDate = this.viewModel.SystemUser.LegalEntity.BirthDate,
                                ProfilePicture = new UpdateFileBindingModel()
                                {
                                    HasChange = false,
                                    FileId = AppSettingsHelper.AppSettings.UserSettings.ProfilePictureFileId,
                                    FileName = this.viewModel.SystemUser.ProfilePicture.FileName,
                                    MimeType = this.viewModel.SystemUser.ProfilePicture.MimeType,
                                    FileSize = this.viewModel.SystemUser.ProfilePicture.FileSize,
                                    FileFromBase64String = Convert.ToBase64String(this.viewModel.SystemUser.ProfilePicture.FileContent, 0, this.viewModel.SystemUser.ProfilePicture.FileContent.Length),
                                    IsDefault = false,
                                }
                            };
                            var result = await this.viewModel.UpdateProfile(model);
                            if (result != null)
                            {
                                await AppSettingsHelper.SetAppSetting(new AppSettingsModel()
                                {
                                    AppToken = new AppTokenModel()
                                    {
                                        AccessToken = AppSettingsHelper.AppSettings.AppToken.AccessToken,
                                        RefreshToken = AppSettingsHelper.AppSettings.AppToken.RefreshToken
                                    },
                                    UserSettings = new AppUserSettingsModel()
                                    {
                                        SystemUserId = result.SystemUserId,
                                        UserName = result.UserName,
                                        FullName = string.Format("{0} {1}", result.LegalEntity.FirstName, result.LegalEntity.LastName),
                                        ProfilePictureFileId = result.ProfilePicture.FileId,
                                        FileContent = result.ProfilePicture.FileContent,
                                    },
                                    IsAuthenticated = true
                                });
                                AppSettingsHelper.AppSettings = await AppSettingsHelper.GetAppSetting();
                                await this.viewModel.InitUserProfile();
                                this.viewModel.IsExecuting = false;
                                this.viewModel.ProgressDialog.Hide();
                            }
                            else
                            {
                                this.viewModel.IsExecuting = false;
                                this.viewModel.ProgressDialog.Hide();
                            }
                        }
                        catch (Exception ex)
                        {
                            this.viewModel.ProgressDialog.Hide();
                            this.viewModel.IsExecuting = false;
                            SilupostExceptionLogger.GetError(ex);
                        }
                    });
                }
                catch(Exception ex)
                {
                    SilupostExceptionLogger.GetError(ex);
                }
            });
            InitUserProfile();
        }
        async void InitUserProfile() 
        {
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

        async void ContentPage_Appearing(object sender, EventArgs e)
        {
            viewModel.Title = "Profile";
        }
        protected override bool OnBackButtonPressed()
        {
            Device.BeginInvokeOnMainThread(async () => {
                await this.viewModel.Navigation.PopToRootAsync();
                await this.viewModel.WaitAndExecute(1000, async () =>
                {
                    MessagingCenter.Send(this, "ReloadProfile");
                });
            });
            return true;
        }
        async void ContentPage_Disappearing(object sender, EventArgs e)
        {
            MessagingCenter.Send(this, "ReloadProfile");
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
                await this.viewModel.InitUserProfileSettings();
                await this.viewModel.WaitAndExecute(1000, async () =>
                {
                    await Navigation.PushAsync(new UserProfileSettingsPage(this.viewModel), true);
                    this.viewModel.IsExecuting = false;
                    this.viewModel.ProgressDialog.Hide();
                });
            }
            catch (Exception ex)
            {
                this.viewModel.IsExecuting = false;
                this.viewModel.ProgressDialog.Hide();
                SilupostExceptionLogger.GetError(ex);
            }
        }

        async void ChangeProfile_Clicked(object sender, EventArgs e)
        {
            try
            {
                if (this.viewModel.IsExecuting)
                    return;
                this.viewModel.IsExecuting = true;
                this.viewModel.ProgressDialog = UserDialogs.Instance.Loading("Loading...", null, "OK", true, MaskType.Gradient);
                this.viewModel.SystemUser = await this.viewModel.SystemUserService.Get(AppSettingsHelper.AppSettings.UserSettings.SystemUserId);
                await this.viewModel.WaitAndExecute(1000, async () =>
                {
                    var viewModel = new MediaUploadViewerViewModel(this.Navigation);
                    viewModel.IsImage = true;
                    viewModel.AllowMultipleType = false;
                    viewModel.NewMedia = new SilupostMediaModel()
                    {
                        IsNew = false,
                        FileId = this.viewModel.SystemUser.ProfilePicture.FileId,
                        FileName = this.viewModel.SystemUser.ProfilePicture.FileName,
                        MimeType = this.viewModel.SystemUser.ProfilePicture.MimeType,
                        FileSize = this.viewModel.SystemUser.ProfilePicture.FileSize,
                        FileContent = this.viewModel.SystemUser.ProfilePicture.FileContent,
                        ImageSource = ImageSource.FromStream(() => { return new MemoryStream(this.viewModel.SystemUser.ProfilePicture.FileContent); })
                    };
                    await this.Navigation.PushAsync(new MediaUploadViewerPage(viewModel), true);
                    this.viewModel.IsExecuting = false;
                    this.viewModel.ProgressDialog.Hide();
                });
            }
            catch (Exception ex)
            {
                this.viewModel.IsExecuting = false;
                this.viewModel.ProgressDialog.Hide();
                SilupostExceptionLogger.GetError(ex, SilupostMessage.APP_ERROR + string.Format(" {0}", ex.Message));
            }
        }
    }
}
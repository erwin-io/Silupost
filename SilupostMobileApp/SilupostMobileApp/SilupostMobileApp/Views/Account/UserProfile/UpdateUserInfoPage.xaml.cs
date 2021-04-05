using Acr.UserDialogs;
using SilupostMobileApp.BindingModels;
using SilupostMobileApp.Common;
using SilupostMobileApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SilupostMobileApp.Views.Account.UserProfile
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class UpdateUserInfoPage : ContentPage
    {
        UpdateUserInfoViewModel viewModel;
        public UpdateUserInfoPage(UpdateUserInfoViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = this.viewModel = viewModel;
            this.viewModel.Title = "Update Info";
            this.viewModel.ProgressDialog.Hide();
        }

        async void ButtonSaveUserInfo_Clicked(object sender, EventArgs e)
        {
            try
            {
                if (await this.viewModel.UserInfoValid())
                {
                    if (this.viewModel.IsExecuting)
                        return;
                    this.viewModel.IsExecuting = true;
                    this.viewModel.ProgressDialog.Show();
                    var model = new UpdateSystemUserBindingModel()
                    {
                        SystemUserId = this.viewModel.SystemUser.SystemUserId,
                        UserName = this.viewModel.SystemUser.UserName,
                        FirstName = this.viewModel.FirstName.Value ?? string.Empty,
                        LastName = this.viewModel.LastName.Value ?? string.Empty,
                        MiddleName = this.viewModel.MiddleName.Value ?? string.Empty,
                        GenderId = this.viewModel.SelectedGender.Value.GenderId,
                        EmailAddress = this.viewModel.SystemUser.UserName,
                        MobileNumber = this.viewModel.SystemUser.LegalEntity.MobileNumber ?? string.Format("{0}", 0),
                        BirthDate = this.viewModel.BirthDay.Value,
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
                    var result = await this.viewModel.UpdateInfo(model);
                    if (result != null)
                    {
                        this.viewModel.IsExecuting = false;
                        this.viewModel.ProgressDialog.Hide();
                        await this.Navigation.PopAsync();
                        MessagingCenter.Send(this, "UpdateSuccess", result);
                    }
                    else
                    {
                        this.viewModel.IsExecuting = false;
                        this.viewModel.ProgressDialog.Hide();
                    }
                }
            }
            catch(Exception ex)
            {
                SilupostExceptionLogger.GetError(ex);
                this.viewModel.IsExecuting = false;
                this.viewModel.ProgressDialog.Hide();
            }
        }

        async void SelectedGender_Change(object sender, EventArgs e)
        {
            await this.viewModel.TextChanged("SelectedGender");
        }
    }
}
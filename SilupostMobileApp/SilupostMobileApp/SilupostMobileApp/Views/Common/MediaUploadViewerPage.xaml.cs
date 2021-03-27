using SilupostMobileApp.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SilupostMobileApp.Common;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using SilupostMobileApp.Models;
using Plugin.Toast;

namespace SilupostMobileApp.Views.Common
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MediaUploadViewerPage : ContentPage
    {
        MediaUploadViewerViewModel viewModel;
        public MediaUploadViewerPage(MediaUploadViewerViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = this.viewModel = viewModel;
            this.viewModel.Title = string.Empty;
        }

        async void ContentPage_Appearing(object sender, EventArgs e)
        {
            var action = await DisplayActionSheet(null, "Cancel", null, SilupostServiceMediaSelectAction.PICKFILE, SilupostServiceMediaSelectAction.TAKEFROMCAMERA);
            this.viewModel.IsAudio = false;
            this.viewModel.IsVideo = false;
            this.viewModel.IsImage = false;
            if(action == SilupostServiceMediaSelectAction.PICKFILE)
            {
                await this.viewModel.PickFile();
            }
            else if(action == SilupostServiceMediaSelectAction.TAKEFROMCAMERA)
            {
                var cameraAction = await DisplayActionSheet(null, "Cancel", null, SilupostServiceMediaSelectFileType.IMAGE, SilupostServiceMediaSelectFileType.VIDEO);
                var cameraFileType = SilupostDocReportMediaTypeEnums.NA;
                if (cameraAction == SilupostServiceMediaSelectFileType.IMAGE)
                {
                    cameraFileType = SilupostDocReportMediaTypeEnums.IMAGE;
                }
                else if (cameraAction == SilupostServiceMediaSelectFileType.VIDEO)
                {
                    cameraFileType = SilupostDocReportMediaTypeEnums.VIDEO;
                }
                await this.viewModel.TakeFromCamera(cameraFileType);
            }
            else
            {
                await this.Navigation.PopAsync();
            }

            if(this.viewModel.NewMedia == null)
                await this.Navigation.PopAsync();
        }

        async void UploadOk_Clicked(object sender, EventArgs e)
        {
            MessagingCenter.Send(this, "UploadMedia", viewModel.NewMedia);
            await this.Navigation.PopAsync();
        }
        async void ResetUpload_Clicked(object sender, EventArgs e)
        {
            var result = await Application.Current.MainPage.DisplayAlert("Reset File", "Do you want to continue?", "Yes", "No");
            if (!result)
                return;

            try
            {
                var action = await DisplayActionSheet(null, "Cancel", null, SilupostServiceMediaSelectAction.PICKFILE, SilupostServiceMediaSelectAction.TAKEFROMCAMERA);
                this.viewModel.IsAudio = false;
                this.viewModel.IsVideo = false;
                this.viewModel.IsImage = false;

                switch (action)
                {
                    case SilupostServiceMediaSelectAction.PICKFILE:
                        await this.viewModel.PickFile();
                        break;
                    case SilupostServiceMediaSelectAction.TAKEFROMCAMERA:

                        var cameraAction = await DisplayActionSheet(null, "Cancel", null, SilupostServiceMediaSelectFileType.IMAGE, SilupostServiceMediaSelectFileType.VIDEO);
                        var cameraFileType = SilupostDocReportMediaTypeEnums.NA;
                        switch (cameraAction)
                        {
                            case SilupostServiceMediaSelectFileType.IMAGE:
                                cameraFileType = SilupostDocReportMediaTypeEnums.IMAGE;
                                break;
                            case SilupostServiceMediaSelectFileType.VIDEO:
                                cameraFileType = SilupostDocReportMediaTypeEnums.VIDEO;
                                break;
                        }
                        await this.viewModel.TakeFromCamera(cameraFileType);
                        break;
                    default:
                        await this.Navigation.PopAsync();
                        break;
                }
                if (this.viewModel.NewMedia == null)
                    await this.Navigation.PopAsync();
            }
            catch(Exception ex)
            {
                CrossToastPopUp.Current.ShowToastMessage(SilupostMessage.APP_ERROR + string.Format(" {0}", ex.Message));
            }
        }
    }
}
using Plugin.Toast;
using SilupostMobileApp.Common;
using SilupostMobileApp.Common.Interface;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using SilupostMobileApp.Models;
using System.IO;

namespace SilupostMobileApp.ViewModels
{

    public class MediaViewerViewModel : BaseViewModel
    {
        #region MODEL PROPERTIES
        SilupostMediaModel _silupostMedia;
        public SilupostMediaModel SilupostMedia
        {
            get => _silupostMedia;
            set => SetProperty(ref _silupostMedia, value);
        }

        bool _isAudio;
        public bool IsAudio
        {
            get => _isAudio;
            set => SetProperty(ref _isAudio, value);
        }

        bool _isVideo;
        public bool IsVideo
        {
            get => _isVideo;
            set => SetProperty(ref _isVideo, value);
        }

        bool _isImage;
        public bool IsImage
        {
            get => _isImage;
            set => SetProperty(ref _isImage, value);
        }
        #endregion

        #region UI PROPERTIES
        public INavigation Navigation { get; set; }

        #endregion

        #region COMMANDS

        #endregion

        public MediaViewerViewModel(INavigation pNavigation, SilupostMediaModel SilupostMedia)
        {
            this.Navigation = pNavigation;
            this.SilupostMedia = SilupostMedia;
        }

        public async Task InitMedia()
        {
            this.IsAudio = false;
            this.IsVideo = false;
            this.IsImage = false;
            if (SilupostMedia.MediaType == SilupostDocReportMediaTypeEnums.AUDIO)
            {
                this.IsAudio = true;
            }
            else if (SilupostMedia.MediaType == SilupostDocReportMediaTypeEnums.VIDEO)
            {
                this.IsVideo = true;
            }
            else
            {
                this.IsImage = true;
            }
        }
    }
}

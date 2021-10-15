using Plugin.Toast;
using SilupostMobileApp.Common;
using SilupostMobileApp.Common.Interface;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using SilupostMobileApp.Models;
using System.IO;
using System.Collections.ObjectModel;
using Plugin.FilePicker;
using Plugin.Media;
using Plugin.Media.Abstractions;

namespace SilupostMobileApp.ViewModels
{

    public class MediaUploadViewerViewModel : BaseViewModel
    {
        #region MODEL PROPERTIES
        SilupostMediaModel _newMedia;
        public SilupostMediaModel NewMedia
        {
            get => _newMedia;
            set => SetProperty(ref _newMedia, value);
        }
        bool _allowMultipleType;
        public bool AllowMultipleType
        {
            get => _allowMultipleType;
            set => SetProperty(ref _allowMultipleType, value);
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

        public MediaUploadViewerViewModel(INavigation pNavigation)
        {
            this.Navigation = pNavigation;
        }

        public async Task InitMediaUpload(SilupostServiceMediaSelectActionEnums actionType)
        {
        }
        public async Task PickFile()
        {
            SilupostMediaModel mediaFile = new SilupostMediaModel();
            try
            {
                mediaFile = new SilupostMediaModel();
                var file = await CrossFilePicker.Current.PickFile();
                if (file == null)
                    SilupostPopMessage.ShowToastMessage(SilupostMessage.APP_ERROR);
                var fileExtension = Path.GetExtension(file.FileName).Replace(".", String.Empty);
                var isFileValid = AppSettingsHelper.goAllowedMediaFileType.ToList().Any(f => f.Key.ToString() == fileExtension.ToUpper());
                if (!isFileValid)
                {
                    await Application.Current.MainPage.DisplayAlert("Oops!", "The selected file is not allowed or invalid", "ok");
                    return;
                }

                var mediaType = AppSettingsHelper.goGetMediaType(fileExtension);
                if (mediaType == SilupostDocReportMediaTypeEnums.IMAGE)
                {
                    this.IsImage = true;
                    var imageSource = ImageSource.FromStream(() => new MemoryStream(file.DataArray));
                    if (imageSource != null)
                    {
                        mediaFile = new SilupostMediaModel()
                        {
                            IsNew = true,
                            FileName = file.FileName,
                            FileSize = file.DataArray.Length,
                            FileContent = file.DataArray,
                            MimeType = AppSettingsHelper.goAllowedMediaFileType.ToList().FirstOrDefault(f => f.Key.ToString() == fileExtension.ToUpper()).Value,
                            MediaType = AppSettingsHelper.goGetMediaType(fileExtension),
                            SourceURL = file.FilePath,
                            ImageSource = imageSource,
                            IconSource = SilupostMediaTypeIconSource.IMAGE
                        };
                    }
                    else
                    {
                        await Application.Current.MainPage.DisplayAlert("Oops!", "The selected file is not allowed or invalid", "ok");
                        return;
                    }
                }
                else if(mediaType == SilupostDocReportMediaTypeEnums.VIDEO)
                {
                    this.IsVideo = true;
                    string fullPath = string.Format("file://{0}", file.FilePath);
                    var thumbnail = MediaHelpers.GenerateThumbImageFromLocal(file.FilePath, 1);
                    mediaFile = new SilupostMediaModel()
                    {
                        IsNew = true,
                        FileName = file.FileName,
                        FileSize = file.DataArray.Length,
                        FileContent = file.DataArray,
                        MimeType = AppSettingsHelper.goAllowedMediaFileType.ToList().FirstOrDefault(f => f.Key.ToString() == fileExtension.ToUpper()).Value,
                        MediaType = AppSettingsHelper.goGetMediaType(fileExtension),
                        SourceURL = file.FilePath,
                        ImageSource = thumbnail,
                        IconSource = SilupostMediaTypeIconSource.VIDEO
                    };
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("Oops!", "The selected file is not allowed or invalid", "ok");
                    return;
                }
                this.NewMedia = mediaFile;
            }
            catch (Exception ex)
            {
                SilupostPopMessage.ShowToastMessage(ex.Message);
            }
        }
        public async Task TakeFromCamera(SilupostDocReportMediaTypeEnums cameraFileType)
        {
            SilupostMediaModel mediaFile = new SilupostMediaModel();
            try
            {
                if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakeVideoSupported)
                {
                    return;
                }

                if(cameraFileType == SilupostDocReportMediaTypeEnums.IMAGE)
                {
                    var file = await CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions
                    {
                        PhotoSize = Plugin.Media.Abstractions.PhotoSize.Medium,
                        Directory = "SilupostMedia",
                        Name = $"Silupost-Media-({DateTime.Now.Year}-{DateTime.Now.Month}-{DateTime.Now.Day}-{DateTime.Now.TimeOfDay}).png"
                    });
                    await this.WaitAndExecute(1000, async () =>
                    {
                        if (file == null)
                            return;
                        var fileExtension = Path.GetExtension(file.Path).Replace(".", String.Empty);
                        var isFileValid = AppSettingsHelper.goAllowedMediaFileType.ToList().Any(f => f.Key.ToString() == fileExtension.ToUpper());
                        if (!isFileValid)
                        {
                            await Application.Current.MainPage.DisplayAlert("Oops!", "Camera error! Please check your camera", "ok");
                            return;
                        }

                        var fileStream = file.GetStream();
                        byte[] fileBytes = null;
                        var buffer = new byte[16 * 1024];
                        using (MemoryStream ms = new MemoryStream())
                        {
                            int read;
                            while ((read = fileStream.Read(buffer, 0, buffer.Length)) > 0)
                            {
                                ms.Write(buffer, 0, read);
                            }
                            fileBytes = ms.ToArray();
                        }

                        this.IsImage = true;
                        if (fileStream != null)
                        {
                            var imageSource = ImageSource.FromStream(() => file.GetStream());
                            mediaFile = new SilupostMediaModel()
                            {
                                IsNew = true,
                                FileName = file.Path,
                                FileSize = fileBytes.Length,
                                FileContent = fileBytes,
                                MimeType = AppSettingsHelper.goAllowedMediaFileType.ToList().FirstOrDefault(f => f.Key.ToString() == fileExtension.ToUpper()).Value,
                                MediaType = AppSettingsHelper.goGetMediaType(fileExtension),
                                SourceURL = file.Path,
                                ImageSource = imageSource,
                                IconSource = SilupostMediaTypeIconSource.IMAGE
                            };
                        }
                        else
                        {
                            await Application.Current.MainPage.DisplayAlert("Oops!", "Camera error! Please check your camera", "ok");
                            return;
                        }
                    });
                }
                else if(cameraFileType == SilupostDocReportMediaTypeEnums.VIDEO)
                {
                    var file = await CrossMedia.Current.TakeVideoAsync(new Plugin.Media.Abstractions.StoreVideoOptions
                    {
                        Directory = "SilupostMedia",
                        Name = $"Silupost-Media-({DateTime.Now.Year}-{DateTime.Now.Month}-{DateTime.Now.Day}-{DateTime.Now.TimeOfDay}).mp4"
                    });

                    await this.WaitAndExecute(1000, async () =>
                    {
                        if (file == null)
                            return;
                        var fileExtension = Path.GetExtension(file.Path).Replace(".", String.Empty);
                        var isFileValid = AppSettingsHelper.goAllowedMediaFileType.ToList().Any(f => f.Key.ToString() == fileExtension.ToUpper());
                        if (!isFileValid)
                        {
                            await Application.Current.MainPage.DisplayAlert("Oops!", "Camera error! Please check your camera", "ok");
                            return;
                        }

                        var fileStream = file.GetStream();
                        byte[] fileBytes = null;
                        var buffer = new byte[16 * 1024];
                        using (MemoryStream ms = new MemoryStream())
                        {
                            int read;
                            while ((read = fileStream.Read(buffer, 0, buffer.Length)) > 0)
                            {
                                ms.Write(buffer, 0, read);
                            }
                            fileBytes = ms.ToArray();
                        }

                        this.IsVideo = true;
                        var thumbnail = MediaHelpers.GenerateThumbImageFromLocal(file.Path, 1);
                        mediaFile = new SilupostMediaModel()
                        {
                            IsNew = true,
                            FileName = file.Path,
                            FileSize = fileBytes.Length,
                            FileContent = fileBytes,
                            MimeType = AppSettingsHelper.goAllowedMediaFileType.ToList().FirstOrDefault(f => f.Key.ToString() == fileExtension.ToUpper()).Value,
                            MediaType = AppSettingsHelper.goGetMediaType(fileExtension),
                            SourceURL = file.Path,
                            ImageSource = thumbnail,
                            IconSource = SilupostMediaTypeIconSource.VIDEO
                        };
                    });
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("Oops!", "Camera error! Please check your camera", "ok");
                    return;
                }
                this.NewMedia = mediaFile;
            }
            catch (Exception ex)
            {
                SilupostPopMessage.ShowToastMessage(ex.Message);
            }
        }
    }
}

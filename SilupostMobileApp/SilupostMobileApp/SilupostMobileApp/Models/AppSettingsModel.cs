using SilupostMobileApp.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace SilupostMobileApp.Models
{
    public class AppSettingsModel
    {
        public bool IsAuthenticated { get; set; }
        public AppUserSettingsModel UserSettings { get; set; }
        public AppTokenModel AppToken { get; set; }
    }

    public class AppUserSettingsModel
    {
        public string SystemUserId { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string FullName { get; set; }
        public byte[] FileContent { get; set; }
        public string ProfilePictureFileId { get; set; }
    }

    public class AppUserSettingsLaunchFromURLDataModel
    {
        public SilupostLaunchFromURLTypeEnums Type { get; set; }
        public string SystemUserId { get; set; }
        public string Code { get; set; }
        public string EmailId { get; set; }
    }
}

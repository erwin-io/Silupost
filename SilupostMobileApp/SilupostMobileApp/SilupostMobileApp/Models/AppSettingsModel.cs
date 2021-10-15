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
    public class AppConnectConfigModel
    {
        public string SILUPOST_API_URL { get; set; }
        public string SILUPOST_WEB_APP_URI { get; set; }
        public string MAP_BOX_TOKEN { get; set; }
        public string SILUPOST_WEBLANDINGPAGEHOST { get; set; }
        public string DEFAULT_LOCATION_LATITUDE { get; set; }
        public string DEFAULT_LOCATION_LONGITUDE { get; set; }
        public string REFRESH_TOKEN_INTERVAL_SECONDS { get; set; }
    }
}

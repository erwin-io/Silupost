using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Plugin.Geolocator;
using Plugin.Geolocator.Abstractions;
using Plugin.Settings;
using Plugin.Settings.Abstractions;
using SilupostMobileApp.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SilupostMobileApp.Common
{
    public static class AppSettingsHelper
    {
        private static ISettings _iAppSettings
        {
            get
            {
                return CrossSettings.Current;
            }
        }

        #region AppSettingsHelper Constants
        private const string AppSettingKey = "appsetting_key";
        private static readonly string AppSettingValue = string.Empty;
        #endregion

        public async static Task<AppSettingsModel> GetAppSetting()
        {
            var stringData = _iAppSettings.GetValueOrDefault(AppSettingKey, AppSettingValue);
            var result = new AppSettingsModel();
            JObject obj = JsonConvert.DeserializeObject<JObject>(stringData);
            result = obj != null ? obj.ToObject<AppSettingsModel>() : null;
            return result;
        }
        public async static Task<bool> SetAppSetting(AppSettingsModel model)
        {
            var success = false;
            try
            {
                var stringData = JsonConvert.SerializeObject(model);
                success = _iAppSettings.AddOrUpdateValue(AppSettingKey, stringData);
            }
            catch(Exception ex)
            {
                throw ex;
            }
            return success;
        }

        public static AppSettingsModel AppSettings
        {
            get;
            set;
        }

        static readonly DateTime UnixEpochStart = DateTime.SpecifyKind(new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc), DateTimeKind.Utc).ToLocalTime(); //Coverting date in to universal

        public static DateTime ToDateTimeFromEpoch(this long epochTime)
        {
            DateTime result = UnixEpochStart.AddMilliseconds(epochTime);
            return result;
        }


        public static readonly string goSILUPOST_WEBAPI_URI = SilupostAppSettings.SILUPOST_API_URI;
        public static string goSILUPOST_WEBAPI_Authentication { get; set; }
        public static string goMAP_BOX_TOKEN { get; set; }

        public static Dictionary<string, string> goAllowedMediaFileType = new Dictionary<string, string>()
        {
            { "WAV", "audio/vnd.wav" },
            { "MID", "audio/mid" },
            { "MP3", "audio/mpeg" },
            { "OGG", "audio/ogg" },
            { "RMA", "audio/vnd.rn-realaudio" },
            { "PNG", "image/png" },
            { "JPG", "image/jpeg" },
            { "JPEG", "image/jpeg" },
            { "BMP", "image/bmp" },
            { "GIF", "image/gif" },
            { "AVI", "video/x-msvideo" },
            { "MP4", "video/mp4" },
            { "WMV", "video/x-ms-wmv" }
        };

        public static SilupostDocReportMediaTypeEnums goGetMediaType(string fileExtension)
        {
            SilupostDocReportMediaTypeEnums result = SilupostDocReportMediaTypeEnums.NA;
            if(fileExtension.ToUpper().Equals("WAV")
                || fileExtension.ToUpper().Equals("MID")
                || fileExtension.ToUpper().Equals("MP3")
                || fileExtension.ToUpper().Equals("OGG")
                || fileExtension.ToUpper().Equals("RMA"))
            {
                result = SilupostDocReportMediaTypeEnums.AUDIO;
            }
            else if (fileExtension.ToUpper().Equals("PNG")
               || fileExtension.ToUpper().Equals("JPG")
               || fileExtension.ToUpper().Equals("JPEG")
               || fileExtension.ToUpper().Equals("BMP")
               || fileExtension.ToUpper().Equals("GIF"))
            {
                result = SilupostDocReportMediaTypeEnums.IMAGE;
            }
            else if(fileExtension.ToUpper().Equals("AVI")
                || fileExtension.ToUpper().Equals("MP4")
                || fileExtension.ToUpper().Equals("WMV"))
            {
                result = SilupostDocReportMediaTypeEnums.VIDEO;
            }
            return result;
        }

        public async static Task<Position> GetCurrentUserGeoLocation()
        {
            try
            {
                var locator = CrossGeolocator.Current;
                locator.DesiredAccuracy = 50;
                TimeSpan timeout = TimeSpan.FromMilliseconds(1000);
                return await locator.GetPositionAsync(timeout: timeout);
            } catch (Exception ex) { throw ex; }
        }
        public static bool IsValidTimeFormat(this string input)
        {
            TimeSpan dummyOutput;
            return TimeSpan.TryParse(input, out dummyOutput);
        }

        public static List<LookupTableModel> goLookupSettings { get; set; }

        public static bool goIsLaunchFromURL { get; set; }
        public static AppUserSettingsLaunchFromURLDataModel goLaunchFromURLData { get; set; }
    }
}

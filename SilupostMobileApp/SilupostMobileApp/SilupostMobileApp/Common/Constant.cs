using System;
using System.Collections.Generic;
using System.Text;

namespace SilupostMobileApp.Common
{
    public class SilupostAppSettings
    {
        public const string SILUPOST_API_CONNECT_URI = "https://api.npoint.io/de4e4a84e70250b866b4";
        public const string SILUPOST_API_URI = "http://192.168.254.109:1100/api/v1/";
        //public const string SILUPOST_API_URI = "http://silupost3api-001-site1.dtempurl.com/api/v1/";
        //public const string SILUPOST_WEB_APP_URI = "http://192.168.43.93:9300/";
        public const string SILUPOST_WEB_APP_URI = "http://192.168.254.109:1000/";
        public const string SILUPOST_WEB_CRIMEINCIDENT_MAP_URI_PATH = "reporttracker/mobileview";
        public const string SILUPOST_WEB_CRIMEINCIDENT_DETAILS_URI_PATH = "crimeIncidentReport/details/";
        public const string MAP_BOX_TOKEN = "pk.eyJ1IjoiZXJ3aW5yYW1pcmV6MjIwIiwiYSI6ImNrZ3U1cHJzazAwYTAycm82MDRmdWNmczAifQ.TarlRjuzi62vw_hPR6uTGg";
        public const string SILUPOST_WEBLANDINGPAGEHOST = "www.silupostweblandingpage.somee.com";
        public const float DEFAULT_LOCATION_LATITUDE = 16.407706109071114f;
        public const float DEFAULT_LOCATION_LONGITUDE = 120.58722946069425f;
        public const double CHECK_INTERNET_INTERVAL_SECONDS = 240;
        public const double REFRESH_TOKEN_INTERVAL_SECONDS = 7200;
        public const string APP_NAME = "Silupost";
    }
    public class SilupostEmergency
    {
        public const string EMERGENCY_CALL_NUMBER = "911";
    }
    public class SilupostCallLogType
    {
        public const string OUTGOING = "Outgoing";
        public const string INCOMING = "Incoming";
        public const string MISSED = "Missed";
    }
    public class SilupostMessage
    {
        public const string SELECTIONTYPE_ALL = "All {0}";
        public const string SELECTIONTYPE_SELECTED = "Selected {0}";
        public const string SUCCESS_SAVED = "{0} Successfully Saved!";
        public const string SUCCESS_UPDATED = "{0} Successfully Updated!";
        public const string SUCCESS_DELETED = "{0} Successfully Deleted!";
        public const string SUCCESS_REPORT_POSTED = "Report Successfully Posted!";
        public const string NO_RECORDS_FOUND = "There are no {0} record found";
        public const string RECORD_NOT_FOUND = "Opps, the record was not found";
        public const string SERVER_ERROR = "Opps, the application encountered problem while accessing the server, Please check your internet or contact the developer";
        public const string SERVER_INACTIVE = "There was a problem connecting to the server. Please try again.";
        public const string APP_ERROR = "Opps, the application encountered problem, Please try again!";
        public const string NO_INTERNET = "No Inernet found. Check your connection or try again.";
        public const string GPS_OR_LOCATION_ERROR = "Unable to get location!. Please swithched Location to 'ON'";
    }
    public class SilupostPageTitle
    {
        public const string EMERGENCY_CALL = "Emergency Call";
        public const string EMERGENCY_CALL_HISTORY = "Call History (Emergency Call)";
        public const string CRIMEINCIDENT_MAP = "Tracker";
        public const string CRIMEINCIDENT_NEW_REPORT = "New Crime/Incident Report";
        public const string CRIMEINCIDENT_VIEW_REPORT = "Crime/Incident Report";
        public const string CRIMEINCIDENT_MANAGE_REPORT = "Manage Crime/Incident Report";
        public const string TIMELINE = "Timeline";
    }
    public class SilupostTheme
    {
        public const string TEXT_ENTRY_FONT_COLOR = "#263238";
        public const string TEXT_ENTRY_LABEL_FOCUS_COLOR = "#039be5";
        public const string TEXT_ENTRY_BORDER_COLOR = "#BDBDBD";
        public const string TEXT_ENTRY_BORDER_FOCUS_COLOR = "#039be5";
        public const string TEXT_ENTRY_BORDER_HOVER_COLOR = "#039be5";
        public const string TEXT_ENTRY_BORDER_DISABLED_COLOR = "#e0e0e0";
        public const string TEXT_ENTRY_BORDER_COLOR_DANGER = "#FF5252";


        public const string BUTTON_FONT_COLOR = "#FFFFFF";
        public const string BUTTON_DOWN_FONT_COLOR = "#FFFFFF";
        public const string BUTTON_UP_FONT_COLOR = "#FFFFFF";
        public const string BUTTON_BACKGROUND_COLOR = "#3F51B5";
        public const string BUTTON_DOWN_BACKGROUND_COLOR = "#3F51B5";
        public const string BUTTON_UP_BACKGROUND_COLOR = "#3F51B5";
        public const string BUTTON_FOCUS_BACKGROUND_COLOR = "#3F51B5";
    }
    public class SilupostMediaTypeIconSource
    {
        public const string NA = "";
        public const string IMAGE = "icons8_image_file_384_WHITE.png";
        public const string VIDEO = "icons8_video_file_96_WHITE.png";
        public const string AUDIO = "icons8_audio_file_96_WHITE.png";
    }
    public class SilupostMapControlIconSource
    {
        public const string CURRENT_LOCATION_ON = "icons8_location_on_96_blue.png";
        public const string CURRENT_LOCATION_OFF = "icons8_location_on_96_black.png";
    }
    public class SilupostServiceMediaSelectAction
    {
        public const string TAKEFROMCAMERA = "Take Photo/Video";
        public const string PICKFILE = "Browse Library";
    }
    public class SilupostServiceMediaSelectFileType
    {
        public const string IMAGE = "Photo";
        public const string VIDEO = "Video";
    }
    public class SilupostSystemLookupTable
    {
        public const string ENTITY_GENDER = "EntityGender";
        public const string SYSTEM_CONFIG_TYPE = "SystemConfigType";
        public const string CRIME_INCIDENT_TYPE = "CrimeIncidentType";
        public const string CRIME_INCIDENT_CATEGORY = "CrimeIncidentCategory";
    }
    public class SilupostUserProfileSettingsMenu
    {
        public const string BASIC_INFO = "Basic Info";
        public const string EMAIL = "Email";
        public const string CHANGE_PASSWORD = "Change Password";
        public const string REPORT_SETTINGS = "Report Settings";
        public const string LOGOUT = "Logout";
    }
    public class SilupostUserProfileSettingsMenuGroup
    {
        public const string PROFILE = "Porfile";
        public const string ACCOUNT = "Account";
        public const string ADDITIONAL_SETTINGS = "Additional Settings";
        public const string ACTIONS = "Actions";
    }
    public class SilupostUserProfileSettingsMenuIcon
    {
        public const string BASIC_INFO = "icons8_user_96_WHITE.png";
        public const string EMAIL = "icons8_email_96_WHITE.png";
        public const string CHANGE_PASSWORD = "icons8_password_1_96_WHITE.png";
        public const string ADDITIONAL_SETTINGS = "icons8_settings_384_WHITE.png";
        public const string LOGOUT = "icons8_reset_96.png";
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace SilupostMobileApp.Common
{
    public enum SilupostDocReportMediaTypeEnums
    {
        NA = 0,
        IMAGE = 1,
        VIDEO = 2,
        AUDIO = 3
    }
    public enum SilupostServiceExceptionTypeEnums
    {
        NOT_FOUND = 1,
        SERVER_ERROR = 2,
        APP_ERROR = 3
    }
    public enum SilupostServiceMediaSelectActionEnums
    {
        NA = 0,
        TAKEFROMCAMERA = 1,
        PICKFILE = 2
    }
    public enum SilupostMapLookupTypeEnums
    {
        ADDRESS = 1,
        RANGE_OR_DISTANCE = 2
    }
    public enum SilupostLaunchFromURLTypeEnums
    {
        EMAIL_CONFIRMATION = 1,
        CHANGE_PASSWORD = 2
    }
}

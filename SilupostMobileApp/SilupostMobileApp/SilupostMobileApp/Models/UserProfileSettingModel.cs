using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace SilupostMobileApp.Models
{

    public class UserProfileSettingModel
    {
        public long Sequence { get; set; }
        public ImageSource SettingIconSource { get; set; }
        public string SettingGroupName { get; set; }
        public string SettingName { get; set; }
        public string SettingValue { get; set; }
        public bool ShowSettingValue { get; set; }
    }
}

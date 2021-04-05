using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Xamarin.Forms;
using SilupostMobileApp.Droid.Common;
using SilupostMobileApp.Common.Interface;

[assembly:Dependency(typeof(SilupostMobileApp.Droid.Common.LocalFiles))]
namespace SilupostMobileApp.Droid.Common
{
    public class LocalFiles : IBaseUrl
    {
        public string Get()
        {
            return "file:///android_asset/";
        }
    }
}
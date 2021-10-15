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
using SilupostMobileApp.Common.Interface;
using Xamarin.Forms;
using SilupostMobile.Droid.Common;

[assembly: Dependency(typeof(SilupostMobile.Droid.Common.ApplicationActivity))]
namespace SilupostMobile.Droid.Common
{
    public class ApplicationActivity : IApplicationActivity
    {

        WindowManagerFlags _originalFlags;
        public ApplicationActivity()
        {

        }

        public void CloseApplication()
        {
            var activity = (Activity)Forms.Context;
            activity.FinishAffinity();
        }

        #region IStatusBar implementation

        public void HideStatusBar()
        {
            var activity = (Activity)Forms.Context;
            activity.Window.AddFlags(WindowManagerFlags.Fullscreen);
        }

        public void ShowStatusBar()
        {
            var activity = (Activity)Forms.Context;
            activity.Window.ClearFlags(WindowManagerFlags.Fullscreen);
        }

        #endregion
    }
}
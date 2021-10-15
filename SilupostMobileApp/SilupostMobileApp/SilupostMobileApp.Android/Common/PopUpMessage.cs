using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;
using Android.Support.Design.Widget;
using Plugin.CurrentActivity;
using Android.App;
using Android.Widget;
using SilupostMobileApp.Common.Interface;

[assembly: Dependency(typeof(SilupostMobileApp.Droid.Common.PopUpMessage))]
namespace SilupostMobileApp.Droid.Common
{
    public class PopUpMessage : IPopUpMessage
    {
        public void ShowSnackbar(string message)
        {
            Activity activity = CrossCurrentActivity.Current.Activity;
            Android.Views.View activityRootView = activity.FindViewById(Android.Resource.Id.Content);
            Snackbar.Make(activityRootView, message, Snackbar.LengthLong).Show();
        }

        public void ShowToast(string message)
        {
            Activity activity = CrossCurrentActivity.Current.Activity;
            Toast.MakeText(Forms.Context, message, ToastLength.Long).Show();
        }
    }
}
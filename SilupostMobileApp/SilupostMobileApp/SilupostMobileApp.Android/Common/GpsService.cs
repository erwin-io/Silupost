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
using Android.Locations;

[assembly: Dependency(typeof(GpsService))]
namespace SilupostMobileApp.Droid.Common
{
    public class GpsService : IGpsService
    {

        public bool IsGpsEnable()
        {
            LocationManager locationManager = (LocationManager)Android.App.Application.Context.GetSystemService(Context.LocationService);
            return locationManager.IsProviderEnabled(LocationManager.GpsProvider);
        }

        public void OpenSettings()
        {

            Intent intent = new Intent(Android.Provider.Settings.ActionLocat‌​ionSourceSettings);
            intent.SetFlags(ActivityFlags.ClearTop | ActivityFlags.NewTask);

            try
            {
                Android.App.Application.Context.StartActivity(intent);

            }
            catch (ActivityNotFoundException activityNotFoundException)
            {
                System.Diagnostics.Debug.WriteLine(activityNotFoundException.Message);
                Android.Widget.Toast.MakeText(Android.App.Application.Context, "Error: Gps Activity", Android.Widget.ToastLength.Short).Show();
            }

        }

    }
}
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

namespace SilupostMobileApp.Droid.Common
{
    class BootReceiver : BroadcastReceiver
    {
        public override void OnReceive(Context context, Intent intent)
        {
            try
            {
                var serviceToStart = new Intent(context, typeof(BackgroundService));
                if (Android.OS.Build.VERSION.SdkInt >= Android.OS.BuildVersionCodes.O)
                {
                    context.StartForegroundService(serviceToStart);
                }
                else
                {
                    context.StartService(serviceToStart);
                }
            }
            catch (Exception ex)
            {
            }
        }
    }
}
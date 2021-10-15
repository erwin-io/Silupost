 using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Util;
using Plugin.Permissions;
using Plugin.Messaging;
using Octane.Xamarin.Forms.VideoPlayer.Android;
using Acr.UserDialogs;
using Android.Content;
using Plugin.Media;
using FFImageLoading.Forms.Droid;

using SilupostMobileApp.Common;
using SilupostMobileApp.Models;
using Xamarin.Forms;
using SilupostMobile.Droid.Common;
using Plugin.AppShortcuts;
using SilupostMobileApp;

namespace SilupostMobile.Droid
{
    [BroadcastReceiver(Enabled = true, DirectBootAware = true)]
    [IntentFilter(new[] { Intent.ActionView, Intent.ActionBootCompleted, Intent.ActionLockedBootCompleted, "android.intent.action.QUICKBOOT_POWERON", "com.htc.intent.action.QUICKBOOT_POWERON" }, Categories = new[] { Intent.CategoryDefault, Intent.CategoryBrowsable }, DataScheme = "http", DataHost = SilupostAppSettings.SILUPOST_WEBLANDINGPAGEHOST)]
    [IntentFilter(new[] { Intent.ActionView }, Categories = new[] { Intent.CategoryDefault}, DataScheme = "appshortcut", DataHost = "SilupostAppShortcut")]
    //[IntentFilter(new string[] { Intent.ActionBootCompleted, Intent.ActionLockedBootCompleted, "android.intent.action.QUICKBOOT_POWERON", "com.htc.intent.action.QUICKBOOT_POWERON" })]
    [Activity(Label = "Silupost", NoHistory = false, Icon = "@mipmap/icon", Theme = "@style/MainTheme", LaunchMode = LaunchMode.SingleInstance, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        private static Context context = global::Android.App.Application.Context;
        protected override async void OnCreate(Bundle savedInstanceState)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            StrictMode.VmPolicy.Builder builder = new StrictMode.VmPolicy.Builder();
            StrictMode.SetVmPolicy(builder.Build());

            base.OnCreate(savedInstanceState);

            //ACR User Dialogs
            UserDialogs.Init(this);

            CachedImageRenderer.Init();

            Plugin.MaterialDesignControls.Android.Renderer.Init();
            Xamarin.Forms.Forms.SetFlags("Brush_Experimental");

            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);

            //CrossMedia
            await CrossMedia.Current.Initialize();

            //Video Player SDK
            FormsVideoPlayer.Init();

            // get the accent color from your theme
            var themeAccentColor = new TypedValue();
            this.Theme.ResolveAttribute(Resource.Attribute.colorAccent, themeAccentColor, true);
            var droidAccentColor = new Android.Graphics.Color(themeAccentColor.Data);

            // set Xamarin Color.Accent to match the theme's accent color
            var accentColorProp = typeof(Xamarin.Forms.Color).GetProperty(nameof(Xamarin.Forms.Color.Accent), System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static);
            var xamarinAccentColor = new Xamarin.Forms.Color(droidAccentColor.R / 255.0, droidAccentColor.G / 255.0, droidAccentColor.B / 255.0, droidAccentColor.A / 255.0);
            accentColorProp.SetValue(null, xamarinAccentColor, System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static, null, null, System.Globalization.CultureInfo.CurrentCulture);

            var intent = Intent;
            var uri = intent.Data;
            if (uri != null)
            {
                AppSettingsHelper.goLaunchFromURLData = new AppUserSettingsLaunchFromURLDataModel();
                string originalPath = new Uri(uri.EncodedPath).OriginalString;
                if (originalPath.Contains("emailverification"))
                {
                    AppSettingsHelper.goLaunchFromURLData.Type = SilupostLaunchFromURLTypeEnums.EMAIL_CONFIRMATION;
                }
                else
                {
                    AppSettingsHelper.goLaunchFromURLData.Type = SilupostLaunchFromURLTypeEnums.EMAIL_CONFIRMATION;
                }
                var accessCode = uri.GetQueryParameter("code");
                var systemUserId = uri.GetQueryParameter("id");
                var email = uri.GetQueryParameter("email");
                AppSettingsHelper.goLaunchFromURLData.Code = accessCode ?? string.Empty;
                AppSettingsHelper.goLaunchFromURLData.SystemUserId = systemUserId ?? string.Empty;
                AppSettingsHelper.goLaunchFromURLData.EmailId = email ?? string.Empty;
                AppSettingsHelper.goIsLaunchFromURL = true;
            }
            CrossAppShortcuts.Current.Init();
            LoadApplication(new App());

            Loadservice();
        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            PermissionsImplementation.Current.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            //Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }


        private void Loadservice()
        {
            try
            {
                var backgroundServiceIntent = new Intent(Android.App.Application.Context, typeof(BackgroundService));
                backgroundServiceIntent.SetAction(BackgroundService.ACTION_START_SERVICE);
                if (Android.OS.Build.VERSION.SdkInt >= Android.OS.BuildVersionCodes.O)
                {
                    StartForegroundService(backgroundServiceIntent);
                }
                else
                {
                    StartService(backgroundServiceIntent);
                }
                MessagingCenter.Subscribe<Object>(this, "StartBackgroundService", (sender) => 
                {
                    try
                    {
                        var backgroundServiceIntent = new Intent(Android.App.Application.Context, typeof(BackgroundService));
                        backgroundServiceIntent.SetAction(BackgroundService.ACTION_START_SERVICE);
                        if (Android.OS.Build.VERSION.SdkInt >= Android.OS.BuildVersionCodes.O)
                        {
                            StartForegroundService(backgroundServiceIntent);
                        }
                        else
                        {
                            StartService(backgroundServiceIntent);
                        }
                    }
                    catch (Exception ex)
                    {
                        if (ex.Message.ToLower().Contains("service"))
                        {
                            Loadservice();
                        }
                        else
                        {
                            MessagingCenter.Send<Object>(new Object(), "Logout");
                        }
                    }
                });

                MessagingCenter.Subscribe<Object>(this, "StopBackgroundService", (sender) => 
                {
                    try
                    {
                        var backgroundServiceIntent = new Intent(Android.App.Application.Context, typeof(BackgroundService));
                        backgroundServiceIntent.SetAction(BackgroundService.ACTION_START_SERVICE);

                        StopService(backgroundServiceIntent);
                    }
                    catch (Exception ex)
                    {
                        if (ex.Message.ToLower().Contains("service"))
                        {
                            Loadservice();
                        }
                        else
                        {
                            MessagingCenter.Send<Object>(new Object(), "Logout");
                        }
                    }
                });
            }
            catch(Exception ex)
            {
                if (ex.Message.ToLower().Contains("service"))
                {
                    Loadservice();
                }
                else
                {
                    MessagingCenter.Send<Object>(new Object(), "Logout");
                }
            }
        }
    }
}
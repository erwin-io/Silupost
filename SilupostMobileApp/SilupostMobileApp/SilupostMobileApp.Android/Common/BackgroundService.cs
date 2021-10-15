using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.App;
using Android.Views;
using Android.Widget;
using Java.Lang;
using SilupostMobileApp.Services.Interface;
using Xamarin.Forms;

namespace SilupostMobileApp.Droid.Common
{
    [Service]
    public class BackgroundService : Service
    {
        private ISystemUserService SystemUserService => DependencyService.Get<ISystemUserService>();
        public const string ACTION_START_SERVICE = "START";
        public const string ACTION_STOP_SERVICE = "STOP";
        public const int NOTIFICATION_ID = 192837465;
        public static string CHANNEL_ID = "com.channelid";
        public static string CHANNEL_NAME = "com.channelname";
        public override IBinder OnBind(Intent intent)
        {
            return null;
        }

        /*
         * This service will run until stopped explicitly because we are returning sticky
         */
        public override void OnCreate()
        {
            try
            {
                var channel = new NotificationChannel(CHANNEL_ID, "FCM Notifications", NotificationImportance.Max)
                {
                    Description = "Firebase Cloud Messages appear in this channel",
                };
                channel.EnableVibration(true);
                channel.EnableLights(true);

                var notificationManager = (NotificationManager)GetSystemService(NotificationService);
                notificationManager.CreateNotificationChannel(channel);

                var notification = new Notification.Builder(this, CHANNEL_ID)
                .SetPriority(NotificationCompat.PriorityMax)
                .SetVibrate(new long[0])
                .SetContentTitle("Silupost")
                .SetContentText("Silupost is running")
                .SetSmallIcon(Resource.Drawable.SILUPOST_ICON)
                .SetOngoing(true);

                StartForeground(NOTIFICATION_ID, notification.Build());

                StartServiceInForeground();
                base.OnCreate();
            }
            catch(System.Exception ex)
            {
                StopSelf();
            }
        }

        public override StartCommandResult OnStartCommand(Intent intent, [GeneratedEnum] StartCommandFlags flags, int startId)
        {
            return StartCommandResult.Sticky;
        }
        /*
         * When our service is to be destroyed, show a Toast message before the destruction.
         */
        public override void OnDestroy()
        {
            base.OnDestroy();
        }

        void StartServiceInForeground()
        {
            Device.StartTimer(TimeSpan.FromSeconds(SilupostMobileApp.Common.SilupostAppSettings.CHECK_INTERNET_INTERVAL_SECONDS), () =>
            {
                CheckInternet();
                return true;
            });
            Device.StartTimer(TimeSpan.FromSeconds(SilupostMobileApp.Common.AppSettingsHelper.goREFRESH_TOKEN_INTERVAL_SECONDS), () =>
            {
                RefreshToken();
                return true;
            });
        }

        async void CheckInternet()
        {
            var result = SilupostMobileApp.Common.AppSettingsHelper.CanAccessInternet();
            if (!result)
            {
                MessagingCenter.Send<System.Object>(new System.Object(), "ApplicationIsOffline");
            }
            else
            {
                MessagingCenter.Send<System.Object>(new System.Object(), "ApplicationIsOnline");
            }
        }

        async void RefreshToken()
        {
            try
            {
                SilupostMobileApp.Common.AppSettingsHelper.AppSettings = await SilupostMobileApp.Common.AppSettingsHelper.GetAppSetting();
                var appSettings = SilupostMobileApp.Common.AppSettingsHelper.AppSettings;
                if(appSettings != null && appSettings.IsAuthenticated)
                {
                    var newToken = await SystemUserService.GetRefreshToken(SilupostMobileApp.Common.AppSettingsHelper.AppSettings.AppToken.RefreshToken);

                    appSettings.AppToken = new Models.AppTokenModel
                    {
                        AccessToken = newToken.AccessToken,
                        RefreshToken = newToken.RefreshToken
                    };
                    await SilupostMobileApp.Common.AppSettingsHelper.SetAppSetting(appSettings);
                }
            }

            catch (System.Exception ex)
            {
                //MessagingCenter.Send<System.Object>(new System.Object(), "Logout");
                //throw ex;
            }
        }
    }
}
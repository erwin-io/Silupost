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
using SilupostMobileApp.Common;
using SilupostMobileApp.Common.Interface;
using SilupostMobileApp.Models;
using Xamarin.Forms;
using SilupostMobile.Droid.Common;
using Android.Support.V4.Content;
using Android;
using Android.Support.V4.App;
using System.Security;
using Android.Provider;
using Android.Database;
using System.Threading.Tasks;

[assembly: Dependency(typeof(SilupostMobile.Droid.Common.PhoneCall))]
namespace SilupostMobile.Droid.Common
{
    public class PhoneCall : IPhoneCall
    {
        public async Task Call(string ContactNumber)
        {
            try
            {

                var URI = Android.Net.Uri.Parse(string.Format("tel:{0}", ContactNumber));
                Intent intent = new Intent(Intent.ActionCall, URI);
                intent.SetFlags(ActivityFlags.NewTask | ActivityFlags.NoUserAction);

                var activity = Xamarin.Forms.Forms.Context as Activity;
                int MY_PERMISSIONS_REQUEST_CALL_PHONE = 0;
                if (ContextCompat.CheckSelfPermission(activity, Manifest.Permission.CallPhone) != Android.Content.PM.Permission.Granted)
                {
                    if (!ActivityCompat.ShouldShowRequestPermissionRationale(activity, Manifest.Permission.CallPhone))
                    {
                        ActivityCompat.RequestPermissions(activity, new String[] { Manifest.Permission.CallPhone }, MY_PERMISSIONS_REQUEST_CALL_PHONE);
                    }
                }
                if (ContextCompat.CheckSelfPermission(activity, Manifest.Permission.ProcessOutgoingCalls) != Android.Content.PM.Permission.Granted)
                {
                    if (!ActivityCompat.ShouldShowRequestPermissionRationale(activity, Manifest.Permission.ProcessOutgoingCalls))
                    {
                        ActivityCompat.RequestPermissions(activity, new String[] { Manifest.Permission.ProcessOutgoingCalls }, MY_PERMISSIONS_REQUEST_CALL_PHONE);
                    }
                }
                Xamarin.Forms.Forms.Context.StartActivity(intent);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IList<PhoneCallLogsModel>> GetCallLogsByNumber(string pContactNumber)
        {
            List<PhoneCallLogsModel> result = new List<PhoneCallLogsModel>();
            var activity = Xamarin.Forms.Forms.Context as Activity;
            int MY_PERMISSIONS_REQUEST_CALL_LOGS = 0;
            if (ContextCompat.CheckSelfPermission(activity, Manifest.Permission.ReadCallLog) != Android.Content.PM.Permission.Granted)
            {
                if (!ActivityCompat.ShouldShowRequestPermissionRationale(activity, Manifest.Permission.ReadCallLog))
                {
                    ActivityCompat.RequestPermissions(activity, new String[] { Manifest.Permission.ReadCallLog }, MY_PERMISSIONS_REQUEST_CALL_LOGS);
                }
            }
            if (ContextCompat.CheckSelfPermission(activity, Manifest.Permission.WriteCallLog) != Android.Content.PM.Permission.Granted)
            {
                if (!ActivityCompat.ShouldShowRequestPermissionRationale(activity, Manifest.Permission.WriteCallLog))
                {
                    ActivityCompat.RequestPermissions(activity, new String[] { Manifest.Permission.WriteCallLog }, MY_PERMISSIONS_REQUEST_CALL_LOGS);
                }
            }

            string queryFilter = String.Format("{0}={1} and {2}={3}", CallLog.Calls.Type, (int)CallType.Outgoing, CallLog.Calls.Number, pContactNumber);
            string querySorter = String.Format("{0} desc ", CallLog.Calls.Date);
            ICursor queryData1 = Android.App.Application.Context.ContentResolver.Query(CallLog.Calls.ContentUri, null, queryFilter, null, querySorter);
            int indexContactNumber = queryData1.GetColumnIndex(CallLog.Calls.Number);
            int indexType = queryData1.GetColumnIndex(CallLog.Calls.Type);
            int indexDate = queryData1.GetColumnIndex(CallLog.Calls.Date);
            int indexDuration = queryData1.GetColumnIndex(CallLog.Calls.Duration);
            while (queryData1.MoveToNext())
            {
                int callTypeCode = int.Parse(queryData1.GetString(indexType));
                string contactNumber = queryData1.GetString(indexContactNumber);
                long dateAndTime = long.Parse(queryData1.GetString(indexDate));
                string callType = string.Empty;

                switch (callTypeCode)
                {
                    case (int)CallType.Outgoing:
                        callType = SilupostCallLogType.OUTGOING;
                        break;

                    case (int)CallType.Incoming:
                        callType = SilupostCallLogType.INCOMING;
                        break;

                    case (int)CallType.Missed:
                        callType = SilupostCallLogType.MISSED;
                        break;
                }
                string callDuration = queryData1.GetString(indexDuration);
                var callLog = new PhoneCallLogsModel()
                {
                    ContactNumber = contactNumber,
                    DateAndTime = AppSettingsHelper.ToDateTimeFromEpoch(dateAndTime),
                    CallType = callType,
                    CallDuration = int.Parse(callDuration)
                };
                result.Add(callLog);

            }
            return result;
        }

        public async Task<bool> ClearCallLogByNumber(string pContactNumber)
        {
            var success = false;
            try
            {
                string queryFilter = String.Format("{0}={1}", CallLog.Calls.Number, pContactNumber);
                var i = Android.App.Application.Context.ContentResolver.Delete(CallLog.Calls.ContentUri, queryFilter, null);
                success = i > 0;
                return success;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
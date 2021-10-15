using Plugin.Toast;
using SilupostMobileApp.Common;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using SilupostMobileApp.Models;
using Plugin.Messaging;
using System.Collections.ObjectModel;

namespace SilupostMobileApp.ViewModels
{

    public class CallHistoryViewModel : BaseViewModel
    {
        #region MODEL PROPERTIES
        private ObservableCollection<PhoneCallLogsModel> _phoneCallLogs;
        public ObservableCollection<PhoneCallLogsModel> PhoneCallLogs
        {
            get => _phoneCallLogs;
            set => SetProperty(ref _phoneCallLogs, value);
        }
        ObservableCollection<GroupingModel<DateTime, PhoneCallLogsModel>> _groupedPhoneCallLogs;
        public ObservableCollection<GroupingModel<DateTime, PhoneCallLogsModel>> GroupedPhoneCallLogs
        {
            get => _groupedPhoneCallLogs;
            set => SetProperty(ref _groupedPhoneCallLogs, value);
        }
        #endregion

        #region UI PROPERTIES
        public INavigation Navigation { get; set; }
        private bool _isEnabledClearEmergencyCallLog;
        public bool IsEnabledClearEmergencyCallLog
        {
            get => _isEnabledClearEmergencyCallLog;
            set => SetProperty(ref _isEnabledClearEmergencyCallLog, value);
        }
        #endregion

        #region COMMANDS
        public Command ClearEmergencyCallLogCommand { get; set; }
        #endregion

        public CallHistoryViewModel(INavigation pNavigation)
        {
            this.Navigation = pNavigation;
            PhoneCallLogs = new ObservableCollection<PhoneCallLogsModel>();
            ClearEmergencyCallLogCommand = new Command(async () => await ClearEmergencyCallLog());
        }

        public async Task InitCallLogs()
        {
            PhoneCallLogs = new ObservableCollection<PhoneCallLogsModel>();

            var callLogs = await PhoneCall.GetCallLogsByNumber(SilupostEmergency.EMERGENCY_CALL_NUMBER);

            GroupedPhoneCallLogs = new ObservableCollection<GroupingModel<DateTime, PhoneCallLogsModel>>();
            var sorted = from log in callLogs
                         orderby log.DateAndTime descending
                         group log by log.DateAndTime.ToString("MMM dd, yyyy") into logGroup
                         select new GroupingModel<DateTime, PhoneCallLogsModel>(DateTime.Parse(logGroup.Key), logGroup);

            GroupedPhoneCallLogs = new ObservableCollection<GroupingModel<DateTime, PhoneCallLogsModel>>(sorted);
            //foreach (var log in callLogs)
            //{
            //    PhoneCallLogs.Add(log);
            //}

            //IsEnabledClearEmergencyCallLog = PhoneCallLogs.Count > 0;
            IsEnabledClearEmergencyCallLog = GroupedPhoneCallLogs.Count > 0;
        }

        async Task ClearEmergencyCallLog()
        {
            try
            {
                var confirm = await Application.Current.MainPage.DisplayAlert("Confirm!", "Clear all Emergency Call history?", "Yes", "No");
                if (confirm)
                {
                    var result = await PhoneCall.ClearCallLogByNumber(SilupostEmergency.EMERGENCY_CALL_NUMBER);
                    SilupostPopMessage.ShowToastMessage(string.Format(SilupostMessage.SUCCESS_DELETED, Title));
                }
                await InitCallLogs();
            }
            catch (Exception ex)
            {
                SilupostPopMessage.ShowToastMessage(ex.Message);
            }
        }

    }
}

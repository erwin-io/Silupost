using Plugin.Toast;
using SilupostMobileApp.Common;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using SilupostMobileApp.Models;
using Plugin.Messaging;
using System.Collections.ObjectModel;

namespace SilupostMobileApp.ViewModels
{

    public class EmergencyCallViewModel : BaseViewModel
    {
        public INavigation Navigation { get; set; }
        public SystemUserModel SystemUser { get; set; }

        private ObservableCollection<PhoneCallLogsModel> _phoneCallLogs;
        public ObservableCollection<PhoneCallLogsModel> PhoneCallLogs
        {
            get => _phoneCallLogs;
            set => SetProperty(ref _phoneCallLogs, value);
        }

        ImageSource _imageSource;
        public ImageSource ImageSource
        {
            get => _imageSource;
            set => SetProperty(ref _imageSource, value);
        }

        public EmergencyCallViewModel(INavigation pNavigation)
        {
            this.Navigation = pNavigation;
            PhoneCallLogs = new ObservableCollection<PhoneCallLogsModel>();
        }

        public async Task EmergencyCall()
        {
            try
            {

                await PhoneCall.Call(SilupostEmergency.EMERGENCY_CALL_NUMBER);
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

    }
}

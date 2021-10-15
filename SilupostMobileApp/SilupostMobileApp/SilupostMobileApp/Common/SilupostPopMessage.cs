using System;
using System.Collections.Generic;
using System.Text;
using Plugin.Toast;
using SilupostMobileApp.Common.Interface;
using Xamarin.Forms;

namespace SilupostMobileApp.Common
{
    public static class SilupostPopMessage
    {
        public static void ShowToastMessage(string message)
        {
            if (Device.OS == TargetPlatform.Android)
            {
                DependencyService.Get<IPopUpMessage>().ShowToast(message);
            }
            else
            {
                SilupostPopMessage.ShowToastMessage(message);
            }
        }
        public static void ShowSnackbarMessage(string message)
        {
            if (Device.OS == TargetPlatform.Android)
            {
                DependencyService.Get<IPopUpMessage>().ShowSnackbar(message);
            }
            else
            {
                SilupostPopMessage.ShowToastMessage(message);
            }
        }
    }
}

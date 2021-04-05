using Plugin.Toast;
using SilupostMobileApp.Common;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using SilupostMobileApp.Models;

namespace SilupostMobileApp.ViewModels
{

    public class WelcomeViewModel : BaseViewModel
    {
        public INavigation Navigation { get; set; }

        bool _showTitle;
        public bool ShowTitle
        {
            get => _showTitle;
            set => SetProperty(ref _showTitle, value);
        }

        bool _showAuthControls;
        public bool ShowAuthControls
        {
            get => _showAuthControls;
            set => SetProperty(ref _showAuthControls, value);
        }

        bool _showSuccessMessage;
        public bool ShowSuccessMessage
        {
            get => _showSuccessMessage;
            set => SetProperty(ref _showSuccessMessage, value);
        }

        bool _isAuthenticated;
        public bool IsAuthenticated
        {
            get => _isAuthenticated;
            set => SetProperty(ref _isAuthenticated, value);
        }

        public WelcomeViewModel(INavigation navigation)
        {
            this.Navigation = navigation;
            if(AppSettingsHelper.goIsLaunchFromURL && AppSettingsHelper.goLaunchFromURLData != null)
            {
                this.ShowTitle = false;
                this.ShowAuthControls = false;
                this.ShowSuccessMessage = false;
            }
        }
    }
}

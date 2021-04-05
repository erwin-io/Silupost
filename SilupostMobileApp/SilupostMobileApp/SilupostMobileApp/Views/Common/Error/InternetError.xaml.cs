using SilupostMobileApp.Common;
using SilupostMobileApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SilupostMobileApp.Views.Common.Error
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class InternetError : ContentPage
    {
        ErrorViewModel viewModel;
        public InternetError()
        {
            InitializeComponent();
            BindingContext = this.viewModel = new ErrorViewModel();
        }

        protected override bool OnBackButtonPressed()
        {
            Device.BeginInvokeOnMainThread(async () => {
                var result = await Application.Current.MainPage.DisplayAlert("Exit!", "Do you want to continue?", "Yes", "No");
                if (result)
                {
                    this.viewModel.ApplicationActivity?.CloseApplication();
                }
            });

            return true;
        }

        async void Retry_Clicked(object sender, EventArgs e)
        {
            try
            {
                App.Current.MainPage = new MainPage();
            }
            catch (Exception ex)
            {
                SilupostExceptionLogger.GetError(ex);
            }
        }

        async void EmergencyCall_Clicked(object sender, EventArgs e)
        {
            try
            {
                await this.viewModel.PhoneCall.Call(SilupostEmergency.EMERGENCY_CALL_NUMBER);
            }
            catch (Exception ex)
            {
                SilupostExceptionLogger.GetError(ex);
            }
        }
    }
}
using Plugin.Messaging;
using SilupostMobileApp.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using SilupostMobileApp.CustomRender;
using SilupostMobileApp.Common;

namespace SilupostMobileApp.Views.Emergency
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EmergencyCallPage : ContentPage
    {
        EmergencyCallViewModel viewModel;
        public EmergencyCallPage()
        {
            InitializeComponent();
            BindingContext = viewModel = new EmergencyCallViewModel(this.Navigation);
            viewModel.Title = SilupostPageTitle.EMERGENCY_CALL;
        }
        async void CallHistory_Clicked(object sender, EventArgs e)
        {
            if (viewModel.IsExecuting)
                return;
            viewModel.IsExecuting = true;
            await Navigation.PushModalAsync(new NavigationPage(new CallHistoryPage()), true);
            viewModel.IsExecuting = false;
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
    }
}
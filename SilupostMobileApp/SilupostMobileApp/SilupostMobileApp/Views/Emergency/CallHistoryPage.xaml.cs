using SilupostMobileApp.Common;
using SilupostMobileApp.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SilupostMobileApp.Views.Emergency
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CallHistoryPage : ContentPage
    {
        CallHistoryViewModel viewModel;
        public CallHistoryPage()
        {
            InitializeComponent();
            BindingContext = viewModel = new CallHistoryViewModel(this.Navigation);
            viewModel.Title = SilupostPageTitle.EMERGENCY_CALL_HISTORY;
        }

        async void ContentPage_Appearing(object sender, EventArgs e)
        {
            try
            {
                await this.viewModel.InitCallLogs();
            }
            catch(Exception ex) { SilupostExceptionLogger.GetError(ex); }
        }
    }
}
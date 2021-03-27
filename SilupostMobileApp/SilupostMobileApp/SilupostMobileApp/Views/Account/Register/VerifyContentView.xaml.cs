using Acr.UserDialogs;
using SilupostMobileApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SilupostMobileApp.Views.Account.Register
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class VerifyContentView : ContentView
    {
        RegisterViewModel viewModel;
        public VerifyContentView(RegisterViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = this.viewModel = viewModel;
            //this.viewModel.ProgressDialog.Hide();
        }

        async void ButtonVerifyCode_Clicked(object sender, EventArgs e)
        {
            if (await this.viewModel.VerificationCodeValid())
            {
                MessagingCenter.Send(this, "VerifyCode");
            }
        }

        async void ButtonResend_Clicked(object sender, EventArgs e)
        {
            MessagingCenter.Send(this, "ResendEmail");
        }
    }
}
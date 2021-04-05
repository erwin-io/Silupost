using Acr.UserDialogs;
using SilupostMobileApp.Common;
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
    public partial class EmailRegistrationContentView : ContentView
    {
        RegisterViewModel viewModel;
        public EmailRegistrationContentView(RegisterViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = this.viewModel = viewModel;
            //this.viewModel.ProgressDialog.Hide();
        }

        async void ButtonSubmitEmail_Clicked(object sender, EventArgs e)
        {
            if (await this.viewModel.EmailValid())
            {
                if (this.viewModel.IsExecuting)
                    return;
                this.viewModel.IsExecuting = true;
                MessagingCenter.Send(this, "SubmitEmail");
                this.viewModel.IsExecuting = false;
            }
        }
    }
}
using Acr.UserDialogs;
using SilupostMobileApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SilupostMobileApp.Views.Account.ChangePassword
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EmailRegistrationContentView : ContentView
    {
        ChangePasswordViewModel viewModel;
        public EmailRegistrationContentView(ChangePasswordViewModel viewModel)
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
                await this.viewModel.WaitAndExecute(1000, async () =>
                {
                    MessagingCenter.Send(this, "SubmitEmailChangePassword");
                    this.viewModel.IsExecuting = false;
                });
            }
        }
    }
}
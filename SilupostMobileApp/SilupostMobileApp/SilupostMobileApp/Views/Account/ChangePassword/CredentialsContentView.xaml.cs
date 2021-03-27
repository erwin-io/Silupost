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
    public partial class CredentialsContentView : ContentView
    {
        ChangePasswordViewModel viewModel;
        public CredentialsContentView(ChangePasswordViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = this.viewModel = viewModel;
            //this.viewModel.ProgressDialog.Hide();
        }

        async void ButtonSaveCredentials_Clicked(object sender, EventArgs e)
        {
            if (await this.viewModel.CredentialsValid())
            {
                if (this.viewModel.IsExecuting)
                    return;
                this.viewModel.IsExecuting = true;
                await this.viewModel.WaitAndExecute(1000, async () =>
                {
                    MessagingCenter.Send(this, "ResetPassword");
                    this.viewModel.IsExecuting = false;
                });
            }
        }

        async void ButtonGotoLogin_Clicked(object sender, EventArgs e)
        {
            if (this.viewModel.IsExecuting)
                return;
            this.viewModel.IsExecuting = true;
            MessagingCenter.Send(this, "GotoLogin");
            this.viewModel.IsExecuting = false;
        }
    }
}
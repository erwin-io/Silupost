using Acr.UserDialogs;
using SilupostMobileApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SilupostMobileApp.Views.Account.UpdateEmail
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EmailRegistrationContentView : ContentView
    {
        UpdateEmailViewModel viewModel;
        public EmailRegistrationContentView(UpdateEmailViewModel viewModel)
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

        async void ButtonGotoLogin_Clicked(object sender, EventArgs e)
        {
            this.viewModel.ProgressDialog = UserDialogs.Instance.Loading("Loading...", null, "OK", true, MaskType.Gradient);
            Device.BeginInvokeOnMainThread(async () =>
            {
                await this.Navigation.PopToRootAsync();
                MessagingCenter.Send(this, "Logout");
            });
        }
    }
}
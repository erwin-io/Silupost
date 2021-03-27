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
    public partial class CredentialsContentView : ContentView
    {
        RegisterViewModel viewModel;
        public CredentialsContentView(RegisterViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = this.viewModel = viewModel;
            //this.viewModel.ProgressDialog.Hide();
        }

        async void ButtonSaveCredentials_Clicked(object sender, EventArgs e)
        {
            if (await this.viewModel.CredentialsValid())
            {
                MessagingCenter.Send(this, "SaveCredentials");
            }
        }
    }
}
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
    public partial class UserInfoContentView : ContentView
    {
        RegisterViewModel viewModel;
        public UserInfoContentView(RegisterViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = this.viewModel = viewModel;
            //this.viewModel.ProgressDialog.Hide();
        }

        async void ButtonSaveUserInfo_Clicked(object sender, EventArgs e)
        {
            if (await this.viewModel.UserInfoValid())
            {
                var result = await Application.Current.MainPage.DisplayAlert("Save!", "Do you want to continue?", "Yes", "No");
                if (result)
                {
                    MessagingCenter.Send(this, "SaveUserInfo");
                }
            }
        }

        async void ButtonBackToCredentials_Clicked(object sender, EventArgs e)
        {
            MessagingCenter.Send(this, "BackToCredentials");
        }

        async void SelectedGender_Change(object sender, EventArgs e)
        {
            await this.viewModel.TextChanged("SelectedGender");
        }
    }
}
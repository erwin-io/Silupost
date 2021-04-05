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
    public partial class ErrorMainPage : ContentPage
    {
        ErrorViewModel viewModel;
        SilupostErrorPageTypeEnums silupostErrorPageType;
        public ErrorMainPage(SilupostErrorPageTypeEnums silupostErrorPageType)
        {
            InitializeComponent();
            BindingContext = this.viewModel = new ErrorViewModel();
            this.silupostErrorPageType = silupostErrorPageType;
            ShowError();
        }

        async void ShowError()
        {
            await this.viewModel.WaitAndExecute(1000, async () =>
            {
                switch (this.silupostErrorPageType)
                {
                    case SilupostErrorPageTypeEnums.INTERNET_ERROR:
                        await this.Navigation.PushModalAsync(new InternetError(), true);
                        break;
                    case SilupostErrorPageTypeEnums.SERVER_ERROR:
                        await this.Navigation.PushModalAsync(new ServerError(), true);
                        break;
                    default:
                        await this.Navigation.PushModalAsync(new ServerError(), true);
                        break;
                }
            });
        }
    }
}
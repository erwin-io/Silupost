using SilupostMobileApp.Common;
using SilupostMobileApp.Models;
using SilupostMobileApp.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SilupostMobileApp.Views.Common
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MediaViewerPage : ContentPage
    {
        MediaViewerViewModel viewModel;
        public MediaViewerPage(MediaViewerViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = this.viewModel = viewModel;
            this.viewModel.Title = string.Empty;
        }

        async void ContentPage_Appearing(object sender, EventArgs e)
        {
            await this.viewModel.InitMedia();
        }
    }
}
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
    public partial class ImageCropperPage : ContentPage
    {
        public ImageCropperPage()
        {
            InitializeComponent();
        }

        async void ContentPage_Appearing(object sender, EventArgs e)
        {
        }
    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using SilupostMobileApp.Models;
using SilupostMobileApp.Views;
using SilupostMobileApp.ViewModels;

namespace SilupostMobileApp.Views
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class ItemsPage : ContentPage
    {
        ItemsViewModel viewModel;

        public ItemsPage()
        {
            InitializeComponent();

            BindingContext = viewModel = new ItemsViewModel();
        }

        async void OnItemSelected(object sender, EventArgs args)
        {
            if (viewModel.IsExecuting)
                return;
            viewModel.IsExecuting = true;
            var layout = (BindableObject)sender;
            var item = (Item)layout.BindingContext;
            await Navigation.PushAsync(new ItemDetailPage(new ItemDetailViewModel(item)));
            viewModel.IsExecuting = false;
        }

        async void AddItem_Clicked(object sender, EventArgs e)
        {
            if (viewModel.IsExecuting)
                return;
            viewModel.IsExecuting = true;
            await Navigation.PushModalAsync(new NavigationPage(new NewItemPage()));
            viewModel.IsExecuting = false;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (viewModel.Items.Count == 0)
                viewModel.IsBusy = true;
        }
    }
}
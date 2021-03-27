using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace SilupostMobileApp.CustomRender
{
    public class SilupostNavigationPage : NavigationPage
    {
        public SilupostNavigationPage(Page root) : base(root)
        {
            Pushed += SilupostNavigationPage_Pushed;
            Popped += SilupostNavigationPage_Popped;
        }

        private void SilupostNavigationPage_Popped(object sender, NavigationEventArgs e)
        {
            if (CurrentPage == RootPage)
            {
                var mainPage = App.Current.MainPage as SilupostTabbedPage;
                mainPage.IsHidden = false;
            }
        }

        private void SilupostNavigationPage_Pushed(object sender, NavigationEventArgs e)
        {
            var mainPage = App.Current.MainPage as SilupostTabbedPage;
            mainPage.IsHidden = true;
        }
    }
}

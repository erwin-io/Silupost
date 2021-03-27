using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace SilupostMobileApp.CustomRender
{
    public class SilupostTabbedPage : TabbedPage
    {
        public static readonly BindableProperty IsHiddenProperty =
            BindableProperty.Create(nameof(IsHidden), typeof(bool), typeof(SilupostTabbedPage), false);

        public bool IsHidden
        {
            get { return (bool)GetValue(IsHiddenProperty); }
            set { SetValue(IsHiddenProperty, value); }
        }
    }
}

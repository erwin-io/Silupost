using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Views;
using Android.Widget;
using SilupostMobileApp.CustomRender;
using SilupostMobileApp.Droid.CustomRender;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android.AppCompat;

[assembly: ExportRenderer(typeof(SilupostTabbedPage), typeof(SilupostTabbedPageRendererDroid))]
namespace SilupostMobileApp.Droid.CustomRender
{
    public class SilupostTabbedPageRendererDroid : TabbedPageRenderer
    {

        public SilupostTabbedPageRendererDroid(Context context) : base(context) { }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);
            if (e.PropertyName == "IsHidden")
            {
                TabLayout TabsLayout = null;
                for (int i = 0; i < ChildCount; ++i)
                {
                    Android.Views.View view = (Android.Views.View)GetChildAt(i).Parent.Parent;
                    if (view is TabLayout)
                    {
                        TabsLayout = view as TabLayout;
                    }
                    //if (TabsLayout == null)
                    //    continue;
                    if ((Element as SilupostTabbedPage).IsHidden)
                    {
                        view.Visibility = ViewStates.Invisible;
                    }
                    else
                    {
                        view.Visibility = ViewStates.Visible;
                    }
                }
            }
        }
    }
}
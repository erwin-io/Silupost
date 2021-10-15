using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Webkit;
using Android.Widget;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using SilupostMobile.Droid.Common;
using SilupostMobileApp.CustomRender;

[assembly: ExportRenderer(typeof(GeoWebView), typeof(GeoWebViewRenderer))]
namespace SilupostMobile.Droid.Common
{
    [Obsolete]
    public class GeoWebViewRenderer : WebViewRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.WebView> e)
        {
            base.OnElementChanged(e);
            Control.Settings.JavaScriptEnabled = true;
            Control.SetWebChromeClient(new MyWebClient());
        }
    }

    public class MyWebClient : WebChromeClient
    {
        public override void OnGeolocationPermissionsShowPrompt(string origin, GeolocationPermissions.ICallback callback)
        {
            callback.Invoke(origin, true, false);
        }
    }
}
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Content.Res;
using Android.Graphics;
using Android.Media;
using Android.OS;
using Android.Provider;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using SilupostMobileApp.Common;
using SilupostMobileApp.Common.Interface;
using Xamarin.Forms;

[assembly: Dependency(typeof(SilupostMobileApp.Droid.Common.FileSystemImplementation))]
namespace SilupostMobileApp.Droid.Common
{
    public class FileSystemImplementation : IFileSystem
    {
        public async Task<string> GetExternalStorageAsync()
        {
            Context context = Android.App.Application.Context;
            var filePath = context.GetExternalFilesDir("");
            return filePath.Path;
        }
    }
}
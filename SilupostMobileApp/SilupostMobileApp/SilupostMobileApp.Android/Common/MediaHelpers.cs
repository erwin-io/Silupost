using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.Media;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Java.IO;
using SilupostMobileApp.Common.Interface;
using Xamarin.Forms;

[assembly: Dependency(typeof(SilupostMobileApp.Droid.Common.MediaHelpers))]
namespace SilupostMobileApp.Droid.Common
{
    public class MediaHelpers : IMediaHelpers
    {
        public ImageSource GenerateThumbImageWeb(string url, long usecond)
        {
            MediaMetadataRetriever retriever = new MediaMetadataRetriever();
            retriever.SetDataSource(url, new Dictionary<string, string>());
            Bitmap bitmap = retriever.GetFrameAtTime(usecond);
            if (bitmap != null)
            {
                MemoryStream stream = new MemoryStream();
                bitmap.Compress(Bitmap.CompressFormat.Png, 0, stream);
                byte[] bitmapData = stream.ToArray();
                return ImageSource.FromStream(() => new MemoryStream(bitmapData));
            }
            return null;
        }
        public ImageSource GenerateThumbImageFromLocal(string url, long usecond)
        {
            Context context = Android.App.Application.Context;
            MediaMetadataRetriever retriever = new MediaMetadataRetriever();
            FileInputStream inputStream = null;
            inputStream = new FileInputStream(url);
            retriever.SetDataSource(inputStream.FD);
            Bitmap bitmap = retriever.GetFrameAtTime(usecond);
            if (bitmap != null)
            {
                MemoryStream stream = new MemoryStream();
                bitmap.Compress(Bitmap.CompressFormat.Png, 0, stream);
                byte[] bitmapData = stream.ToArray();
                return ImageSource.FromStream(() => new MemoryStream(bitmapData));
            }
            return null;
        }
    }
}
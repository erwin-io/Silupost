using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace SilupostMobileApp.Common.Interface
{
    public interface IMediaHelpers
    {
        ImageSource GenerateThumbImageWeb(string url, long usecond);
        ImageSource GenerateThumbImageFromLocal(string url, long usecond);
    }
}

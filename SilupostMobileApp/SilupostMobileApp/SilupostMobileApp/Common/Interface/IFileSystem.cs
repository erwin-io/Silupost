using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace SilupostMobileApp.Common.Interface
{
    public interface IFileSystem
    {
        Task<string> GetExternalStorageAsync();
    }
}

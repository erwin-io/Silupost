using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using SilupostMobileApp.Models;

namespace SilupostMobileApp.Services.Interface
{
    public interface IFileService
    {
        Task<FileModel> Get(string fileId);
    }
}

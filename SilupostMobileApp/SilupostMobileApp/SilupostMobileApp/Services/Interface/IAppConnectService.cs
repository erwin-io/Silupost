using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using SilupostMobileApp.Models;

namespace SilupostMobileApp.Services.Interface
{
    public interface IAppConnectService
    {
        Task<AppConnectConfigModel> Get(string jsonUrl);
    }
}

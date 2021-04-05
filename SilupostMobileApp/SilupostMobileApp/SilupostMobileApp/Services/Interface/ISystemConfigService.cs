using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using SilupostMobileApp.BindingModels;
using SilupostMobileApp.Common;
using SilupostMobileApp.Models;

namespace SilupostMobileApp.Services.Interface
{
    public interface ISystemConfigService
    {
        Task<SilupostServerStatusEnums> GetServerStatus();
    }
}

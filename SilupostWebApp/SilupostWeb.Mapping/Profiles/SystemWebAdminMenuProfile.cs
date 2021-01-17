using AutoMapper;
using SilupostWeb.Data.Entity;
using SilupostWeb.Domain.BindingModel;
using SilupostWeb.Domain.ViewModel;
using System.Collections.Generic;

namespace SilupostWeb.Mapping.Profiles
{
    public class SystemWebAdminMenuProfile : Profile
    {
        public SystemWebAdminMenuProfile()
        {
            this.IgnoreUnmapped();
            CreateMap<SystemWebAdminMenuModel, SystemWebAdminMenuViewModel>();
            CreateMap<SystemWebAdminModuleModel, SystemWebAdminModuleViewModel>();
        }
    }
}

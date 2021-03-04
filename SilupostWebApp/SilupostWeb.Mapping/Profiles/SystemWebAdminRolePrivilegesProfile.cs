using AutoMapper;
using SilupostWeb.Data.Entity;
using SilupostWeb.Domain.BindingModel;
using SilupostWeb.Domain.ViewModel;
using System.Collections.Generic;

namespace SilupostWeb.Mapping.Profiles
{
    public class SystemWebAdminRolePrivilegesProfile : Profile
    {
        public SystemWebAdminRolePrivilegesProfile()
        {
            this.IgnoreUnmapped();
            CreateMap<SystemWebAdminRolePrivilegesModel, SystemWebAdminRolePrivilegesViewModel>();
        }
    }
}

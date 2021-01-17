using AutoMapper;
using SilupostWeb.Data.Entity;
using SilupostWeb.Domain.BindingModel;
using SilupostWeb.Domain.ViewModel;
using System.Collections.Generic;

namespace SilupostWeb.Mapping.Profiles
{
    public class SystemWebAdminMenuRolesProfile : Profile
    {
        public SystemWebAdminMenuRolesProfile()
        {
            this.IgnoreUnmapped();
            CreateMap<SystemWebAdminMenuRolesModel, SystemWebAdminMenuRolesViewModel>();
        }
    }
}

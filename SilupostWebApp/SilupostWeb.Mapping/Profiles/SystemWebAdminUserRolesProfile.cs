using AutoMapper;
using SilupostWeb.Data.Entity;
using SilupostWeb.Domain.BindingModel;
using SilupostWeb.Domain.ViewModel;
using System.Collections.Generic;

namespace SilupostWeb.Mapping.Profiles
{
    public class SystemWebAdminUserRolesProfile : Profile
    {
        public SystemWebAdminUserRolesProfile()
        {
            this.IgnoreUnmapped();
            CreateMap<SystemWebAdminUserRolesModel, SystemWebAdminUserRolesViewModel>();
            CreateMap<SystemWebAdminUserRolesBindingModel, SystemWebAdminUserRolesViewModel>()
                .ForPath(dest => dest.SystemWebAdminRole, opt => opt.MapFrom(src =>
                    new SystemWebAdminRoleModel() { SystemWebAdminRoleId = src.SystemWebAdminRoleId }))
                .ForPath(dest => dest.SystemRecordManager, opt => opt.MapFrom(src =>
                    new SystemRecordManagerModel()));
        }
    }
}

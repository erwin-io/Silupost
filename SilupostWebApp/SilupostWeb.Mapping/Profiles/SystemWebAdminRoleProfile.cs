using AutoMapper;
using SilupostWeb.Data.Entity;
using SilupostWeb.Domain.BindingModel;
using SilupostWeb.Domain.ViewModel;
using System.Collections.Generic;

namespace SilupostWeb.Mapping.Profiles
{
    public class SystemWebAdminRoleProfile : Profile
    {
        public SystemWebAdminRoleProfile()
        {
            this.IgnoreUnmapped();
            CreateMap<SystemWebAdminRoleModel, SystemWebAdminRoleViewModel>();
            CreateMap<SystemWebAdminRoleBindingModel, SystemWebAdminRoleModel>()
                .ForPath(dest => dest.SystemRecordManager, opt => opt.MapFrom(src =>
                    new SystemRecordManagerModel()));
            CreateMap<UpdateSystemWebAdminRoleBindingModel, SystemWebAdminRoleModel>()
                .ForPath(dest => dest.SystemRecordManager, opt => opt.MapFrom(src =>
                    new SystemRecordManagerModel()));
        }
    }
}

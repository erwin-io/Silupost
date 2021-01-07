using AutoMapper;
using SilupostWeb.Data.Entity;
using SilupostWeb.Domain.BindingModel;
using SilupostWeb.Domain.ViewModel;
using System.Collections.Generic;

namespace POSWeb.POS.Mapping.Profiles
{
    public class SystemMenuRoleProfile : Profile
    {
        public SystemMenuRoleProfile()
        {
            this.IgnoreUnmapped();
            CreateMap<SystemMenuRoleModel, SystemMenuRoleViewModel>();
            CreateMap<SystemMenuRoleBindingModel, SystemMenuRoleModel>()
                .ForPath(dest => dest.SystemRole, opt => opt.MapFrom(src =>
                new SystemRoleModel
                {
                    SystemRoleId = src.SystemRoleId
                }))
                .ForPath(dest => dest.SystemMenu, opt => opt.MapFrom(src =>
                new SystemMenuModel
                {
                    SystemMenuId = src.SystemMenuId
                }))
                .ForPath(dest => dest.CreatedBy, opt => opt.MapFrom(src =>
                new SystemRecordManagerModel
                {
                    SystemUserId = src.CreatedBy
                }))
                .ForPath(dest => dest.Location, opt => opt.MapFrom(src => new LocationModel() { LocationId = src.LocationId }));
        }
    }
}

using AutoMapper;
using SilupostWeb.Data.Entity;
using SilupostWeb.Domain.BindingModel;
using SilupostWeb.Domain.ViewModel;
using System.Collections.Generic;

namespace SilupostWeb.Mapping.Profiles
{
    public class SystemConfigProfile : Profile
    {
        public SystemConfigProfile()
        {
            this.IgnoreUnmapped();
            CreateMap<SystemConfigModel, SystemConfigViewModel>();
            CreateMap<SystemConfigBindingModel, SystemConfigModel>()
                .ForPath(dest => dest.SystemConfigType, opt => opt.MapFrom(src =>
                    new SystemConfigTypeModel() { SystemConfigTypeId = src.SystemConfigTypeId }));
            CreateMap<UpdateSystemConfigBindingModel, SystemConfigModel>()
                .ForPath(dest => dest.SystemConfigType, opt => opt.MapFrom(src =>
                    new SystemConfigTypeModel() { SystemConfigTypeId = src.SystemConfigTypeId }));
        }
    }
}

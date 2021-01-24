using AutoMapper;
using SilupostWeb.Data.Entity;
using SilupostWeb.Domain.BindingModel;
using SilupostWeb.Domain.ViewModel;
using System.Collections.Generic;

namespace SilupostWeb.Mapping.Profiles
{
    public class EnforcementTypeProfile : Profile
    {
        public EnforcementTypeProfile()
        {
            this.IgnoreUnmapped();
            CreateMap<EnforcementTypeModel, EnforcementTypeViewModel>();
            CreateMap<CreateEnforcementTypeBindingModel, EnforcementTypeModel>()
                .ForPath(dest => dest.SystemRecordManager, opt => opt.MapFrom(src =>
                    new SystemRecordManagerModel()));
            CreateMap<UpdateEnforcementTypeBindingModel, EnforcementTypeModel>()
                .ForPath(dest => dest.SystemRecordManager, opt => opt.MapFrom(src =>
                    new SystemRecordManagerModel()));
        }
    }
}

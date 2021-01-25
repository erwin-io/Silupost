using AutoMapper;
using SilupostWeb.Data.Entity;
using SilupostWeb.Domain.BindingModel;
using SilupostWeb.Domain.ViewModel;
using System.Collections.Generic;

namespace SilupostWeb.Mapping.Profiles
{
    public class EnforcementStationProfile : Profile
    {
        public EnforcementStationProfile()
        {
            this.IgnoreUnmapped();
            CreateMap<EnforcementStationModel, EnforcementStationViewModel>();
            CreateMap<CreateEnforcementStationBindingModel, EnforcementStationModel>()
                .ForPath(dest => dest.SystemRecordManager, opt => opt.MapFrom(src =>
                    new SystemRecordManagerModel()));
            CreateMap<UpdateEnforcementStationBindingModel, EnforcementStationModel>()
                .ForPath(dest => dest.SystemRecordManager, opt => opt.MapFrom(src =>
                    new SystemRecordManagerModel()));
        }
    }
}

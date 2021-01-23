using AutoMapper;
using SilupostWeb.Data.Entity;
using SilupostWeb.Domain.BindingModel;
using SilupostWeb.Domain.ViewModel;
using System.Collections.Generic;

namespace SilupostWeb.Mapping.Profiles
{
    public class CrimeIncidentTypeProfile : Profile
    {
        public CrimeIncidentTypeProfile()
        {
            this.IgnoreUnmapped();
            CreateMap<CrimeIncidentTypeModel, CrimeIncidentTypeViewModel>();
            CreateMap<CreateCrimeIncidentTypeBindingModel, CrimeIncidentTypeModel>()
                .ForPath(dest => dest.SystemRecordManager, opt => opt.MapFrom(src =>
                    new SystemRecordManagerModel()));
            CreateMap<UpdateCrimeIncidentTypeBindingModel, CrimeIncidentTypeModel>()
                .ForPath(dest => dest.SystemRecordManager, opt => opt.MapFrom(src =>
                    new SystemRecordManagerModel()));
        }
    }
}

using AutoMapper;
using SilupostWeb.Data.Entity;
using SilupostWeb.Domain.BindingModel;
using SilupostWeb.Domain.ViewModel;
using System.Collections.Generic;

namespace SilupostWeb.Mapping.Profiles
{
    public class CrimeIncidentCategoryProfile : Profile
    {
        public CrimeIncidentCategoryProfile()
        {
            this.IgnoreUnmapped();
            CreateMap<CrimeIncidentCategoryModel, CrimeIncidentCategoryViewModel>();
            CreateMap<CrimeIncidentCategoryBindingModel, CrimeIncidentCategoryModel>()
                .ForPath(dest => dest.CrimeIncidentType, opt => opt.MapFrom(src =>
                    new CrimeIncidentTypeModel()
                    {
                        CrimeIncidentTypeId = src.CrimeIncidentTypeId
                    }))
                .ForPath(dest => dest.SystemRecordManager, opt => opt.MapFrom(src =>
                    new SystemRecordManagerModel()));
            CreateMap<UpdateCrimeIncidentCategoryBindingModel, CrimeIncidentCategoryModel>()
                .ForPath(dest => dest.CrimeIncidentType, opt => opt.MapFrom(src =>
                    new CrimeIncidentTypeModel()
                    {
                        CrimeIncidentTypeId = src.CrimeIncidentTypeId
                    }))
                .ForPath(dest => dest.SystemRecordManager, opt => opt.MapFrom(src =>
                    new SystemRecordManagerModel()));
        }
    }
}

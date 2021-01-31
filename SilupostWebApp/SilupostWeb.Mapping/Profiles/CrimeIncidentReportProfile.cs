using AutoMapper;
using SilupostWeb.Data.Entity;
using SilupostWeb.Domain.BindingModel;
using SilupostWeb.Domain.ViewModel;
using System.Collections.Generic;

namespace SilupostWeb.Mapping.Profiles
{
    public class CrimeIncidentReportProfile : Profile
    {
        public CrimeIncidentReportProfile()
        {
            this.IgnoreUnmapped();
            CreateMap<CrimeIncidentReportModel, CrimeIncidentReportViewModel>();
            CreateMap<CreateCrimeIncidentReportBindingModel, CrimeIncidentReportModel>()
                .ForPath(dest => dest.CrimeIncidentCategory, opt => opt.MapFrom(src =>
                    new CrimeIncidentCategoryModel()
                    {
                        CrimeIncidentCategoryId = src.CrimeIncidentCategoryId
                    }))
                .ForPath(dest => dest.PostedBySystemUser, opt => opt.MapFrom(src =>
                    new SystemUserModel()
                    {
                        SystemUserId = src.PostedBySystemUserId
                    }))
                .ForPath(dest => dest.SystemRecordManager, opt => opt.MapFrom(src =>
                    new SystemRecordManagerModel()));
            CreateMap<UpdateCrimeIncidentReportBindingModel, CrimeIncidentReportModel>()
                .ForPath(dest => dest.CrimeIncidentCategory, opt => opt.MapFrom(src =>
                    new CrimeIncidentCategoryModel()
                    {
                        CrimeIncidentCategoryId = src.CrimeIncidentCategoryId
                    }))
                .ForPath(dest => dest.SystemRecordManager, opt => opt.MapFrom(src =>
                    new SystemRecordManagerModel()));
        }
    }
}

using AutoMapper;
using SilupostWeb.Data.Entity;
using SilupostWeb.Domain.BindingModel;
using SilupostWeb.Domain.ViewModel;
using System.Collections.Generic;

namespace SilupostWeb.Mapping.Profiles
{
    public class EnforcementReportValidationProfile : Profile
    {
        public EnforcementReportValidationProfile()
        {
            this.IgnoreUnmapped();
            CreateMap<EnforcementReportValidationModel, EnforcementReportValidationViewModel>();
            CreateMap<CreateEnforcementReportValidationBindingModel, EnforcementReportValidationModel>()
                .ForPath(dest => dest.CrimeIncidentReport, opt => opt.MapFrom(src =>
                    new CrimeIncidentReportModel()
                    {
                        CrimeIncidentReportId = src.CrimeIncidentReportId
                    }))
                .ForPath(dest => dest.EnforcementUnit, opt => opt.MapFrom(src =>
                    new EnforcementUnitModel()
                    {
                        EnforcementUnitId = src.EnforcementUnitId
                    }))
                .ForPath(dest => dest.SystemRecordManager, opt => opt.MapFrom(src =>
                    new SystemRecordManagerModel()));
            CreateMap<UpdateEnforcementReportValidationBindingModel, EnforcementReportValidationModel>()
                .ForPath(dest => dest.EnforcementUnit, opt => opt.MapFrom(src =>
                    new EnforcementUnitModel()
                    {
                        EnforcementUnitId = src.EnforcementUnitId
                    }))
                .ForPath(dest => dest.ReportValidationStatus, opt => opt.MapFrom(src =>
                    new ReportValidationStatusModel()
                    {
                        ReportValidationStatusId = src.ReportValidationStatusId
                    }))
                .ForPath(dest => dest.SystemRecordManager, opt => opt.MapFrom(src =>
                    new SystemRecordManagerModel()));
        }
    }
}

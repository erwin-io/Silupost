using AutoMapper;
using SilupostWeb.Data.Entity;
using SilupostWeb.Domain.BindingModel;
using SilupostWeb.Domain.ViewModel;
using System.Collections.Generic;

namespace SilupostWeb.Mapping.Profiles
{
    public class CrimeIncidentReportMediaProfile : Profile
    {
        public CrimeIncidentReportMediaProfile()
        {
            this.IgnoreUnmapped();
            CreateMap<CrimeIncidentReportMediaModel, CrimeIncidentReportMediaViewModel>();
            CreateMap<CreateCrimeIncidentReportMediaBindingModel, CrimeIncidentReportMediaModel>()
                .ForPath(dest => dest.DocReportMediaType, opt => opt.MapFrom(src =>
                    new DocReportMediaTypeModel()
                    {
                        DocReportMediaTypeId = src.DocReportMediaTypeId
                    }))
                .ForPath(dest => dest.CrimeIncidentReport, opt => opt.MapFrom(src =>
                    new CrimeIncidentReportModel()
                    {
                        CrimeIncidentReportId = src.CrimeIncidentReportId
                    }));
            CreateMap<UpdateCrimeIncidentReportMediaBindingModel, CrimeIncidentReportMediaModel>()
                .ForPath(dest => dest.DocReportMediaType, opt => opt.MapFrom(src =>
                    new DocReportMediaTypeModel()
                    {
                        DocReportMediaTypeId = src.DocReportMediaTypeId
                    }))
                .ForPath(dest => dest.File, opt => opt.MapFrom(src =>
                    new FileModel()
                    {
                        FileId = src.FileId,
                        FileName = src.File.FileName,
                        MimeType = src.File.MimeType,
                        FileSize = src.File.FileSize,
                        FileContent = src.File.FileContent,
                        SystemRecordManager = new SystemRecordManagerModel() { },
                    }));
        }
    }
}

using AutoMapper;
using SilupostWeb.Data.Entity;
using SilupostWeb.Domain.BindingModel;
using SilupostWeb.Domain.ViewModel;
using System.Collections.Generic;

namespace SilupostWeb.Mapping.Profiles
{
    public class EnforcementUnitProfile : Profile
    {
        public EnforcementUnitProfile()
        {
            this.IgnoreUnmapped();
            CreateMap<EnforcementUnitModel, EnforcementUnitViewModel>();
            CreateMap<CreateEnforcementUnitBindingModel, EnforcementUnitModel>()
                .ForPath(dest => dest.LegalEntity, opt => opt.MapFrom(src =>
                    new LegalEntityModel
                    {
                        FirstName = src.FirstName,
                        LastName = src.LastName,
                        MiddleName = src.MiddleName,
                        EmailAddress = src.EmailAddress,
                        MobileNumber = src.MobileNumber,
                        BirthDate = src.BirthDate,
                        Gender = new EntityGenderModel() { GenderId = src.GenderId },
                    }))
                .ForPath(dest => dest.LegalEntity.LegalEntityAddress, opt => opt.MapFrom(src => src.LegalEntityAddress))
                .ForPath(dest => dest.EnforcementType, opt => opt.MapFrom(src =>
                    new EnforcementTypeModel
                    {
                        EnforcementTypeId = src.EnforcementTypeId
                    }))
                .ForPath(dest => dest.EnforcementStation, opt => opt.MapFrom(src =>
                    new EnforcementStationModel
                    {
                        EnforcementStationId = src.EnforcementStationId
                    }))
                .ForPath(dest => dest.SystemRecordManager, opt => opt.MapFrom(src =>
                    new SystemRecordManagerModel()));
            CreateMap<UpdateEnforcementUnitBindingModel, EnforcementUnitModel>()
                .ForPath(dest => dest.LegalEntity, opt => opt.MapFrom(src =>
                    new LegalEntityModel
                    {
                        LegalEntityId = src.LegalEntityId,
                        FirstName = src.FirstName,
                        LastName = src.LastName,
                        MiddleName = src.MiddleName,
                        EmailAddress = src.EmailAddress,
                        MobileNumber = src.MobileNumber,
                        BirthDate = src.BirthDate,
                        Age = 0,
                        Gender = new EntityGenderModel() { GenderId = src.GenderId }
                    }))
                .ForPath(dest => dest.EnforcementType, opt => opt.MapFrom(src =>
                    new EnforcementTypeModel
                    {
                        EnforcementTypeId = src.EnforcementTypeId
                    }))
                .ForPath(dest => dest.EnforcementStation, opt => opt.MapFrom(src =>
                    new EnforcementStationModel
                    {
                        EnforcementStationId = src.EnforcementStationId
                    }))
                .ForPath(dest => dest.SystemRecordManager, opt => opt.MapFrom(src =>
                    new SystemRecordManagerModel()));

        }
    }
}

using AutoMapper;
using SilupostWeb.Data.Entity;
using SilupostWeb.Domain.BindingModel;
using SilupostWeb.Domain.ViewModel;
using System.Collections.Generic;

namespace SilupostWeb.Mapping.Profiles
{
    public class SystemUserProfile : Profile
    {
        public SystemUserProfile()
        {
            this.IgnoreUnmapped();
            CreateMap<SystemUserModel, SystemUserViewModel>();
            CreateMap<CreateSystemUserBindingModel, SystemUserModel>()
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
                .ForPath(dest => dest.SystemUserType, opt => opt.MapFrom(src =>
                    new SystemUserTypeModel
                    {
                        SystemUserTypeId = src.SystemUserTypeId
                    }))
                .ForPath(dest => dest.SystemRecordManager, opt => opt.MapFrom(src =>
                    new SystemRecordManagerModel()))
                .ForPath(dest => dest.SystemWebAdminUserRoles, opt => opt.MapFrom(src =>
                    new List<SystemWebAdminUserRolesModel>()));
            CreateMap<UpdateSystemUserBindingModel, SystemUserModel>()
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
                .ForPath(dest => dest.SystemRecordManager, opt => opt.MapFrom(src =>
                    new SystemRecordManagerModel()))
                .ForPath(dest => dest.SystemWebAdminUserRoles, opt => opt.MapFrom(src =>
                    new List<SystemWebAdminUserRolesModel>()));


            CreateMap<SystemUserTypeModel, SystemUserTypeViewModel>();
        }
    }
}

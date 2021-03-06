﻿using AutoMapper;
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
                .ForPath(dest => dest.EnforcementUnit, opt => opt.MapFrom(src =>
                    new EnforcementUnitModel
                    {
                        EnforcementType = new EnforcementTypeModel() { EnforcementTypeId = src.EnforcementTypeId },
                        EnforcementStation = new EnforcementStationModel() { EnforcementStationId = src.EnforcementStationId },
                        LegalEntity = new LegalEntityModel
                        {
                            FirstName = src.FirstName,
                            LastName = src.LastName,
                            MiddleName = src.MiddleName,
                            EmailAddress = src.EmailAddress,
                            MobileNumber = src.MobileNumber,
                            BirthDate = src.BirthDate,
                            Gender = new EntityGenderModel() { GenderId = src.GenderId },
                        }
                    }))
                .ForPath(dest => dest.EnforcementStation, opt => opt.MapFrom(src =>
                    new EnforcementStationModel
                    {
                        EnforcementStationId = src.EnforcementStationId
                    }))
                .ForPath(dest => dest.SystemUserConfig, opt => opt.MapFrom(src =>
                    new SystemUserConfigModel
                    {
                        SystemUser = new SystemUserModel()
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

            CreateMap<CreateAccountSystemUserBindingModel, SystemUserModel>()
                .ForPath(dest => dest.LegalEntity, opt => opt.MapFrom(src =>
                    new LegalEntityModel
                    {
                        FirstName = src.FirstName,
                        LastName = src.LastName,
                        MiddleName = src.MiddleName,
                        EmailAddress = src.EmailAddress,
                        BirthDate = src.BirthDate,
                        Gender = new EntityGenderModel() { GenderId = src.GenderId },
                    }))
                .ForPath(dest => dest.SystemUserConfig, opt => opt.MapFrom(src =>
                    new SystemUserConfigModel
                    {
                        SystemUser = new SystemUserModel()
                    }))
                .ForPath(dest => dest.SystemUserType, opt => opt.MapFrom(src =>
                    new SystemUserTypeModel()))
                .ForPath(dest => dest.ProfilePicture, opt => opt.MapFrom(src =>
                    new FileModel()));


            CreateMap<CreateWebAccountSystemUserBindingModel, SystemUserModel>()
                .ForPath(dest => dest.LegalEntity, opt => opt.MapFrom(src =>
                    new LegalEntityModel
                    {
                        FirstName = src.FirstName,
                        LastName = src.LastName,
                        MiddleName = src.MiddleName,
                        EmailAddress = src.EmailAddress,
                        Gender = new EntityGenderModel() { GenderId = src.GenderId },
                    }))
                .ForPath(dest => dest.EnforcementStation, opt => opt.MapFrom(src =>
                    new EnforcementStationModel
                    {
                        EnforcementStationGuestCode = src.EnforcementStationGuestCode
                    }))
                .ForPath(dest => dest.SystemUserConfig, opt => opt.MapFrom(src =>
                    new SystemUserConfigModel
                    {
                        SystemUser = new SystemUserModel()
                    }))
                .ForPath(dest => dest.SystemUserConfig, opt => opt.MapFrom(src =>
                    new SystemUserConfigModel
                    {
                        SystemUser = new SystemUserModel()
                    }))
                .ForPath(dest => dest.SystemUserType, opt => opt.MapFrom(src =>
                    new SystemUserTypeModel()))
                .ForPath(dest => dest.ProfilePicture, opt => opt.MapFrom(src =>
                    new FileModel()));

            CreateMap<UpdateSystemUserBindingModel, SystemUserModel>()
                .ForPath(dest => dest.LegalEntity, opt => opt.MapFrom(src =>
                    new LegalEntityModel
                    {
                        FirstName = src.FirstName,
                        LastName = src.LastName,
                        MiddleName = src.MiddleName,
                        EmailAddress = src.EmailAddress,
                        MobileNumber = src.MobileNumber,
                        BirthDate = src.BirthDate,
                        Age = 0,
                        Gender = new EntityGenderModel() { GenderId = src.GenderId }
                    }))
                .ForPath(dest => dest.EnforcementUnit, opt => opt.MapFrom(src =>
                    new EnforcementUnitModel
                    {
                        EnforcementType = new EnforcementTypeModel() { EnforcementTypeId = src.EnforcementTypeId },
                        EnforcementStation = new EnforcementStationModel() { EnforcementStationId = src.EnforcementStationId },
                        LegalEntity = new LegalEntityModel
                        {
                            FirstName = src.FirstName,
                            LastName = src.LastName,
                            MiddleName = src.MiddleName,
                            EmailAddress = src.EmailAddress,
                            MobileNumber = src.MobileNumber,
                            BirthDate = src.BirthDate,
                            Gender = new EntityGenderModel() { GenderId = src.GenderId },
                        }
                    }))
                .ForPath(dest => dest.EnforcementStation, opt => opt.MapFrom(src =>
                    new EnforcementStationModel
                    {
                        EnforcementStationId = src.EnforcementStationId
                    }))
                .ForPath(dest => dest.SystemUserConfig, opt => opt.MapFrom(src =>
                    new SystemUserConfigModel
                    {
                        SystemUser = new SystemUserModel()
                    }))
                .ForPath(dest => dest.SystemRecordManager, opt => opt.MapFrom(src =>
                    new SystemRecordManagerModel()))
                .ForPath(dest => dest.SystemWebAdminUserRoles, opt => opt.MapFrom(src =>
                    new List<SystemWebAdminUserRolesModel>()));


            CreateMap<SystemUserTypeModel, SystemUserTypeViewModel>();
            CreateMap<UpdateSystemUserNameBindingModel, SystemUserModel>()
                .ForPath(dest => dest.UserName, opt => opt.MapFrom(src => src.UserName))
                .ForPath(dest => dest.SystemRecordManager, opt => opt.MapFrom(src =>
                    new SystemRecordManagerModel()));
            CreateMap<UpdateSystemResetPasswordBindingModel, SystemUserModel>()
                .ForPath(dest => dest.Password, opt => opt.MapFrom(src => src.NewPassword))
                .ForPath(dest => dest.SystemRecordManager, opt => opt.MapFrom(src =>
                    new SystemRecordManagerModel()));
            CreateMap<UpdateSystemPasswordBindingModel, SystemUserModel>()
                .ForPath(dest => dest.Password, opt => opt.MapFrom(src => src.NewPassword))
                .ForPath(dest => dest.SystemRecordManager, opt => opt.MapFrom(src =>
                    new SystemRecordManagerModel()));
            CreateMap<SystemUserStatusTrackerBindingModel, SystemUserModel>();
        }
    }
}

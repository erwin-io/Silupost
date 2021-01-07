using AutoMapper;
using POSWeb.POSAdmin.Data.Entity;
using POSWeb.POSAdmin.Domain.BindingModel;
using POSWeb.POSAdmin.Domain.ViewModel;
using System.Collections.Generic;

namespace POSWeb.POS.Mapping.Profiles
{
    public class SystemUserProfile : Profile
    {
        public SystemUserProfile()
        {
            this.IgnoreUnmapped();
            CreateMap<SystemUserModel, SystemUserViewModel>();
            CreateMap<CreateSystemUserBindingModel, SystemUserModel>()
                .ForPath(dest => dest.EntityInformation, opt => opt.MapFrom(src =>
                    new EntityInformationModel
                    {
                        FirstName = src.FirstName,
                        LastName = src.LastName,
                        MiddleName = src.MiddleName,
                        EmailAddress = src.EmailAddress,
                        BirthDate = src.BirthDate,
                        Gender = new EntityGenderTypeModel() {GenderId = src.GenderId },
                        CivilStatusType = new EntityCivilStatusTypeModel() { CivilStatusTypeId = src.CivilStatusTypeId },
                        Location = new LocationModel() { LocationId = src.LocationId }
                    }))
                .ForPath(dest=> dest.EntityInformation.Contact, opt => opt.MapFrom(src=> src.Contact))
                .ForPath(dest => dest.Location, opt => opt.MapFrom(src =>
                new LocationModel()
                {
                    LocationId = src.LocationId
                })); ;
            CreateMap<UpdateSystemUserBindingModel, SystemUserModel>()
                .ForPath(dest => dest.EntityInformation, opt => opt.MapFrom(src =>
                    new EntityInformationModel
                    {
                        FirstName = src.FirstName,
                        LastName = src.LastName,
                        MiddleName = src.MiddleName,
                        EmailAddress = src.EmailAddress,
                        BirthDate = src.BirthDate,
                        Gender = new EntityGenderTypeModel() { GenderId = src.GenderId },
                        CivilStatusType = new EntityCivilStatusTypeModel() { CivilStatusTypeId = src.CivilStatusTypeId }
                    }));
        }
    }
}

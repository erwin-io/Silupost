﻿using AutoMapper;
using SilupostWeb.Data.Entity;
using SilupostWeb.Domain.BindingModel;
using SilupostWeb.Domain.ViewModel;
using System.Collections.Generic;

namespace POSWeb.POS.Mapping.Profiles
{
    public class SystemRoleProfile : Profile
    {
        public SystemRoleProfile()
        {
            this.IgnoreUnmapped();
            CreateMap<SystemRoleModel, SystemRoleViewModel>();
            CreateMap<SystemRoleBindingModel, SystemRoleModel>()
                .ForPath(dest => dest.CreatedBy, opt => opt.MapFrom(src =>
                new SystemRecordManagerModel
                {
                    SystemUserId = null
                }))
                .ForPath(dest => dest.Location, opt => opt.MapFrom(src => 
                new LocationModel() 
                { 
                    LocationId = 0
                }));
            CreateMap<UpdateSystemRoleBindingModel, SystemRoleModel>()
                .ForPath(dest => dest.UpdatedBy, opt => opt.MapFrom(src =>
                new SystemRecordManagerModel
                {
                    SystemUserId = null
                }));
        }
    }
}

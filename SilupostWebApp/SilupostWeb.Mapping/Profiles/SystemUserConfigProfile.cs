﻿using AutoMapper;
using SilupostWeb.Data.Entity;
using SilupostWeb.Domain.BindingModel;
using SilupostWeb.Domain.ViewModel;
using System.Collections.Generic;

namespace SilupostWeb.Mapping.Profiles
{
    public class SystemUserConfigProfile : Profile
    {
        public SystemUserConfigProfile()
        {
            this.IgnoreUnmapped();
            CreateMap<SystemUserConfigModel, SystemUserConfigViewModel>();
            CreateMap<UpdateSystemUserConfigBindingModel, SystemUserConfigModel>();
        }
    }
}
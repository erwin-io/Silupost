using AutoMapper;
using SilupostWeb.Data.Entity;
using SilupostWeb.Domain.BindingModel;
using SilupostWeb.Domain.ViewModel;
using System.Collections.Generic;

namespace SilupostWeb.Mapping.Profiles
{
    public class SystemUserVerificationProfile : Profile
    {
        public SystemUserVerificationProfile()
        {
            this.IgnoreUnmapped();
            CreateMap<SystemUserVerificationModel, SystemUserVerificationViewModel>();
            CreateMap<SystemUserVerificationBindingModel, SystemUserVerificationModel>();
        }
    }
}

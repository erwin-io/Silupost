using AutoMapper;
using SilupostWeb.Data.Entity;
using SilupostWeb.Domain.BindingModel;
using SilupostWeb.Domain.ViewModel;
using System.Collections.Generic;

namespace SilupostWeb.Mapping.Profiles
{
    public class SystemTokenProfile : Profile
    {
        public SystemTokenProfile()
        {
            this.IgnoreUnmapped();
            CreateMap<SystemTokenModel, SystemRefreshTokenViewModel>()
                .ForPath(dest => dest.UserId, opt => opt.MapFrom(src => src.SystemUser.SystemUserId));
            CreateMap<SystemRefreshTokenBindingModel, SystemTokenModel>()
                .ForPath(dest => dest.SystemUser, opt => opt.MapFrom(src =>
                    new SystemUserModel
                    {
                        SystemUserId = src.UserId
                    }));
        }
    }
}

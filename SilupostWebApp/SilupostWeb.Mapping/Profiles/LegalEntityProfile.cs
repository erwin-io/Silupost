using AutoMapper;
using SilupostWeb.Data.Entity;
using SilupostWeb.Domain.BindingModel;
using SilupostWeb.Domain.ViewModel;
using System.Collections.Generic;

namespace SilupostWeb.Mapping.Profiles
{
    public class LegalEntityProfile : Profile
    {
        public LegalEntityProfile()
        {
            this.IgnoreUnmapped();
            CreateMap<LegalEntityModel, LegalEntityViewModel>();
            CreateMap<EntityGenderModel, EntityGenderViewModel>();
            CreateMap<EntityStatusModel, EntityStatusViewModel>();
        }
    }
}

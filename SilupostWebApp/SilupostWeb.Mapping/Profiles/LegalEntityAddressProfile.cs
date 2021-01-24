using AutoMapper;
using SilupostWeb.Data.Entity;
using SilupostWeb.Domain.BindingModel;
using SilupostWeb.Domain.ViewModel;
using System.Collections.Generic;

namespace SilupostWeb.Mapping.Profiles
{
    public class LegalEntityAddressProfile : Profile
    {
        public LegalEntityAddressProfile()
        {
            this.IgnoreUnmapped();
            CreateMap<LegalEntityAddressModel, LegalEntityAddressViewModel>();
            CreateMap<LegalEntityAddressBindingModel, LegalEntityAddressModel>();
            CreateMap<CreateLegalEntityAddressBindingModel, LegalEntityAddressModel>()
                .ForPath(dest => dest.LegalEntity, opt => opt.MapFrom(src =>
                    new LegalEntityModel
                    {
                        LegalEntityId = src.LegalEntityId
                    }));
            CreateMap<UpdateLegalEntityAddressBindingModel, LegalEntityAddressModel>();
        }
    }
}

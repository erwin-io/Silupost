using AutoMapper;
using SilupostWeb.Data.Entity;
using SilupostWeb.Domain.BindingModel;
using SilupostWeb.Domain.ViewModel;
using System.Collections.Generic;

namespace SilupostWeb.Mapping.Profiles
{
    public class LookupTableProfile : Profile
    {
        public LookupTableProfile()
        {
            this.IgnoreUnmapped();
            CreateMap<LookupModel, LookupViewModel>();
            CreateMap<LookupTableModel, LookupTableViewModel>();
        }
    }
}

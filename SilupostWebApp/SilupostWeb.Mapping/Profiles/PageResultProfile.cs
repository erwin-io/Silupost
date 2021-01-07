using AutoMapper;
using SilupostWeb.Data.Entity;
using SilupostWeb.Domain.BindingModel;
using SilupostWeb.Domain.ViewModel;
using System.Collections.Generic;

namespace POSWeb.POS.Mapping.Profiles
{
    public class PageResultProfile : Profile
    {
        public PageResultProfile()
        {
            this.IgnoreUnmapped();
            CreateMap<PageResultsModel<SystemMenuRoleModel>, PageResultsViewModel<SystemRoleViewModel>>();
        }
    }
}

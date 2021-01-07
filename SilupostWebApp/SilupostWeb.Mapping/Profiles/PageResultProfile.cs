using AutoMapper;
using POSWeb.POSAdmin.Data.Entity;
using POSWeb.POSAdmin.Domain.BindingModel;
using POSWeb.POSAdmin.Domain.ViewModel;
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

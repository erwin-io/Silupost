using SilupostWeb.Domain.BindingModel;
using SilupostWeb.Domain.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SilupostWeb.Facade.Interface
{
    public interface ISystemUserFacade
    {
        string Add(CreateSystemUserBindingModel model, string CreatedBy);
        PageResultsViewModel<SystemUserViewModel> GetPage(string Search, long SystemUserType, long PageNo, long PageSize, string OrderColumn, string OrderDir);
        SystemUserViewModel Find(string id);
        SystemUserViewModel Find(string Username, string Password);
        bool Remove(string id, string LastUpdatedBy);
        bool Update(UpdateSystemUserBindingModel model, string LastUpdatedBy);
    }
}

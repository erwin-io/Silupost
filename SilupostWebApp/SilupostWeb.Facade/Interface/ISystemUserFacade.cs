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
        string Add(CreateSystemUserBindingModel model);
        SystemUserViewModel Find(string id);
        SystemUserViewModel Find(string Username, string Password);

        bool SystemUserAccountApproval(SystemUserAccountApprovalBindingModel model);
    }
}

using POSWeb.POSAdmin.Domain.BindingModel;
using POSWeb.POSAdmin.Domain.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POSWeb.POSAdmin.Facade.Interface
{
    public interface IUserAuthFacade
    {
        SystemUserViewModel Find(string id);
        SystemUserViewModel Find(string Username, string Password);
    }
}

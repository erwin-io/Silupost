using SilupostWeb.Domain.BindingModel;
using SilupostWeb.Domain.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SilupostWeb.Facade.Interface
{
    public interface ISystemConfigFacade
    {
        SystemConfigViewModel Find(long id);
        bool Update(UpdateSystemConfigBindingModel model);
        List<SystemConfigViewModel> GetAll();
    }
}

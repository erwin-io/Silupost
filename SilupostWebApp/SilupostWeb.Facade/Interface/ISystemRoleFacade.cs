using SilupostWeb.Domain.BindingModel;
using SilupostWeb.Domain.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SilupostWeb.Facade.Interface
{
    public interface ISystemRoleFacade
    {
        string Add(SystemRoleBindingModel model, long LocationId, string CreatedBy);
        SystemRoleViewModel Find(string id);
        bool Remove(string id, string UpdatedBy);
        bool Update(UpdateSystemRoleBindingModel model, string UpdatedBy);
        PageResultsViewModel<SystemRoleViewModel> GetPage(string SystemRoleId, string Name, string CreatedAt, string UpdatedAt, int PageNo, int PageSize, long LocationId);
    }
}

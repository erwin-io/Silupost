using SilupostWeb.Domain.BindingModel;
using SilupostWeb.Domain.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SilupostWeb.Facade.Interface
{
    public interface IEnforcementUnitFacade
    {
        string Add(CreateEnforcementUnitBindingModel model, string CreatedBy);
        PageResultsViewModel<EnforcementUnitViewModel> GetPage(string Search, long PageNo, long PageSize, string OrderColumn, string OrderDir);
        EnforcementUnitViewModel Find(string id);
        bool Remove(string id, string LastUpdatedBy);
        bool Update(UpdateEnforcementUnitBindingModel model, string LastUpdatedBy);
    }
}

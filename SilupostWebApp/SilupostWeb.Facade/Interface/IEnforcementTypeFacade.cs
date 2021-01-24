using SilupostWeb.Domain.BindingModel;
using SilupostWeb.Domain.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SilupostWeb.Facade.Interface
{
    public interface IEnforcementTypeFacade
    {
        string Add(CreateEnforcementTypeBindingModel model, string CreatedBy);
        EnforcementTypeViewModel Find(string id);
        bool Remove(string id, string LastUpdatedBy);
        bool Update(UpdateEnforcementTypeBindingModel model, string LastUpdatedBy);
        PageResultsViewModel<EnforcementTypeViewModel> GetPage(string Search, int PageNo, int PageSize, string OrderColumn, string OrderDir);
    }
}

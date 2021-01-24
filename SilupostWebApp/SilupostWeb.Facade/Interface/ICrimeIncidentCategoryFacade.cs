using SilupostWeb.Domain.BindingModel;
using SilupostWeb.Domain.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SilupostWeb.Facade.Interface
{
    public interface ICrimeIncidentCategoryFacade
    {
        string Add(CrimeIncidentCategoryBindingModel model, string CreatedBy);
        CrimeIncidentCategoryViewModel Find(string id);
        bool Remove(string id, string LastUpdatedBy);
        bool Update(UpdateCrimeIncidentCategoryBindingModel model, string LastUpdatedBy);
        PageResultsViewModel<CrimeIncidentCategoryViewModel> GetPage(string CrimeIncidentTypeId, string Search, int PageNo, int PageSize, string OrderColumn, string OrderDir);
    }
}

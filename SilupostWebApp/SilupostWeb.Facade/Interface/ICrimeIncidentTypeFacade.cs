using SilupostWeb.Domain.BindingModel;
using SilupostWeb.Domain.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SilupostWeb.Facade.Interface
{
    public interface ICrimeIncidentTypeFacade
    {
        string Add(CreateCrimeIncidentTypeBindingModel model, string CreatedBy);
        CrimeIncidentTypeViewModel Find(string id);
        bool Remove(string id, string LastUpdatedBy);
        bool Update(UpdateCrimeIncidentTypeBindingModel model, string LastUpdatedBy);
        PageResultsViewModel<CrimeIncidentTypeViewModel> GetPage(string Search, int PageNo, int PageSize, string OrderColumn, string OrderDir);
    }
}

using SilupostWeb.Domain.BindingModel;
using SilupostWeb.Domain.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SilupostWeb.Facade.Interface
{
    public interface IEnforcementStationFacade
    {
        string Add(CreateEnforcementStationBindingModel model, string CreatedBy);
        EnforcementStationViewModel Find(string id);
        bool Remove(string id, string LastUpdatedBy);
        bool Update(UpdateEnforcementStationBindingModel model, string LastUpdatedBy);
        PageResultsViewModel<EnforcementStationViewModel> GetPage(string Search, int PageNo, int PageSize, string OrderColumn, string OrderDir);
    }
}

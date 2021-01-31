using SilupostWeb.Domain.BindingModel;
using SilupostWeb.Domain.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SilupostWeb.Facade.Interface
{
    public interface ICrimeIncidentReportMediaFacade
    {
        string Add(CreateCrimeIncidentReportMediaBindingModel model, string CreatedBy);
        CrimeIncidentReportMediaViewModel Find(string id);
        List<CrimeIncidentReportMediaViewModel> FindByCrimeIncidentReportId(string CrimeIncidentReportId);
        bool Remove(string id);
        bool Update(UpdateCrimeIncidentReportMediaBindingModel model,string LastUpdatedBy);
    }
}

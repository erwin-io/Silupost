using SilupostWeb.Domain.BindingModel;
using SilupostWeb.Domain.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SilupostWeb.Facade.Interface
{
    public interface ICrimeIncidentReportFacade
    {
        string Add(CreateCrimeIncidentReportBindingModel model, string CreatedBy);
        CrimeIncidentReportViewModel Find(string id);
        bool Remove(string id, string LastUpdatedBy);
        bool Update(UpdateCrimeIncidentReportBindingModel model, string LastUpdatedBy);
        PageResultsViewModel<CrimeIncidentReportViewModel> GetPage(string Search,
                                                                   bool IsAdvanceSearchMode,
                                                                   long ApprovalStatusId,
                                                                   string CrimeIncidentReportId,
                                                                   string CrimeIncidentCategoryName,
                                                                   string PostedByFullName,
                                                                   DateTime DateReportedFrom,
                                                                   DateTime DateReportedTo,
                                                                   DateTime PossibleDateFrom,
                                                                   DateTime PossibleDateTo,
                                                                   string PossibleTimeFrom,
                                                                   string PossibleTimeTo,
                                                                   string Description,
                                                                   string GeoStreet,
                                                                   string GeoDistrict,
                                                                   string GeoCityMun,
                                                                   string GeoProvince,
                                                                   string GeoCountry,
                                                                   int PageNo,
                                                                   int PageSize,
                                                                   string OrderColumn,
                                                                   string OrderDir);
    }
}

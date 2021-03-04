using SilupostWeb.Domain.BindingModel;
using SilupostWeb.Domain.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SilupostWeb.Facade.Interface
{
    public interface IEnforcementReportValidationFacade
    {
        string Add(CreateEnforcementReportValidationBindingModel model, string CreatedBy);
        EnforcementReportValidationViewModel Find(string id);
        bool Remove(string id, string LastUpdatedBy);
        bool Update(UpdateEnforcementReportValidationBindingModel model, string LastUpdatedBy);

        PageResultsViewModel<EnforcementReportValidationViewModel> GetTablePageByCrimeIncidentReportId(string Search,
                                               bool IsAdvanceSearchMode,
                                               string CrimeIncidentReportId,
                                               string EnforcementUnitName,
                                               DateTime DateSubmittedFrom,
                                               DateTime DateSubmittedTo,
                                               string ReportValidationStatusId,
                                               int PageNo,
                                               int PageSize,
                                               string OrderColumn,
                                               string OrderDir);
        PageResultsViewModel<EnforcementReportValidationViewModel> GetPageByEnforcementSystemUserId(string Search,
                                               bool IsAdvanceSearchMode,
                                               string SystemUserId,
                                               string CrimeIncidentCategoryName,
                                               DateTime DateSubmittedFrom,
                                               DateTime DateSubmittedTo,
                                               string ReportValidationStatusId,
                                               int PageNo,
                                               int PageSize,
                                               string OrderColumn,
                                               string OrderDir);
        PageResultsViewModel<EnforcementReportValidationViewModel> GetPageByEnforcementStationId(string Search,
                                               bool IsAdvanceSearchMode,
                                               string EnforcementStationId,
                                               string CrimeIncidentCategoryName,
                                               DateTime DateSubmittedFrom,
                                               DateTime DateSubmittedTo,
                                               string ReportValidationStatusId,
                                               int PageNo,
                                               int PageSize,
                                               string OrderColumn,
                                               string OrderDir);
    }
}

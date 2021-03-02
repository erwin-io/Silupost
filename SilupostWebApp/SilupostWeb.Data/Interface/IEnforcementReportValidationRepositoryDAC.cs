using SilupostWeb.Data.Core;
using SilupostWeb.Data.Entity;
using System;
using System.Collections.Generic;

namespace SilupostWeb.Data.Interface
{
    public interface IEnforcementReportValidationRepositoryDAC : IRepository<EnforcementReportValidationModel>
    {
        List<EnforcementReportValidationModel> GetTablePageByCrimeIncidentReportId(string Search,
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
        bool Remove(string id, string LastUpdatedBy);
        List<EnforcementReportValidationModel> GetPageByEnforcementSystemUserId(string Search,
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
    }
}

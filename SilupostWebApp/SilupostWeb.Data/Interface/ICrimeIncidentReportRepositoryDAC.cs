using SilupostWeb.Data.Core;
using SilupostWeb.Data.Entity;
using System;
using System.Collections.Generic;

namespace SilupostWeb.Data.Interface
{
    public interface ICrimeIncidentReportRepositoryDAC : IRepository<CrimeIncidentReportModel>
    {
        List<CrimeIncidentReportModel> GetPage(string Search,
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
        bool Remove(string id, string LastUpdatedBy);
        List<CrimeIncidentReportModel> GetPageByPostedBySystemUserId(string PostedBySystemUserId, int PageNo, int PageSize);
    }
}

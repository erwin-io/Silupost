using SilupostWeb.Data.Core;
using SilupostWeb.Data.Entity;
using System.Collections.Generic;

namespace SilupostWeb.Data.Interface
{
    public interface ICrimeIncidentCategoryRepositoryDAC : IRepository<CrimeIncidentCategoryModel>
    {
        List<CrimeIncidentCategoryModel> GetPage(string CrimeIncidentTypeId, string Search,  int PageNo, int PageSize, string OrderColumn, string OrderDir);
        bool Remove(string id, string LastUpdatedBy);
    }
}

using SilupostWeb.Data.Core;
using SilupostWeb.Data.Entity;
using System.Collections.Generic;

namespace SilupostWeb.Data.Interface
{
    public interface IEnforcementUnitRepositoryDAC : IRepository<EnforcementUnitModel>
    {
        List<EnforcementUnitModel> GetPage(string Search, long PageNo, long PageSize, string OrderColumn, string OrderDir);
        bool Remove(string id, string LastUpdatedBy);
    }
}

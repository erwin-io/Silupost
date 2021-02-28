using SilupostWeb.Data.Core;
using SilupostWeb.Data.Entity;
using System.Collections.Generic;

namespace SilupostWeb.Data.Interface
{
    public interface IEnforcementUnitRepositoryDAC : IRepository<EnforcementUnitModel>
    {
        EnforcementUnitModel FindLegalEntityId(string LegalEntityId);
        List<EnforcementUnitModel> GetPage(string Search, long PageNo, long PageSize, string OrderColumn, string OrderDir);
        bool Remove(string id, string LastUpdatedBy);
    }
}

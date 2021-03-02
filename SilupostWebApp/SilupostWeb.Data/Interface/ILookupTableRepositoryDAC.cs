using SilupostWeb.Data.Core;
using SilupostWeb.Data.Entity;
using System.Collections.Generic;

namespace SilupostWeb.Data.Interface
{
    public interface ILookupTableRepositoryDAC : IRepository<LookupTableModel>
    {
        List<LookupTableModel> FindLookupByTableNames(string TableNames);
        List<LookupTableModel> FindEnforcementUnitByEnforcementStationId(string EnforcementStationId);
    }
}

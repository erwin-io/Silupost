using SilupostWeb.Data.Core;
using SilupostWeb.Data.Entity;
using System.Collections.Generic;

namespace SilupostWeb.Data.Interface
{
    public interface ISystemWebAdminRolePrivilegesRepositoryDAC : IRepository<SystemWebAdminRolePrivilegesModel>
    {
        SystemWebAdminRolePrivilegesModel FindBySystemWebAdminPrivilegeIdAndSystemWebAdminRoleId(long SystemWebAdminPrivilegeId, string SystemWebAdminRoleId);
        List<SystemWebAdminRolePrivilegesModel> FindBySystemWebAdminRoleId(string SystemWebAdminRoleId);
        bool Remove(string id, string LastUpdatedBy);
    }
}

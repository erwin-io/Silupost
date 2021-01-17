using SilupostWeb.Data.Core;
using SilupostWeb.Data.Entity;
using System.Collections.Generic;

namespace SilupostWeb.Data.Interface
{
    public interface ISystemWebAdminMenuRolesRepositoryDAC : IRepository<SystemWebAdminMenuRolesModel>
    {
        SystemWebAdminMenuRolesModel FindBySystemWebAdminMenuIdAndSystemWebAdminRoleId(long SystemWebAdminMenuId, string SystemWebAdminRoleId);
        List<SystemWebAdminMenuRolesModel> FindBySystemWebAdminRoleId(string SystemWebAdminRoleId);
        List<SystemWebAdminMenuRolesModel> FindBySystemWebAdminRoleIdandSystemWebAdminModuleId(string SystemWebAdminRoleId, long SystemWebAdminModuleId);
        bool Remove(string id, string LastUpdatedBy);
    }
}

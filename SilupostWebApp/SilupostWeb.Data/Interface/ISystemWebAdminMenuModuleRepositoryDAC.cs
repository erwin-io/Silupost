using SilupostWeb.Data.Core;
using SilupostWeb.Data.Entity;
using System.Collections.Generic;

namespace SilupostWeb.Data.Interface
{
    public interface ISystemWebAdminMenuModuleRepositoryDAC : IRepository<SystemWebAdminModuleModel>
    {
        SystemWebAdminModuleModel FindByMenuId(long menuId);
    }
}

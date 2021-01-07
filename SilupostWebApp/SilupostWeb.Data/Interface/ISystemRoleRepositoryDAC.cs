using POSWeb.POSAdmin.Data.Core;
using POSWeb.POSAdmin.Data.Entity;

namespace POSWeb.POSAdmin.Data.Interface
{
    public interface ISystemRoleRepositoryDAC : IRepository<SystemRoleModel>
    {
        PageResultsModel<SystemRoleModel> GetPage(string SystemRoleId, string Name, string CreatedAt, string UpdatedAt, int PageNo, int PageSize, long LocationId);
        bool Remove(string id, string UpdatedBy);
    }
}

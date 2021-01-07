using SilupostWeb.Data.Core;
using SilupostWeb.Data.Entity;

namespace SilupostWeb.Data.Interface
{
    public interface ISystemRoleRepositoryDAC : IRepository<SystemRoleModel>
    {
        PageResultsModel<SystemRoleModel> GetPage(string SystemRoleId, string Name, string CreatedAt, string UpdatedAt, int PageNo, int PageSize, long LocationId);
        bool Remove(string id, string UpdatedBy);
    }
}

using POSWeb.POSAdmin.Data.Core;
using POSWeb.POSAdmin.Data.Entity;

namespace POSWeb.POSAdmin.Data.Interface
{
    public interface ISystemUserRepository : IRepository<SystemUserModel>
    {
        SystemUserModel Find(string Username, string Password);
    }
}

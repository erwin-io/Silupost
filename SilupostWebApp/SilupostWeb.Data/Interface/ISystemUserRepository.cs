using SilupostWeb.Data.Core;
using SilupostWeb.Data.Entity;

namespace SilupostWeb.Data.Interface
{
    public interface ISystemUserRepository : IRepository<SystemUserModel>
    {
        SystemUserModel Find(string Username, string Password);
    }
}

using SilupostWeb.Data.Core;
using SilupostWeb.Data.Entity;
using System.Collections.Generic;

namespace SilupostWeb.Data.Interface
{
    public interface ISystemUserRepositoryDAC : IRepository<SystemUserModel>
    {
        SystemUserModel Find(string Username, string Password);
        List<SystemUserModel> GetPage(string Search, long SystemUserType, long PageNo, long PageSize, string OrderColumn, string OrderDir);
        bool Remove(string id, string LastUpdatedBy);
    }
}

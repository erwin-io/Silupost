using SilupostWeb.Data.Core;
using SilupostWeb.Data.Entity;
using System.Collections.Generic;

namespace SilupostWeb.Data.Interface
{
    public interface ISystemUserRepositoryDAC : IRepository<SystemUserModel>
    {
        string CreateAccount(SystemUserModel model);
        SystemUserModel FindByUsername(string Username);
        SystemUserModel Find(string Username, string Password);
        SystemUserModel GetTrackerStatus(string id);
        List<SystemUserModel> GetPage(string Search, long SystemUserType, long ApprovalStatus, long PageNo, long PageSize, string OrderColumn, string OrderDir);
        bool Remove(string id, string LastUpdatedBy);
        bool UpdateUsername(SystemUserModel model);
        bool UpdatePassword(SystemUserModel model);
    }
}

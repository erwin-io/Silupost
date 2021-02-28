using SilupostWeb.Data.Core;
using SilupostWeb.Data.Entity;
using System.Collections.Generic;

namespace SilupostWeb.Data.Interface
{
    public interface ISystemUserVerificationRepositoryDAC : IRepository<SystemUserVerificationModel>
    {
        SystemUserVerificationModel FindBySender(string sender, string code);
    }
}

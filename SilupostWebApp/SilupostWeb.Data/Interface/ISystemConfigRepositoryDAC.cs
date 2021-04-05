using SilupostWeb.Data.Core;
using SilupostWeb.Data.Entity;
using System.Collections.Generic;

namespace SilupostWeb.Data.Interface
{
    public interface ISystemConfigRepositoryDAC : IRepository<SystemConfigModel>
    {
        SystemConfigModel Find(long id);
    }
}

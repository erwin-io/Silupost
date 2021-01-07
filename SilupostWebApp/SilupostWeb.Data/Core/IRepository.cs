using System.Collections.Generic;

namespace SilupostWeb.Data.Core
{
    public interface IRepository<T>
    {
        List<T> GetAll();
        T Find(string id);
        string Add(T model);
        bool Remove(string id);
        bool Update(T model);

    }
}

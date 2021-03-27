using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SilupostMobileApp.Services.Interface
{
    public interface IService<T>
    {
        Task<bool> AddAsync(T model);
        Task<bool> UpdateAsync(T model);
        Task<bool> DeleteAsync(string id);
        Task<T> GetAsync(string id);
        Task<IEnumerable<T>> GetAllAsync();

    }
}

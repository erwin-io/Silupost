using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using SilupostMobileApp.Services.Interface;

namespace SilupostMobileApp.Services
{
    public abstract class ServiceBase<T> : IService<T>
    {
        public abstract Task<bool> AddAsync(T model);
        public abstract Task<bool> UpdateAsync(T model);
        public abstract Task<bool> DeleteAsync(string id);
        public abstract Task<T> GetAsync(string id);
        public abstract Task<IEnumerable<T>> GetAllAsync();
    }
}

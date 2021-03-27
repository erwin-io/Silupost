using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using SilupostMobileApp.BindingModels;
using SilupostMobileApp.Models;

namespace SilupostMobileApp.Services.Interface
{
    public interface ICrimeIncidentReportMediaService
    {
        Task<bool> AddAsync(CreateCrimeIncidentReportMediaBindingModel model);
        Task<bool> DeleteAsync(string id);
        Task<IEnumerable<CrimeIncidentReportMediaModel>> GetAllAsync();
        Task<CrimeIncidentReportMediaModel> GetAsync(string id);
        Task<bool> UpdateAsync(UpdateCrimeIncidentReportMediaBindingModel model);
    }
}

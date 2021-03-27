using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using SilupostMobileApp.BindingModels;
using SilupostMobileApp.Models;

namespace SilupostMobileApp.Services.Interface
{
    public interface ICrimeIncidentReportService
    {
        Task<bool> AddAsync(CreateCrimeIncidentReportBindingModel model);
        Task<bool> DeleteAsync(string id);
        Task<IEnumerable<CrimeIncidentReportModel>> GetAllAsync();
        Task<CrimeIncidentReportModel> GetAsync(string id);
        Task<bool> UpdateAsync(UpdateCrimeIncidentReportBindingModel model);
        Task<List<CrimeIncidentReportModel>> GetPageReportBySystemUserIdAsync(string SystemUserId, long PageNo, long PageSize);
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using SilupostMobileApp.BindingModels;
using SilupostMobileApp.Common;
using SilupostMobileApp.Models;

namespace SilupostMobileApp.Services.Interface
{
    public interface ICrimeIncidentReportService
    {
        Task<bool> AddAsync(CreateCrimeIncidentReportBindingModel model);
        Task<bool> DeleteAsync(string id);
        Task<IEnumerable<CrimeIncidentReportModel>> GetAllAsync();
        Task<SillupostWebAPIResponseModel<CrimeIncidentReportModel>> GetAsync(string id);
        Task<bool> UpdateAsync(UpdateCrimeIncidentReportBindingModel model);
        Task<SillupostWebAPIResponseModel<List<CrimeIncidentReportModel>>> GetPageReportBySystemUserIdAsync(string SystemUserId, long PageNo, long PageSize);
    }
}


using SilupostWeb.Mapping;
using SilupostWeb.Data.Entity;
using SilupostWeb.Data.Interface;
using SilupostWeb.Domain.BindingModel;
using SilupostWeb.Domain.ViewModel;
using SilupostWeb.Facade.Interface;
using System;
using System.Collections.Generic;
using System.Transactions;
using System.Linq;
using System.IO;

namespace SilupostWeb.Facade
{
    public class EnforcementReportValidationFacade : IEnforcementReportValidationFacade
    {
        private readonly IEnforcementReportValidationRepositoryDAC _enforcementReportValidationRepositoryDAC;
        private readonly ICrimeIncidentReportRepositoryDAC _crimeIncidentReportRepositoryDAC;

        #region CONSTRUCTORS
        public EnforcementReportValidationFacade(IEnforcementReportValidationRepositoryDAC enforcementReportValidationRepositoryDAC, ICrimeIncidentReportRepositoryDAC crimeIncidentReportRepositoryDAC)
        {
            _enforcementReportValidationRepositoryDAC = enforcementReportValidationRepositoryDAC ?? throw new ArgumentNullException(nameof(enforcementReportValidationRepositoryDAC));
            _crimeIncidentReportRepositoryDAC = crimeIncidentReportRepositoryDAC ?? throw new ArgumentNullException(nameof(crimeIncidentReportRepositoryDAC));
        }
        #endregion
        public string Add(CreateEnforcementReportValidationBindingModel model, string CreatedBy)
        {
            try
            {
                var id = string.Empty;
                using (var scope = new TransactionScope())
                {
                    var addReportValidationModel = AutoMapperHelper<CreateEnforcementReportValidationBindingModel, EnforcementReportValidationModel>.Map(model);
                    addReportValidationModel.SystemRecordManager.CreatedBy = CreatedBy;
                    id = _enforcementReportValidationRepositoryDAC.Add(addReportValidationModel);
                    if (string.IsNullOrEmpty(id))
                        throw new Exception("Error Saving Enforcement Report Validation");
                    scope.Complete();
                }
                return id;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public EnforcementReportValidationViewModel Find(string id)
        {
            var result = AutoMapperHelper<EnforcementReportValidationModel, EnforcementReportValidationViewModel>.Map(_enforcementReportValidationRepositoryDAC.Find(id));
            foreach(var media in result.CrimeIncidentReport.CrimeIncidentReportMedia)
            {
                if (media.File != null && File.Exists(media.File.FileName))
                    media.File.FileContent = System.IO.File.ReadAllBytes(media.File.FileName);
            }
            return result;
        }

        public PageResultsViewModel<EnforcementReportValidationViewModel> GetTablePageByCrimeIncidentReportId(string Search,
                                                                                   bool IsAdvanceSearchMode,
                                                                                   string CrimeIncidentReportId,
                                                                                   string EnforcementUnitName,
                                                                                   DateTime DateSubmittedFrom,
                                                                                   DateTime DateSubmittedTo,
                                                                                   string ReportValidationStatusId,
                                                                                   int PageNo,
                                                                                   int PageSize,
                                                                                   string OrderColumn,
                                                                                   string OrderDir)
        {
            var result = new PageResultsViewModel<EnforcementReportValidationViewModel>();
            var data = _enforcementReportValidationRepositoryDAC.GetTablePageByCrimeIncidentReportId(Search, 
                                                                 IsAdvanceSearchMode,
                                                                 CrimeIncidentReportId,
                                                                 EnforcementUnitName,
                                                                 DateSubmittedFrom,
                                                                 DateSubmittedTo,
                                                                 ReportValidationStatusId,
                                                                 PageNo, 
                                                                 PageSize, 
                                                                 OrderColumn, 
                                                                 OrderDir);
            result.Items = AutoMapperHelper<EnforcementReportValidationModel, EnforcementReportValidationViewModel>.MapList(data);
            result.TotalRows = data.Count > 0 ? data.FirstOrDefault().PageResult.TotalRows: 0;
            return result;
        } 

        public PageResultsViewModel<EnforcementReportValidationViewModel> GetPageByEnforcementSystemUserId(string Search,
                                                                                                           bool IsAdvanceSearchMode,
                                                                                                           string SystemUserId,
                                                                                                           string CrimeIncidentCategoryName,
                                                                                                           DateTime DateSubmittedFrom,
                                                                                                           DateTime DateSubmittedTo,
                                                                                                           string ReportValidationStatusId,
                                                                                                           int PageNo,
                                                                                                           int PageSize,
                                                                                                           string OrderColumn,
                                                                                                           string OrderDir)
        {
            var result = new PageResultsViewModel<EnforcementReportValidationViewModel>();
            var data = _enforcementReportValidationRepositoryDAC.GetPageByEnforcementSystemUserId(Search,
                                                                                                 IsAdvanceSearchMode,
                                                                                                 SystemUserId,
                                                                                                 CrimeIncidentCategoryName,
                                                                                                 DateSubmittedFrom,
                                                                                                 DateSubmittedTo,
                                                                                                 ReportValidationStatusId,
                                                                                                 PageNo,
                                                                                                 PageSize,
                                                                                                 OrderColumn,
                                                                                                 OrderDir);
            result.Items = AutoMapperHelper<EnforcementReportValidationModel, EnforcementReportValidationViewModel>.MapList(data);
            result.TotalRows = data.Count > 0 ? data.FirstOrDefault().PageResult.TotalRows : 0;
            return result;
        }

        public PageResultsViewModel<EnforcementReportValidationViewModel> GetPageByEnforcementStationId(string Search,
                                                                                                           bool IsAdvanceSearchMode,
                                                                                                           string EnforcementStationId,
                                                                                                           string CrimeIncidentCategoryName,
                                                                                                           DateTime DateSubmittedFrom,
                                                                                                           DateTime DateSubmittedTo,
                                                                                                           string ReportValidationStatusId,
                                                                                                           int PageNo,
                                                                                                           int PageSize,
                                                                                                           string OrderColumn,
                                                                                                           string OrderDir)
        {
            var result = new PageResultsViewModel<EnforcementReportValidationViewModel>();
            var data = _enforcementReportValidationRepositoryDAC.GetPageByEnforcementStationId(Search,
                                                                                                 IsAdvanceSearchMode,
                                                                                                 EnforcementStationId,
                                                                                                 CrimeIncidentCategoryName,
                                                                                                 DateSubmittedFrom,
                                                                                                 DateSubmittedTo,
                                                                                                 ReportValidationStatusId,
                                                                                                 PageNo,
                                                                                                 PageSize,
                                                                                                 OrderColumn,
                                                                                                 OrderDir);
            result.Items = AutoMapperHelper<EnforcementReportValidationModel, EnforcementReportValidationViewModel>.MapList(data);
            result.TotalRows = data.Count > 0 ? data.FirstOrDefault().PageResult.TotalRows : 0;
            return result;
        }
        public bool Remove(string id, string LastUpdatedBy)
        {
            var success = false;
            using (var scope = new TransactionScope())
            {
                success = _enforcementReportValidationRepositoryDAC.Remove(id, LastUpdatedBy);
                if(success)
                    scope.Complete();
            }
            return success;
        }

        public bool Update(UpdateEnforcementReportValidationBindingModel model, string LastUpdatedBy)
        {
            var success = false;
            using (var scope = new TransactionScope())
            {
                var updateModel = AutoMapperHelper<UpdateEnforcementReportValidationBindingModel, EnforcementReportValidationModel>.Map(model);
                updateModel.SystemRecordManager.LastUpdatedBy = LastUpdatedBy;
                success = _enforcementReportValidationRepositoryDAC.Update(updateModel);
                if (success)
                    scope.Complete();
            }
            return success;
        }
    }
}

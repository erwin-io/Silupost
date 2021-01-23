
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

namespace SilupostWeb.Facade
{
    public class CrimeIncidentCategoryFacade : ICrimeIncidentCategoryFacade
    {
        private readonly ICrimeIncidentCategoryRepositoryDAC _crimeIncidentCategoryRepositoryDAC;

        #region CONSTRUCTORS
        public CrimeIncidentCategoryFacade(ICrimeIncidentCategoryRepositoryDAC crimeIncidentCategoryRepositoryDAC)
        {
            _crimeIncidentCategoryRepositoryDAC = crimeIncidentCategoryRepositoryDAC ?? throw new ArgumentNullException(nameof(crimeIncidentCategoryRepositoryDAC));
        }
        #endregion
        public string Add(CrimeIncidentCategoryBindingModel model, string CreatedBy)
        {
            try
            {
                var id = string.Empty;
                using (var scope = new TransactionScope())
                {
                    var addModel = AutoMapperHelper<CrimeIncidentCategoryBindingModel, CrimeIncidentCategoryModel>.Map(model);
                    addModel.SystemRecordManager.CreatedBy = CreatedBy;
                    id = _crimeIncidentCategoryRepositoryDAC.Add(addModel);
                    if(!string.IsNullOrEmpty(id))
                        scope.Complete();
                }
                return id;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public CrimeIncidentCategoryViewModel Find(string id) => AutoMapperHelper<CrimeIncidentCategoryModel, CrimeIncidentCategoryViewModel>.Map(_crimeIncidentCategoryRepositoryDAC.Find(id));

        public PageResultsViewModel<CrimeIncidentCategoryViewModel> GetPage(string Search, int PageNo, int PageSize, string OrderColumn, string OrderDir) 
        {
            var result = new PageResultsViewModel<CrimeIncidentCategoryViewModel>();
            var data = _crimeIncidentCategoryRepositoryDAC.GetPage(Search, PageNo, PageSize, OrderColumn, OrderDir);
            result.Items = AutoMapperHelper<CrimeIncidentCategoryModel, CrimeIncidentCategoryViewModel>.MapList(data);
            result.TotalRows = data.Count > 0 ? data.FirstOrDefault().PageResult.TotalRows: 0;
            return result;
        } 

        public bool Remove(string id, string LastUpdatedBy)
        {
            var success = false;
            using (var scope = new TransactionScope())
            {
                success = _crimeIncidentCategoryRepositoryDAC.Remove(id, LastUpdatedBy);
                if(success)
                    scope.Complete();
            }
            return success;
        }

        public bool Update(UpdateCrimeIncidentCategoryBindingModel model, string LastUpdatedBy)
        {
            var success = false;
            using (var scope = new TransactionScope())
            {
                var updateModel = AutoMapperHelper<UpdateCrimeIncidentCategoryBindingModel, CrimeIncidentCategoryModel>.Map(model);
                updateModel.SystemRecordManager.LastUpdatedBy = LastUpdatedBy;
                success = _crimeIncidentCategoryRepositoryDAC.Update(updateModel);
                if (success)
                    scope.Complete();
            }
            return success;
        }
    }
}

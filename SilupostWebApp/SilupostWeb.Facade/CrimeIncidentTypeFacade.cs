
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
    public class CrimeIncidentTypeFacade : ICrimeIncidentTypeFacade
    {
        private readonly ICrimeIncidentTypeRepositoryDAC _crimeIncidentTypeRepositoryDAC;
        private readonly IFileRepositoryDAC _fileRepositoryDAC;

        #region CONSTRUCTORS
        public CrimeIncidentTypeFacade(ICrimeIncidentTypeRepositoryDAC crimeIncidentTypeRepositoryDAC, IFileRepositoryDAC fileRepositoryDAC)
        {
            _crimeIncidentTypeRepositoryDAC = crimeIncidentTypeRepositoryDAC ?? throw new ArgumentNullException(nameof(crimeIncidentTypeRepositoryDAC));
            _fileRepositoryDAC = fileRepositoryDAC ?? throw new ArgumentNullException(nameof(fileRepositoryDAC));
        }
        #endregion
        public string Add(CreateCrimeIncidentTypeBindingModel model, string CreatedBy)
        {
            try
            {
                var id = string.Empty;
                using (var scope = new TransactionScope())
                {
                    var addModel = AutoMapperHelper<CreateCrimeIncidentTypeBindingModel, CrimeIncidentTypeModel>.Map(model);

                    //Start Saving file
                    addModel.IconFile.FileContent = System.Convert.FromBase64String(model.IconFile.FileFromBase64String);
                    addModel.IconFile.SystemRecordManager.CreatedBy = CreatedBy;
                    var fileId = _fileRepositoryDAC.Add(addModel.IconFile);
                    if (string.IsNullOrEmpty(fileId))
                        throw new Exception("Error Saving File");
                    addModel.IconFile.FileId = fileId;
                    //End Saving file

                    addModel.SystemRecordManager.CreatedBy = CreatedBy;
                    id = _crimeIncidentTypeRepositoryDAC.Add(addModel);
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

        public CrimeIncidentTypeViewModel Find(string id) => AutoMapperHelper<CrimeIncidentTypeModel, CrimeIncidentTypeViewModel>.Map(_crimeIncidentTypeRepositoryDAC.Find(id));

        public PageResultsViewModel<CrimeIncidentTypeViewModel> GetPage(string Search, int PageNo, int PageSize, string OrderColumn, string OrderDir) 
        {
            var result = new PageResultsViewModel<CrimeIncidentTypeViewModel>();
            var data = _crimeIncidentTypeRepositoryDAC.GetPage(Search, PageNo, PageSize, OrderColumn, OrderDir);
            result.Items = AutoMapperHelper<CrimeIncidentTypeModel, CrimeIncidentTypeViewModel>.MapList(data);
            result.TotalRows = data.Count > 0 ? data.FirstOrDefault().PageResult.TotalRows: 0;
            return result;
        } 

        public bool Remove(string id, string LastUpdatedBy)
        {
            var success = false;
            using (var scope = new TransactionScope())
            {
                success = _crimeIncidentTypeRepositoryDAC.Remove(id, LastUpdatedBy);
                if(success)
                    scope.Complete();
            }
            return success;
        }

        public bool Update(UpdateCrimeIncidentTypeBindingModel model, string LastUpdatedBy)
        {
            var success = false;
            using (var scope = new TransactionScope())
            {
                var updateModel = AutoMapperHelper<UpdateCrimeIncidentTypeBindingModel, CrimeIncidentTypeModel>.Map(model);
                updateModel.SystemRecordManager.LastUpdatedBy = LastUpdatedBy;
                success = _crimeIncidentTypeRepositoryDAC.Update(updateModel);
                if (success)
                {
                    //Start Saving file
                    updateModel.IconFile.FileContent = System.Convert.FromBase64String(model.IconFile.FileFromBase64String);
                    updateModel.IconFile.SystemRecordManager.LastUpdatedBy = LastUpdatedBy;
                    success = _fileRepositoryDAC.Update(updateModel.IconFile);
                    if (success)
                        scope.Complete();
                    //End Saving file
                }
            }
            return success;
        }
    }
}

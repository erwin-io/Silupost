
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
    public class EnforcementTypeFacade : IEnforcementTypeFacade
    {
        private readonly IEnforcementTypeRepositoryDAC _enforcementTypeRepositoryDAC;
        private readonly IFileRepositoryRepositoryDAC _fileRepositoryDAC;

        #region CONSTRUCTORS
        public EnforcementTypeFacade(IEnforcementTypeRepositoryDAC enforcementTypeRepositoryDAC, IFileRepositoryRepositoryDAC fileRepositoryDAC)
        {
            _enforcementTypeRepositoryDAC = enforcementTypeRepositoryDAC ?? throw new ArgumentNullException(nameof(enforcementTypeRepositoryDAC));
            _fileRepositoryDAC = fileRepositoryDAC ?? throw new ArgumentNullException(nameof(fileRepositoryDAC));
        }
        #endregion
        public string Add(CreateEnforcementTypeBindingModel model, string CreatedBy)
        {
            try
            {
                var id = string.Empty;
                using (var scope = new TransactionScope())
                {
                    var addModel = AutoMapperHelper<CreateEnforcementTypeBindingModel, EnforcementTypeModel>.Map(model);

                    //Start Saving file
                    addModel.IconFile.FileContent = System.Convert.FromBase64String(model.IconFile.FileFromBase64String);
                    addModel.IconFile.SystemRecordManager.CreatedBy = CreatedBy;
                    var fileId = _fileRepositoryDAC.Add(addModel.IconFile);
                    if (string.IsNullOrEmpty(fileId))
                        throw new Exception("Error Saving File");
                    addModel.IconFile.FileId = fileId;
                    //End Saving file

                    addModel.SystemRecordManager.CreatedBy = CreatedBy;
                    id = _enforcementTypeRepositoryDAC.Add(addModel);
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

        public EnforcementTypeViewModel Find(string id) => AutoMapperHelper<EnforcementTypeModel, EnforcementTypeViewModel>.Map(_enforcementTypeRepositoryDAC.Find(id));

        public PageResultsViewModel<EnforcementTypeViewModel> GetPage(string Search, int PageNo, int PageSize, string OrderColumn, string OrderDir) 
        {
            var result = new PageResultsViewModel<EnforcementTypeViewModel>();
            var data = _enforcementTypeRepositoryDAC.GetPage(Search, PageNo, PageSize, OrderColumn, OrderDir);
            result.Items = AutoMapperHelper<EnforcementTypeModel, EnforcementTypeViewModel>.MapList(data);
            result.TotalRows = data.Count > 0 ? data.FirstOrDefault().PageResult.TotalRows: 0;
            return result;
        } 

        public bool Remove(string id, string LastUpdatedBy)
        {
            var success = false;
            using (var scope = new TransactionScope())
            {
                success = _enforcementTypeRepositoryDAC.Remove(id, LastUpdatedBy);
                if(success)
                    scope.Complete();
            }
            return success;
        }

        public bool Update(UpdateEnforcementTypeBindingModel model, string LastUpdatedBy)
        {
            var success = false;
            using (var scope = new TransactionScope())
            {
                var updateModel = AutoMapperHelper<UpdateEnforcementTypeBindingModel, EnforcementTypeModel>.Map(model);
                updateModel.SystemRecordManager.LastUpdatedBy = LastUpdatedBy;
                success = _enforcementTypeRepositoryDAC.Update(updateModel);
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

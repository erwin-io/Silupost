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
    public class EnforcementUnitFacade : IEnforcementUnitFacade
    {
        private readonly IEnforcementUnitRepositoryDAC _enforcementUnitRepositoryDAC;
        private readonly ILegalEntityRepository _legalEntityRepository;
        private readonly ILegalEntityAddressRepositoryDAC _legalEntityAddressRepositoryDAC;
        private readonly IFileRepositoryRepositoryDAC _fileRepositoryDAC;

        #region CONSTRUCTORS
        public EnforcementUnitFacade(IEnforcementUnitRepositoryDAC enforcementUnitRepositoryDAC,
            ILegalEntityRepository legalEntityRepository,
            ILegalEntityAddressRepositoryDAC legalEntityAddressRepositoryDAC,
            IFileRepositoryRepositoryDAC fileRepositoryDAC)
        {
            _enforcementUnitRepositoryDAC = enforcementUnitRepositoryDAC ?? throw new ArgumentNullException(nameof(enforcementUnitRepositoryDAC));
            _legalEntityRepository = legalEntityRepository ?? throw new ArgumentNullException(nameof(legalEntityRepository));
            _legalEntityAddressRepositoryDAC = legalEntityAddressRepositoryDAC ?? throw new ArgumentNullException(nameof(legalEntityAddressRepositoryDAC));
            _fileRepositoryDAC = fileRepositoryDAC ?? throw new ArgumentNullException(nameof(fileRepositoryDAC));
        }
        #endregion

        public string Add(CreateEnforcementUnitBindingModel model, string CreatedBy)
        {
            try
            {
                var id = string.Empty;
                using (var scope = new TransactionScope())
                {
                    var addModel = AutoMapperHelper<CreateEnforcementUnitBindingModel, EnforcementUnitModel>.Map(model);

                    //Start Saving LegalEntity
                    var legalEntityId = _legalEntityRepository.Add(addModel.LegalEntity);
                    if (string.IsNullOrEmpty(legalEntityId))
                    {
                        throw new Exception("Error Saving Enforcement Unit Legal Entity");
                    }

                    //End Saving LegalEntity

                    //Start Saving LegalEntity Address
                    foreach (var addess in addModel.LegalEntity.LegalEntityAddress)
                    {
                        var legalEntityAddressId = _legalEntityAddressRepositoryDAC.Add(new LegalEntityAddressModel()
                        {
                            LegalEntity = new LegalEntityModel() {  LegalEntityId = legalEntityId },
                            Address = addess.Address
                        });
                        if (string.IsNullOrEmpty(legalEntityAddressId))
                        {
                            throw new Exception("Error Saving Enforcement Unit Legal Entity Address");
                        }
                    }
                    //End Saving LegalEntity Address

                    //Start Saving file
                    addModel.ProfilePicture.FileContent = System.Convert.FromBase64String(model.ProfilePicture.FileFromBase64String);
                    addModel.ProfilePicture.SystemRecordManager.CreatedBy = CreatedBy;
                    var fileId = _fileRepositoryDAC.Add(addModel.ProfilePicture);
                    if (string.IsNullOrEmpty(fileId))
                        throw new Exception("Error Saving File");
                    addModel.ProfilePicture.FileId = fileId;
                    //End Saving file

                    //start store file directory
                    if (File.Exists(addModel.ProfilePicture.FileName))
                    {
                        File.Delete(addModel.ProfilePicture.FileName);
                    }
                    using (var fs = new FileStream(addModel.ProfilePicture.FileName, FileMode.Create, FileAccess.Write))
                    {
                        fs.Write(addModel.ProfilePicture.FileContent, 0, addModel.ProfilePicture.FileContent.Length);
                    }
                    //end store file directory

                    addModel.SystemRecordManager.CreatedBy = CreatedBy;
                    addModel.LegalEntity.LegalEntityId = legalEntityId;
                    id = _enforcementUnitRepositoryDAC.Add(addModel);
                    if (string.IsNullOrEmpty(id))
                    {
                        throw new Exception("Error Creating System User");
                    }
                    scope.Complete();
                }
                return id;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public PageResultsViewModel<EnforcementUnitViewModel> GetPage(string Search, long PageNo, long PageSize, string OrderColumn, string OrderDir)
        {
            var result = new PageResultsViewModel<EnforcementUnitViewModel>();
            var data = _enforcementUnitRepositoryDAC.GetPage(Search, PageNo, PageSize, OrderColumn, OrderDir);
            result.Items = AutoMapperHelper<EnforcementUnitModel, EnforcementUnitViewModel>.MapList(data);
            foreach (var item in result.Items)
            {
                if (item.ProfilePicture != null && File.Exists(item.ProfilePicture.FileName))
                    item.ProfilePicture.FileContent = System.IO.File.ReadAllBytes(item.ProfilePicture.FileName);
            }
            result.TotalRows = data.Count > 0 ? data.FirstOrDefault().PageResult.TotalRows : 0;
            return result;
        }


        public EnforcementUnitViewModel Find(string id)
        {
            var result = AutoMapperHelper<EnforcementUnitModel, EnforcementUnitViewModel>.Map(_enforcementUnitRepositoryDAC.Find(id));
            if (result.ProfilePicture != null && File.Exists(result.ProfilePicture.FileName))
                result.ProfilePicture.FileContent = System.IO.File.ReadAllBytes(result.ProfilePicture.FileName);
            return result;
        }

        public bool Remove(string id, string LastUpdatedBy)
        {
            var success = false;
            using (var scope = new TransactionScope())
            {
                success = _enforcementUnitRepositoryDAC.Remove(id, LastUpdatedBy);
                if(success)
                    scope.Complete();
            }
            return success;
        }

        public bool Update(UpdateEnforcementUnitBindingModel model, string LastUpdatedBy)
        {
            var success = false;
            using (var scope = new TransactionScope())
            {
                var updateModel = AutoMapperHelper<UpdateEnforcementUnitBindingModel, EnforcementUnitModel>.Map(model);
                updateModel.SystemRecordManager.LastUpdatedBy = LastUpdatedBy;
                success = _enforcementUnitRepositoryDAC.Update(updateModel);
                if (!success)
                {
                    throw new Exception("Error Updating Enforcement Unit");
                }

                //Start Saving LegalEntity
                updateModel.SystemRecordManager.LastUpdatedBy = LastUpdatedBy;
                success = _legalEntityRepository.Update(updateModel.LegalEntity);
                if (!success)
                {
                    throw new Exception("Error Updating Enforcement Unit Legal Entity");
                }
                //End Saving file

                //Start Saving file
                updateModel.ProfilePicture.FileContent = System.Convert.FromBase64String(model.ProfilePicture.FileFromBase64String);
                updateModel.ProfilePicture.SystemRecordManager.LastUpdatedBy = LastUpdatedBy;
                success = _fileRepositoryDAC.Update(updateModel.ProfilePicture);
                if (!success)
                {
                    throw new Exception("Error Saving File");
                }
                //End Saving file

                //start store file directory
                if (File.Exists(updateModel.ProfilePicture.FileName))
                {
                    File.Delete(updateModel.ProfilePicture.FileName);
                }
                using (var fs = new FileStream(updateModel.ProfilePicture.FileName, FileMode.Create, FileAccess.Write))
                {
                    fs.Write(updateModel.ProfilePicture.FileContent, 0, updateModel.ProfilePicture.FileContent.Length);
                }
                //end store file directory

                scope.Complete();
            }
            return success;
        }
    }
}

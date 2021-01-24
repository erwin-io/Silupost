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
    public class SystemUserFacade : ISystemUserFacade
    {
        private readonly ISystemUserRepository _systemUserRepository;
        private readonly ILegalEntityRepository _legalEntityRepository;
        private readonly ILegalEntityAddressRepositoryDAC _legalEntityAddressRepositoryDAC;
        private readonly ISystemWebAdminUserRolesRepositoryDAC _systemWebAdminUserRolesRepositoryDAC;
        private readonly IFileRepositoryRepositoryDAC _fileRepositoryDAC;

        #region CONSTRUCTORS
        public SystemUserFacade(ISystemUserRepository systemUserRepository,
            ILegalEntityRepository legalEntityRepository,
            ILegalEntityAddressRepositoryDAC legalEntityAddressRepositoryDAC,
            ISystemWebAdminUserRolesRepositoryDAC systemWebAdminUserRolesRepositoryDAC,
            IFileRepositoryRepositoryDAC fileRepositoryDAC)
        {
            _systemUserRepository = systemUserRepository ?? throw new ArgumentNullException(nameof(systemUserRepository));
            _legalEntityRepository = legalEntityRepository ?? throw new ArgumentNullException(nameof(legalEntityRepository));
            _legalEntityAddressRepositoryDAC = legalEntityAddressRepositoryDAC ?? throw new ArgumentNullException(nameof(legalEntityAddressRepositoryDAC));
            _systemWebAdminUserRolesRepositoryDAC = systemWebAdminUserRolesRepositoryDAC ?? throw new ArgumentNullException(nameof(systemWebAdminUserRolesRepositoryDAC));
            _fileRepositoryDAC = fileRepositoryDAC ?? throw new ArgumentNullException(nameof(fileRepositoryDAC));
        }
        #endregion

        public string Add(CreateSystemUserBindingModel model, string CreatedBy)
        {
            try
            {
                var id = string.Empty;
                using (var scope = new TransactionScope())
                {
                    var addModel = AutoMapperHelper<CreateSystemUserBindingModel, SystemUserModel>.Map(model);

                    //Start Saving LegalEntity
                    var legalEntityId = _legalEntityRepository.Add(addModel.LegalEntity);
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
                            throw new Exception("Error Saving System User Legal Entity Address");
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

                    addModel.SystemRecordManager.CreatedBy = CreatedBy;
                    addModel.LegalEntity.LegalEntityId = legalEntityId;
                    id = _systemUserRepository.Add(addModel);
                    if (string.IsNullOrEmpty(id))
                    {
                        throw new Exception("Error Creating System User");
                    }
                    foreach (var role in model.SystemWebAdminUserRoles)
                    {
                        var SystemWebAdminUserRoleId = _systemWebAdminUserRolesRepositoryDAC.Add(new SystemWebAdminUserRolesModel()
                        {
                            SystemUser = new SystemUserModel() { SystemUserId = id },
                            SystemWebAdminRole = new SystemWebAdminRoleModel() { SystemWebAdminRoleId = role.SystemWebAdminRoleId },
                            SystemRecordManager = new SystemRecordManagerModel() { CreatedBy = CreatedBy }
                        });
                        if (string.IsNullOrEmpty(SystemWebAdminUserRoleId))
                        {
                            throw new Exception("Error Creating System User Roles");
                        }
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
        public PageResultsViewModel<SystemUserViewModel> GetPage(string Search, long SystemUserType, long PageNo, long PageSize, string OrderColumn, string OrderDir)
        {
            var result = new PageResultsViewModel<SystemUserViewModel>();
            var data = _systemUserRepository.GetPage(Search, SystemUserType, PageNo, PageSize, OrderColumn, OrderDir);
            result.Items = AutoMapperHelper<SystemUserModel, SystemUserViewModel>.MapList(data);
            result.TotalRows = data.Count > 0 ? data.FirstOrDefault().PageResult.TotalRows : 0;
            return result;
        }


        public SystemUserViewModel Find(string id) => AutoMapperHelper<SystemUserModel, SystemUserViewModel>.Map(_systemUserRepository.Find(id));

        public SystemUserViewModel Find(string Username, string Password) => AutoMapperHelper<SystemUserModel, SystemUserViewModel>.Map(_systemUserRepository.Find(Username, Password));

        public bool Remove(string id, string LastUpdatedBy)
        {
            var success = false;
            using (var scope = new TransactionScope())
            {
                success = _systemUserRepository.Remove(id, LastUpdatedBy);
                if(success)
                    scope.Complete();
            }
            return success;
        }

        public bool Update(UpdateSystemUserBindingModel model, string LastUpdatedBy)
        {
            var success = false;
            using (var scope = new TransactionScope())
            {
                var updateModel = AutoMapperHelper<UpdateSystemUserBindingModel, SystemUserModel>.Map(model);
                updateModel.SystemRecordManager.LastUpdatedBy = LastUpdatedBy;
                success = _systemUserRepository.Update(updateModel);
                if (!success)
                {
                    throw new Exception("Error Updating System User");
                }

                //Start Saving LegalEntity
                updateModel.SystemRecordManager.LastUpdatedBy = LastUpdatedBy;
                success = _legalEntityRepository.Update(updateModel.LegalEntity);
                if (!success)
                {
                    throw new Exception("Error Updating System User");
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

                var currentSystemWebAdminUserRoles = AutoMapperHelper<SystemWebAdminUserRolesModel, SystemWebAdminUserRolesViewModel>.MapList(_systemWebAdminUserRolesRepositoryDAC.FindBySystemUserId(model.SystemUserId));
                var newSystemWebAdminUserRoles = new List<SystemWebAdminUserRolesModel>();

                foreach (var role in currentSystemWebAdminUserRoles)
                {
                    if(!model.SystemWebAdminUserRoles.Any(swaur=>swaur.SystemWebAdminRoleId == role.SystemWebAdminRole.SystemWebAdminRoleId))
                    {
                        var systemUserRole = _systemWebAdminUserRolesRepositoryDAC.FindBySystemWebAdminRoleIdAndSystemUserId(role.SystemWebAdminRole.SystemWebAdminRoleId, model.SystemUserId);
                        if(systemUserRole != null)
                        {
                            if (!_systemWebAdminUserRolesRepositoryDAC.Remove(systemUserRole.SystemWebAdminUserRoleId, LastUpdatedBy))
                                throw new Exception("Error Updating System User Role");
                        }
                    }
                }
                foreach (var role in model.SystemWebAdminUserRoles)
                {
                    if (!currentSystemWebAdminUserRoles.Any(swaur => swaur.SystemWebAdminRole.SystemWebAdminRoleId == role.SystemWebAdminRoleId))
                    {
                        var SystemWebAdminUserRoleId = _systemWebAdminUserRolesRepositoryDAC.Add(new SystemWebAdminUserRolesModel()
                        {
                            SystemUser = new SystemUserModel() { SystemUserId = model.SystemUserId },
                            SystemWebAdminRole = new SystemWebAdminRoleModel() { SystemWebAdminRoleId = role.SystemWebAdminRoleId },
                            SystemRecordManager = new SystemRecordManagerModel() { CreatedBy = LastUpdatedBy }
                        });
                        if (string.IsNullOrEmpty(SystemWebAdminUserRoleId))
                        {
                            throw new Exception("Error Creating System User Roles");
                        }
                    }
                }
                scope.Complete();
            }
            return success;
        }
    }
}

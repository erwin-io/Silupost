
using POSWeb.POS.Mapping;
using POSWeb.POSAdmin.Data.Entity;
using POSWeb.POSAdmin.Data.Interface;
using POSWeb.POSAdmin.Domain.BindingModel;
using POSWeb.POSAdmin.Domain.ViewModel;
using POSWeb.POSAdmin.Facade.Interface;
using System;
using System.Collections.Generic;
using System.Transactions;

namespace POSWeb.POSAdmin.Facade
{
    public class SystemRoleFacade : ISystemRoleFacade
    {
        private readonly ISystemRoleRepositoryDAC _systemRoleRepositoryDAC;

        #region CONSTRUCTORS
        public SystemRoleFacade(ISystemRoleRepositoryDAC systemRoleRepositoryDAC)
        {
            _systemRoleRepositoryDAC = systemRoleRepositoryDAC ?? throw new ArgumentNullException(nameof(systemRoleRepositoryDAC));
        }
        #endregion
        public string Add(SystemRoleBindingModel model, long LocationId, string CreatedBy)
        {
            try
            {
                var id = string.Empty;
                using (var scope = new TransactionScope())
                {
                    var addModel = AutoMapperHelper<SystemRoleBindingModel, SystemRoleModel>.Map(model);
                    addModel.Location.LocationId = LocationId;
                    addModel.CreatedBy.SystemUserId = CreatedBy;
                    id = _systemRoleRepositoryDAC.Add(addModel);
                    scope.Complete();
                }
                return id;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public SystemRoleViewModel Find(string id) => AutoMapperHelper<SystemRoleModel, SystemRoleViewModel>.Map(_systemRoleRepositoryDAC.Find(id));

        public PageResultsViewModel<SystemRoleViewModel> GetPage(string SystemRoleId, string Name, string CreatedAt, string UpdatedAt, int PageNo, int PageSize, long LocationId) => AutoMapperHelper<PageResultsModel<SystemRoleModel>, PageResultsViewModel<SystemRoleViewModel>>.Map(_systemRoleRepositoryDAC.GetPage(SystemRoleId, Name, CreatedAt, UpdatedAt, PageNo, PageSize, LocationId));

        public bool Remove(string id, string UpdatedBy)
        {
            var success = false;
            using (var scope = new TransactionScope())
            {
                success = _systemRoleRepositoryDAC.Remove(id, UpdatedBy);
                scope.Complete();
            }
            return success;
        }

        public bool Update(UpdateSystemRoleBindingModel model, string UpdatedBy)
        {
            var success = false;
            using (var scope = new TransactionScope())
            {
                var updateModel = AutoMapperHelper<UpdateSystemRoleBindingModel, SystemRoleModel>.Map(model);
                updateModel.UpdatedBy.SystemUserId = UpdatedBy;
                success = _systemRoleRepositoryDAC.Update(updateModel);
                scope.Complete();
            }
            return success;
        }
    }
}

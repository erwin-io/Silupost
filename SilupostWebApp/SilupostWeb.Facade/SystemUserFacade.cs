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
    public class SystemUserFacade : ISystemUserFacade
    {
        private readonly ISystemUserRepository _systemUserRepository;
        private readonly IEntityInformationRepository _entityInformationRepository;

        #region CONSTRUCTORS
        public SystemUserFacade(ISystemUserRepository systemUserRepository, IEntityInformationRepository entityInformationRepository)
        {
            _systemUserRepository = systemUserRepository ?? throw new ArgumentNullException(nameof(systemUserRepository));
            _entityInformationRepository = entityInformationRepository ?? throw new ArgumentNullException(nameof(entityInformationRepository));
        }
        #endregion

        public string Add(CreateSystemUserBindingModel model)
        {
            try
            {
                var id = string.Empty;
                using (var scope = new TransactionScope())
                {
                    var addModel = AutoMapperHelper<CreateSystemUserBindingModel, SystemUserModel>.Map(model);
                    var legalEntityId = _entityInformationRepository.Add(addModel.EntityInformation);
                    addModel.EntityInformation.LegalEntityId = legalEntityId;
                    id = _systemUserRepository.Add(addModel);
                    scope.Complete();
                }
                return id;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public SystemUserViewModel Find(string id) => AutoMapperHelper<SystemUserModel, SystemUserViewModel>.Map(_systemUserRepository.Find(id));

        public SystemUserViewModel Find(string Username, string Password) => AutoMapperHelper<SystemUserModel, SystemUserViewModel>.Map(_systemUserRepository.Find(Username, Password));

        public bool SystemUserAccountApproval(SystemUserAccountApprovalBindingModel model)
        {
            var success = false;
            //using (var scope = new TransactionScope())
            //{
            //    success = _systemUserRepository.Update(AutoMapperHelper<UpdateSystemRoleBindingModel, SystemRoleModel>.Map(model));
            //    scope.Complete();
            //}
            return success;
        }
    }
}

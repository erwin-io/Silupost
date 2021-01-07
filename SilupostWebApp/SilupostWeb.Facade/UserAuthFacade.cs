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
    public class UserAuthFacade : IUserAuthFacade
    {
        private readonly ISystemUserRepository _systemUserRepository;

        #region CONSTRUCTORS
        public UserAuthFacade(ISystemUserRepository systemUserRepository)
        {
            _systemUserRepository = systemUserRepository ?? throw new ArgumentNullException(nameof(systemUserRepository));
        }
        #endregion
        public SystemUserViewModel Find(string id) => AutoMapperHelper<SystemUserModel, SystemUserViewModel>.Map(_systemUserRepository.Find(id));

        public SystemUserViewModel Find(string Username, string Password) => AutoMapperHelper<SystemUserModel, SystemUserViewModel>.Map(_systemUserRepository.Find(Username, Password));
    }
}

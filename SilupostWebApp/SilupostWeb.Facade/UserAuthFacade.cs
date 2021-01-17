using SilupostWeb.Mapping;
using SilupostWeb.Data.Entity;
using SilupostWeb.Data.Interface;
using SilupostWeb.Domain.BindingModel;
using SilupostWeb.Domain.ViewModel;
using SilupostWeb.Facade.Interface;
using System;
using System.Collections.Generic;
using System.Transactions;

namespace SilupostWeb.Facade
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

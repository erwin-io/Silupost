using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using SilupostMobileApp.BindingModels;
using SilupostMobileApp.Models;

namespace SilupostMobileApp.Services.Interface
{
    public interface ISystemUserService
    {
        Task<SystemUserModel> Get(string id);
        Task<SystemUserModel> GetByCredentials(string username, string password);
        Task<SystemUserModel> CreateAccount(CreateAccountSystemUserBindingModel model);
        Task<SystemUserModel> UpdatePersonalDetails(UpdateSystemUserBindingModel model);
        Task<SystemTokenModel> GetRefreshToken(string RefreshToken);
        Task<SystemUserModel> ResetPassword(UpdateSystemResetPasswordBindingModel model);
        Task<SystemUserModel> UpdateUsername(UpdateSystemUserNameBindingModel model);
    }
}

using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using SilupostMobileApp.BindingModels;
using SilupostMobileApp.Models;
using SilupostMobileApp.ViewModels;

namespace SilupostMobileApp.Services.Interface
{
    public interface ISystemUserVerificationService
    {
        Task<SystemUserVerificationModel> GetBySender(string sender, string code);
        Task<SystemUserVerificationModel> SendEmailVerification(SystemUserVerificationBindingModel model);
        Task<SystemUserVerificationModel> SendEmailChangePassword(SystemUserVerificationBindingModel model);
    }
}

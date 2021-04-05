using Plugin.Toast;
using SilupostMobileApp.Common;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using SilupostMobileApp.BindingModels;
using SilupostMobileApp.Models;

namespace SilupostMobileApp.ViewModels
{

    public class ChangePasswordViewModel : BaseViewModel
    {
        public INavigation Navigation { get; set; }
        public Command TextChangedCommand { get; set; }

        SystemUserModel _systemUser;
        public SystemUserModel SystemUser
        {
            get => _systemUser;
            set => SetProperty(ref _systemUser, value);
        }
        SystemUserVerificationModel _systemUserVerification;
        public SystemUserVerificationModel SystemUserVerification
        {
            get => _systemUserVerification;
            set => SetProperty(ref _systemUserVerification, value);
        }

        string _systemUserId;
        public string SystemUserId
        {
            get => _systemUserId;
            set => SetProperty(ref _systemUserId, value);
        }

        string _verificationCode;
        public string VerificationCode
        {
            get => _verificationCode;
            set => SetProperty(ref _verificationCode, value);
        }

        ValidatableObject<string> _email;
        public ValidatableObject<string> Email
        {
            get => _email;
            set => SetProperty(ref _email, value);
        }

        ValidatableObject<string> _password;
        public ValidatableObject<string> Password
        {
            get => _password;
            set => SetProperty(ref _password, value);
        }

        ValidatableObject<string> _passwordConfirm;
        public ValidatableObject<string> PasswordConfirm
        {
            get => _passwordConfirm;
            set => SetProperty(ref _passwordConfirm, value);
        }

        bool _isEmailSubmitted;
        public bool IsEmailSubmitted
        {
            get => _isEmailSubmitted;
            set => SetProperty(ref _isEmailSubmitted, value);
        }

        bool _isResetPasswordSuccess;
        public bool IsResetPasswordSuccess
        {
            get => _isResetPasswordSuccess;
            set => SetProperty(ref _isResetPasswordSuccess, value);
        }

        bool _isFromProfile;
        public bool IsFromProfile
        {
            get => _isFromProfile;
            set => SetProperty(ref _isFromProfile, value);
        }
        public ChangePasswordViewModel(INavigation navigation)
        {
            this.Navigation = navigation;
            InitViewModel();
            AddValidationRules();
            InitGenderList();
            TextChangedCommand = new Command<string>(async (Name) => await TextChanged(Name));
        }

        public async Task InitViewModel()
        {
            Email = new ValidatableObject<string>();
            Password = new ValidatableObject<string>();
            PasswordConfirm = new ValidatableObject<string>();
        }

        public async Task AddValidationRules()
        {
            Email.Validations.Add(new IsNotNullOrEmptyRule<string> { ValidationMessage = "Email Required" });
            Email.Validations.Add(new IsValidEmailRule<string> { ValidationMessage = "Please enter valid Email" });

            Password.Validations.Add(new IsNotNullOrEmptyRule<string> { ValidationMessage = "Password Required" });
            Password.Validations.Add(new IsValidPasswordRule<string> { ValidationMessage = "Password between 8-20 characters; must contain at least one lowercase letter, one uppercase letter, one numeric digit, and one special character" });
            PasswordConfirm.Validations.Add(new IsNotNullOrEmptyRule<string> { ValidationMessage = "Confirm password required" });
        }

        public async Task ResetEmailRegistration()
        {
            this.IsEmailSubmitted = false;
            Email = new ValidatableObject<string>();
            Email.Validations.Add(new IsNotNullOrEmptyRule<string> { ValidationMessage = "Email Required" });
            Email.Validations.Add(new IsValidEmailRule<string> { ValidationMessage = "Please enter valid Email" });
        }

        public async Task ResetCredentials()
        {
            this.IsResetPasswordSuccess = false;
            Password = new ValidatableObject<string>();
            PasswordConfirm = new ValidatableObject<string>();
            Password.Validations.Add(new IsNotNullOrEmptyRule<string> { ValidationMessage = "Password Required" });
            Password.Validations.Add(new IsValidPasswordRule<string> { ValidationMessage = "Password between 8-20 characters; must contain at least one lowercase letter, one uppercase letter, one numeric digit, and one special character" });
            PasswordConfirm.Validations.Add(new IsNotNullOrEmptyRule<string> { ValidationMessage = "Confirm password required" });
        }
        public async Task<bool> EmailValid()
        {
            bool isEmailValid = Email.Validate();

            return isEmailValid;
        }
        public async Task<bool> CredentialsValid()
        {
            bool isEmailValid = Email.Validate();
            bool isPasswordValid = Password.Validate();
            bool isPasswordConfirmValid = PasswordConfirm.Validate();
            if(isPasswordConfirmValid && isPasswordValid && Password.Value != PasswordConfirm.Value)
            {
                Password.HasCustomError = true;
                Password.Errors = new List<string>() { "Password and confirm password don't match" };
                isPasswordValid = Password.Validate();

                PasswordConfirm.HasCustomError = true;
                PasswordConfirm.Errors = new List<string>() { "Password and confirm password don't match" };
                isPasswordConfirmValid = PasswordConfirm.Validate();
            }
            else
            {
                Password.HasCustomError = false;
                PasswordConfirm.HasCustomError = false;
                isPasswordValid = Password.Validate();
                isPasswordConfirmValid = PasswordConfirm.Validate();
            }
            return isEmailValid && isPasswordValid && isPasswordConfirmValid;
        }
        public async Task<SystemUserVerificationModel> SendEmailChangePassword(SystemUserVerificationBindingModel model)
        {
            SystemUserVerificationModel result = null;
            try
            {
                result = await SystemUserVerificationService.SendEmailChangePassword(model);
                IsBusy = false;
                return result;
            }
            catch (Exception ex)
            {
                IsBusy = false;
                throw ex;
            }
        }
        public async Task<SystemUserVerificationModel> GetBySender(string sender, string code)
        {
            SystemUserVerificationModel result = null;
            try
            {
                result = await SystemUserVerificationService.GetBySender(sender, code);
                IsBusy = false;
                return result;
            }
            catch (Exception ex)
            {
                IsBusy = false;
                throw ex;
            }
        }
        public async Task<SystemUserModel> ResetPassword(UpdateSystemResetPasswordBindingModel model)
        {
            SystemUserModel result = null;
            try
            {
                result = await SystemUserService.ResetPassword(model);
                IsBusy = false;
                return result;
            }
            catch (Exception ex)
            {
                IsBusy = false;
                throw ex;
            }
        }
        public async Task TextChanged(string Name)
        {
            switch (Name)
            {
                case "Email":
                    Email.Validate();
                    break;
                case "Password":
                    Password.Validate();
                    break;
                case "PasswordConfirm":
                    PasswordConfirm.Validate();
                    break;
            }
        }
    }
}

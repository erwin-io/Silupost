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

    public class UpdateEmailViewModel : BaseViewModel
    {
        public INavigation Navigation { get; set; }
        public Command LoginCommand { get; set; }
        public Command TextChangedCommand { get; set; }
        public SystemUserModel SystemUser { get; set; }

        ContentView _content;
        public ContentView Content
        {
            get => _content;
            set => SetProperty(ref _content, value);
        }

        ValidatableObject<string> _email;
        public ValidatableObject<string> Email
        {
            get => _email;
            set => SetProperty(ref _email, value);
        }

        ValidatableObject<string> _verificationCode;
        public ValidatableObject<string> VerificationCode
        {
            get => _verificationCode;
            set => SetProperty(ref _verificationCode, value);
        }

        bool _isEmailSubmitted;
        public bool IsEmailSubmitted
        {
            get => _isEmailSubmitted;
            set => SetProperty(ref _isEmailSubmitted, value);
        }

        bool _isEmailUpdateddSuccess;
        public bool IsEmailUpdateddSuccess
        {
            get => _isEmailUpdateddSuccess;
            set => SetProperty(ref _isEmailUpdateddSuccess, value);
        }

        public UpdateEmailViewModel(INavigation navigation)
        {
            this.Navigation = navigation;
            SystemUser = new SystemUserModel();
            InitValidattions();
            AddValidationRules();
            InitGenderList();
            TextChangedCommand = new Command<string>(async (Name) => await TextChanged(Name));
        }

        public void InitValidattions()
        {
            Email = new ValidatableObject<string>();
            VerificationCode = new ValidatableObject<string>();
        }
        public void AddValidationRules()
        {
            Email.Validations.Add(new IsNotNullOrEmptyRule<string> { ValidationMessage = "Email Required" });
            Email.Validations.Add(new IsValidEmailRule<string> { ValidationMessage = "Please enter valid Email" });
            VerificationCode.Validations.Add(new IsNotNullOrEmptyRule<string> { ValidationMessage = "Please enter Verification Code" });

        }

        public async Task ResetEmailRegistration()
        {
            Email = new ValidatableObject<string>();
            Email.Validations.Add(new IsNotNullOrEmptyRule<string> { ValidationMessage = "Email Required" });
            Email.Validations.Add(new IsValidEmailRule<string> { ValidationMessage = "Please enter valid Email" });
        }

        public async Task ResetVerification()
        {
            VerificationCode = new ValidatableObject<string>();
            VerificationCode.Validations.Add(new IsNotNullOrEmptyRule<string> { ValidationMessage = "Please enter Verification Code" });
        }
        public async Task<bool> EmailValid()
        {
            bool isEmailValid = Email.Validate();

            return isEmailValid;
        }
        public async Task<bool> VerificationCodeValid()
        {
            bool _isVerificationCodeValid = VerificationCode.Validate();

            return _isVerificationCodeValid;
        }
        public async Task<SystemUserVerificationModel> SendEmailVerification(SystemUserVerificationBindingModel model)
        {
            SystemUserVerificationModel result = null;
            try
            {
                result = await SystemUserVerificationService.SendEmailVerification(model);
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
        public async Task TextChanged(string Name)
        {
            switch (Name)
            {
                case "Email":
                    Email.Validate();
                    break;
                case "VerificationCode":
                    VerificationCode.HasCustomError = false;
                    VerificationCode.Validations.Add(new IsNotNullOrEmptyRule<string> { ValidationMessage = "Please enter Verification Code" });
                    VerificationCode.Validate();
                    break;
            }
        }

        public async Task<SystemUserModel> UpdateEmail(UpdateSystemUserNameBindingModel model)
        {
            SystemUserModel result = null;
            try
            {
                result = await SystemUserService.UpdateUsername(model);
                IsBusy = false;
                return result;
            }
            catch (Exception ex)
            {
                IsBusy = false;
                throw ex;
            }
        }
    }
}

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

    public class UpdateUserInfoViewModel : BaseViewModel
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

        ValidatableObject<string> _firstName;
        public ValidatableObject<string> FirstName
        {
            get => _firstName;
            set => SetProperty(ref _firstName, value);
        }

        ValidatableObject<string> _middleName;
        public ValidatableObject<string> MiddleName
        {
            get => _middleName;
            set => SetProperty(ref _middleName, value);
        }

        ValidatableObject<string> _lastName;
        public ValidatableObject<string> LastName
        {
            get => _lastName;
            set => SetProperty(ref _lastName, value);
        }
        

       ValidatableObject<EntityGenderModel> _selectedGender;
        public ValidatableObject<EntityGenderModel> SelectedGender
        {
            get => _selectedGender;
            set => SetProperty(ref _selectedGender, value);
        }

        int _selectedGenderIndex;
        public int SelectedGenderIndex
        {
            get => _selectedGenderIndex;
            set => SetProperty(ref _selectedGenderIndex, value);
        }

        ValidatableObject<DateTime> _birthDay;
        public ValidatableObject<DateTime> BirthDay
        {
            get => _birthDay;
            set => SetProperty(ref _birthDay, value);
        }

        bool _isFromProfile;
        public bool IsFromProfile
        {
            get => _isFromProfile;
            set => SetProperty(ref _isFromProfile, value);
        }

        public UpdateUserInfoViewModel(INavigation navigation)
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
            Password = new ValidatableObject<string>();
            PasswordConfirm = new ValidatableObject<string>();
            FirstName = new ValidatableObject<string>();
            MiddleName = new ValidatableObject<string>();
            LastName = new ValidatableObject<string>();
            SelectedGender = new ValidatableObject<EntityGenderModel>();
            BirthDay = new ValidatableObject<DateTime>();
            BirthDay.Value = DateTime.Now;
        }
        public void AddValidationRules()
        {
            Email.Validations.Add(new IsNotNullOrEmptyRule<string> { ValidationMessage = "Email Required" });
            Email.Validations.Add(new IsValidEmailRule<string> { ValidationMessage = "Please enter valid Email" });
            VerificationCode.Validations.Add(new IsNotNullOrEmptyRule<string> { ValidationMessage = "Please enter Verification Code" });

            Password.Validations.Add(new IsNotNullOrEmptyRule<string> { ValidationMessage = "Password Required" });
            Password.Validations.Add(new IsValidPasswordRule<string> { ValidationMessage = "Password between 8-20 characters; must contain at least one lowercase letter, one uppercase letter, one numeric digit, and one special character" });
            PasswordConfirm.Validations.Add(new IsNotNullOrEmptyRule<string> { ValidationMessage = "Confirm password required" });

            FirstName.Validations.Add(new IsNotNullOrEmptyRule<string> { ValidationMessage = "First Name Required" });
            LastName.Validations.Add(new IsNotNullOrEmptyRule<string> { ValidationMessage = "Last Name Required" });
            BirthDay.Validations.Add(new HasValidAgeRule<DateTime> { ValidationMessage = "You must be 18 years of age or older" });
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

        public async Task ResetCredentials()
        {
            Password = new ValidatableObject<string>();
            PasswordConfirm = new ValidatableObject<string>();
            Password.Validations.Add(new IsNotNullOrEmptyRule<string> { ValidationMessage = "Password Required" });
            Password.Validations.Add(new IsValidPasswordRule<string> { ValidationMessage = "Password between 8-20 characters; must contain at least one lowercase letter, one uppercase letter, one numeric digit, and one special character" });
            PasswordConfirm.Validations.Add(new IsNotNullOrEmptyRule<string> { ValidationMessage = "Confirm password required" });
        }

        public async Task ResetUserInfo()
        {
            FirstName = new ValidatableObject<string>();
            LastName = new ValidatableObject<string>();
            SelectedGender = new ValidatableObject<EntityGenderModel>();
            BirthDay = new ValidatableObject<DateTime>();
            BirthDay.Value = DateTime.Now;
            FirstName.Validations.Add(new IsNotNullOrEmptyRule<string> { ValidationMessage = "First Name Required" });
            LastName.Validations.Add(new IsNotNullOrEmptyRule<string> { ValidationMessage = "Last Name Required" });
            LastName.Validations.Add(new IsNotNullOrEmptyRule<string> { ValidationMessage = "Last Name Required" });
            SelectedGender.Validations.Add(new IsNotNullOrEmptyRule<EntityGenderModel> { ValidationMessage = "Please select gender" });
            BirthDay.Validations.Add(new HasValidAgeRule<DateTime> { ValidationMessage = "You must be 18 years of age or older" });
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
        public async Task<bool> UserInfoValid()
        {
            bool isEmailValid = FirstName.Validate();
            bool _isLastNameValid = LastName.Validate();
            bool _isGenderValid = SelectedGender.Validate();
            bool _isBirthDayValid = BirthDay.Validate();
            return isEmailValid && _isLastNameValid && _isGenderValid && _isBirthDayValid;
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
        public async Task<SystemUserModel> CreateAccount(CreateAccountSystemUserBindingModel model)
        {
            SystemUserModel result = null;
            try
            {
                result = await SystemUserService.CreateAccount(model);
                IsBusy = false;
                return result;
            }
            catch (Exception ex)
            {
                IsBusy = false;
                throw ex;
            }
        }
        public async Task<SystemUserModel> UpdateInfo(UpdateSystemUserBindingModel model)
        {
            SystemUserModel result = null;
            try
            {
                result = await SystemUserService.UpdatePersonalDetails(model);
                IsBusy = false;
                return result;
            }
            catch (Exception ex)
            {
                IsBusy = false;
                throw ex;
            }
        }
        public async Task<SystemUserModel> Login()
        {
            SystemUserModel result = null;
            try
            {
                if (await CredentialsValid())
                {
                    result = await SystemUserService.GetByCredentials(this.Email.Value, this.Password.Value);
                }
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
                case "Password":
                    Password.Validate();
                    break;
                case "PasswordConfirm":
                    PasswordConfirm.Validate();
                    break;
                case "FirstName":
                    FirstName.Validate();
                    break;
                case "LastName":
                    LastName.Validate();
                    break;
                case "BirthDay":
                    BirthDay.Validate();
                    break;
                case "SelectedGender":
                    SelectedGender.Validate();
                    break;
            }
        }
    }
}

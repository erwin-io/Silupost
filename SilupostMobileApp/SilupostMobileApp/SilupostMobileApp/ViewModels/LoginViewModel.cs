using Plugin.Toast;
using SilupostMobileApp.Common;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using SilupostMobileApp.Models;

namespace SilupostMobileApp.ViewModels
{

    public class LoginViewModel : BaseViewModel
    {
        public INavigation Navigation { get; set; }
        public Command LoginCommand { get; set; }
        public Command TextChangedCommand { get; set; }
        public SystemUserModel SystemUser { get; set; }

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
        public LoginViewModel(INavigation navigation)
        {
            this.Navigation = navigation;
            SystemUser = new SystemUserModel();
            InitValidattions();
            AddValidationRules();
            TextChangedCommand = new Command<string>(async (Name) => await TextChanged(Name));
        }

        public void InitValidattions()
        {
            Email = new ValidatableObject<string>();
            Password = new ValidatableObject<string>();
        }
        public void AddValidationRules()
        {
            Email.Validations.Add(new IsNotNullOrEmptyRule<string> { ValidationMessage = "Email Required" });
            Password.Validations.Add(new IsNotNullOrEmptyRule<string> { ValidationMessage = "Password Required" });


            //LastName.Validations.Add(new IsNotNullOrEmptyRule<string> { ValidationMessage = "Last Name Required" });
            //LastName.Validations.Add(new IsNotNullOrEmptyRule<string> { ValidationMessage = "Last Name Required" });
            //BirthDay.Validations.Add(new HasValidAgeRule<DateTime> { ValidationMessage = "You must be 18 years of age or older" });
            //PhoneNumber.Validations.Add(new IsNotNullOrEmptyRule<string> { ValidationMessage = "Phone Number Required" });
            //PhoneNumber.Validations.Add(new IsLenghtValidRule<string> { ValidationMessage = "Phone Number should have a maximun of 10 digits and minimun of 8", MaximunLenght = 10, MinimunLenght = 8 });

            //Email Validation Rules
            //Email.Item1.Validations.Add(new IsNotNullOrEmptyRule<string> { ValidationMessage = "Email Required" });
            //Email.Item1.Validations.Add(new IsValidEmailRule<string> { ValidationMessage = "Invalid Email" });
            //Email.Item2.Validations.Add(new IsNotNullOrEmptyRule<string> { ValidationMessage = "Confirm Email Required" });
            //Email.Validations.Add(new MatchPairValidationRule<string> { ValidationMessage = "Email and confirm email don't match" });
            //Email.Validations.Add(new MatchPairValidationRule<string> { ValidationMessage = "Email and confirm email don't match" });

            //Password Validation Rules
            //Password.Item1.Validations.Add(new IsNotNullOrEmptyRule<string> { ValidationMessage = "Password Required" });
            //Password.Item1.Validations.Add(new IsValidPasswordRule<string> { ValidationMessage = "Password between 8-20 characters; must contain at least one lowercase letter, one uppercase letter, one numeric digit, and one special character" });
            //Password.Item2.Validations.Add(new IsNotNullOrEmptyRule<string> { ValidationMessage = "Confirm password required" });
            //Password.Validations.Add(new MatchPairValidationRule<string> { ValidationMessage = "Password and confirm password don't match" });

            //TermsAndCondition.Validations.Add(new IsValueTrueRule<bool> { ValidationMessage = "Please accept tems and condition" });
        }
        public async Task<bool> AreFieldsValid()
        {
            bool isEmailValid = Email.Validate();
            bool isPasswordValid = Password.Validate();
            return isEmailValid && isPasswordValid;
        }

        async Task TextChanged(string Name)
        {
            switch (Name)
            {
                case "Email":
                    Email.Validate();
                    break;
                case "Password":
                    Password.Validate();
                    break;
            }
        }
        public async Task<SystemUserModel> Login()
        {
            SystemUserModel model = null;
            try
            {

                var serverStatus = await SystemConfigService.GetServerStatus();
                if (serverStatus != SilupostServerStatusEnums.ACTIVE)
                    throw new Exception(SilupostMessage.SERVER_INACTIVE);
                var result = await SystemUserService.GetByCredentials(this.Email.Value, this.Password.Value);
                if (result.Data == null && !result.IsSuccess)
                    throw new Exception(result.Message);
                model = result.Data;
                IsBusy = false;
            }
            catch (Exception ex)
            {
                IsBusy = false;
                throw ex;
            }
            return model;
        }
    }
}

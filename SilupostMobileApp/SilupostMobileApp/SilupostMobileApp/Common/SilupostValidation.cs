using SilupostMobileApp.Common.Interface;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using Xamarin.Forms;

namespace SilupostMobileApp.Common
{
    public class ValidatableObject<T> : IValidatable<T>
    {
        public ValidatableObject()
        {
            Validations = new List<IValidationRule<T>>();
            Errors = new List<string>();
            IsValid = true;
            BorderColor = Color.FromHex(SilupostTheme.TEXT_ENTRY_BORDER_COLOR);
        }
        public event PropertyChangedEventHandler PropertyChanged;

        List<IValidationRule<T>> _validations;
        public List<IValidationRule<T>> Validations
        {
            get => _validations;
            set => SetProperty(ref _validations, value);
        }


        List<string> _error;
        public List<string> Errors
        {
            get => _error;
            set => SetProperty(ref _error, value);
        }

        bool _cleanOnChange;
        public bool CleanOnChange
        {
            get => _cleanOnChange;
            set => SetProperty(ref _cleanOnChange, value);
        }

        T _value;
        public T Value
        {
            get => _value;
            set
            {
                SetProperty(ref _value, value);

                if (CleanOnChange)
                    IsValid = true;
            }
        }

        Color _borderColor;
        public Color BorderColor
        {
            get => _borderColor;
            set => SetProperty(ref _borderColor, value);
        }

        bool _isValid;
        public bool IsValid
        {
            get => _isValid;
            set => SetProperty(ref _isValid, value);
        }

        bool _hasCustomError;
        public bool HasCustomError
        {
            get => _hasCustomError;
            set => SetProperty(ref _hasCustomError, value);
        }

        public virtual bool Validate()
        {

            IEnumerable<string> errors = Validations.Where(v => !v.Check(Value))
                .Select(v => v.ValidationMessage);
            if (!HasCustomError)
            {
                Errors.Clear();
                Errors = errors.ToList();
            }

            IsValid = !Errors.Any();

            if (!IsValid)
                BorderColor = Color.FromHex("#FF5252");
            else
                BorderColor = Color.FromHex(SilupostTheme.TEXT_ENTRY_BORDER_COLOR);

            return this.IsValid;
        }
        public override string ToString()
        {
            return $"{Value}";
        }
        protected bool SetProperty<T>(ref T backingStore, T value,
            [CallerMemberName] string propertyName = "",
            Action onChanged = null)
        {
            if (EqualityComparer<T>.Default.Equals(backingStore, value))
                return false;

            backingStore = value;
            onChanged?.Invoke();
            OnPropertyChanged(propertyName);
            return true;
        }

        #region INotifyPropertyChanged
        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            var changed = PropertyChanged;
            if (changed == null)
                return;

            changed.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }

    public class ValidatablePair<T> : IValidatable<ValidatablePair<T>>
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public List<IValidationRule<ValidatablePair<T>>> Validations { get; } = new List<IValidationRule<ValidatablePair<T>>>();

        public List<string> Errors { get; set; } = new List<string>();

        public bool CleanOnChange { get; set; } = true;

        public Color BorderColor { get; set; }

        ValidatablePair<T> _value;
        public ValidatablePair<T> Value
        {
            get => _value;
            set
            {
                _value = value;

                if (CleanOnChange)
                    IsValid = true;
            }
        }

        public bool IsValid { get; set; } = true;

        public virtual bool Validate()
        {
            Errors.Clear();

            IEnumerable<string> errors = Validations.Where(v => !v.Check(Value))
                .Select(v => v.ValidationMessage);

            Errors = errors.ToList();
            IsValid = !Errors.Any();

            return this.IsValid;
        }
        public override string ToString()
        {
            return $"{Value}";
        }
        public ValidatableObject<T> Item1 { get; set; }
        public ValidatableObject<T> Item2 { get; set; }
    }
    public class IsNotNullOrEmptyRule<T> : IValidationRule<T>
    {
        public string ValidationMessage { get; set; }

        public bool Check(T value)
        {
            if (value == null)
            {
                return false;
            }

            var str = $"{value }";
            return !string.IsNullOrWhiteSpace(str);
        }
    }
    public class IsLenghtValidRule<T> : IValidationRule<T>
    {
        public string ValidationMessage { get; set; }
        public int MinimunLenght { get; set; }
        public int MaximunLenght { get; set; }

        public bool Check(T value)
        {
            if (value == null)
            {
                return false;
            }

            var str = value as string;
            return (str.Length > MinimunLenght && str.Length <= MaximunLenght);
        }
    }
    public class HasValidAgeRule<T> : IValidationRule<T>
    {
        public string ValidationMessage { get; set; }

        public bool Check(T value)
        {
            if (value is DateTime bday)
            {
                DateTime today = DateTime.Today;
                int age = today.Year - bday.Year;
                return (age >= 18);
            }

            return false;
        }
    }
    public class IsValueTrueRule<T> : IValidationRule<T>
    {
        public string ValidationMessage { get; set; }

        public bool Check(T value)
        {
            return bool.Parse($"{value}");
        }
    }
    public class IsValidEmailRule<T> : IValidationRule<T>
    {
        public string ValidationMessage { get; set; }

        public bool Check(T value)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress($"{value}");
                return addr.Address == $"{value}";
            }
            catch
            {
                return false;
            }
        }
    }
    public class IsValidPasswordRule<T> : IValidationRule<T>
    {
        public string ValidationMessage { get; set; }
        public Regex RegexPassword { get; set; } = new Regex("(?=.*[A-Z])(?=.*\\d)(?=.*[¡!@#$%*¿?\\-_.\\(\\)])[A-Za-z\\d¡!@#$%*¿?\\-\\(\\)&]{8,20}");

        public bool Check(T value)
        {
            return (RegexPassword.IsMatch($"{value}"));
        }
    }
    public class MatchPairValidationRule<T> : IValidationRule<ValidatablePair<T>>
    {
        public string ValidationMessage { get; set; }

        public bool Check(ValidatablePair<T> value)
        {
            return value.Item1.Value.Equals(value.Item2.Value);
        }
    }
}

using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

using Xamarin.Forms;

using SilupostMobileApp.Models;
using SilupostMobileApp.Services;
using SilupostMobileApp.Services.Interface;
using SilupostMobileApp.Common.Interface;
using SilupostMobileApp.Common;
using System.Threading.Tasks;
using Acr.UserDialogs;
using Plugin.Geolocator.Abstractions;

namespace SilupostMobileApp.ViewModels
{
    public class BaseViewModel : INotifyPropertyChanged
    {
        public BaseViewModel()
        {
            TextEntryFontColor = SilupostTheme.TEXT_ENTRY_FONT_COLOR;
            TextEntryLabelFocusColor = SilupostTheme.TEXT_ENTRY_LABEL_FOCUS_COLOR;
            TextEntryBorderColor = SilupostTheme.TEXT_ENTRY_BORDER_COLOR;
            TextEntryBorderFocusColor = SilupostTheme.TEXT_ENTRY_BORDER_FOCUS_COLOR;
            TextEntryBorderHoverColor = SilupostTheme.TEXT_ENTRY_BORDER_HOVER_COLOR;
            TextEntryBorderDisabledColor = SilupostTheme.TEXT_ENTRY_BORDER_DISABLED_COLOR;

            ButtonFontColor = SilupostTheme.BUTTON_FONT_COLOR;
            ButtonDownFontColor = SilupostTheme.BUTTON_DOWN_FONT_COLOR;
            ButtonUpFontColor = SilupostTheme.BUTTON_UP_FONT_COLOR;
            ButtonBackgroundColor = SilupostTheme.BUTTON_BACKGROUND_COLOR;
            ButtonDownBackgroundColor = SilupostTheme.BUTTON_DOWN_BACKGROUND_COLOR;
            ButtonUpBackgroundColor = SilupostTheme.BUTTON_UP_BACKGROUND_COLOR;

        }
        public IDataStore<Item> DataStore => DependencyService.Get<IDataStore<Item>>();
        public IBaseUrl BaseUrl => DependencyService.Get<IBaseUrl>();
        public IGeoCodeOpenCageDataService GeoCodeOpenCageDataService => DependencyService.Get<IGeoCodeOpenCageDataService>();
        public ICrimeIncidentReportService CrimeIncidentReportService => DependencyService.Get<ICrimeIncidentReportService>();
        public ICrimeIncidentReportMediaService CrimeIncidentReportMedia => DependencyService.Get<ICrimeIncidentReportMediaService>();
        public ICrimeIncidentCategoryService CrimeIncidentCategoryService => DependencyService.Get<ICrimeIncidentCategoryService>();
        public ISystemUserService SystemUserService => DependencyService.Get<ISystemUserService>();
        public ISystemUserVerificationService SystemUserVerificationService => DependencyService.Get<ISystemUserVerificationService>();
        public IApplicationActivity ApplicationActivity => DependencyService.Get<IApplicationActivity>();
        public IMediaHelpers MediaHelpers => DependencyService.Get<IMediaHelpers>();
        public IPhoneCall PhoneCall => DependencyService.Get<IPhoneCall>();
        public IFileSystem FileSystem => DependencyService.Get<IFileSystem>();
        public ISystemLookupService SystemLookupService => DependencyService.Get<ISystemLookupService>();

        #region THEME COLORS

        string _buttonFontColor = string.Empty;
        public string ButtonFontColor
        {
            get { return _buttonFontColor; }
            set { SetProperty(ref _buttonFontColor, value); }
        }


        string _buttonDownFontColor = string.Empty;
        public string ButtonDownFontColor
        {
            get { return _buttonDownFontColor; }
            set { SetProperty(ref _buttonDownFontColor, value); }
        }

        string _buttonUpFontColor = string.Empty;
        public string ButtonUpFontColor
        {
            get { return _buttonUpFontColor; }
            set { SetProperty(ref _buttonUpFontColor, value); }
        }

        string _buttonBackgroundColor = string.Empty;
        public string ButtonBackgroundColor
        {
            get { return _buttonBackgroundColor; }
            set { SetProperty(ref _buttonBackgroundColor, value); }
        }

        string _buttonDownBackgroundColor = string.Empty;
        public string ButtonDownBackgroundColor
        {
            get { return _buttonDownBackgroundColor; }
            set { SetProperty(ref _buttonDownBackgroundColor, value); }
        }

        string _buttonUpBackgroundColor = string.Empty;
        public string ButtonUpBackgroundColor
        {
            get { return _buttonUpBackgroundColor; }
            set { SetProperty(ref _buttonUpBackgroundColor, value); }
        }


        string _textEntryFontColor = string.Empty;
        public string TextEntryFontColor
        {
            get { return _textEntryFontColor; }
            set { SetProperty(ref _textEntryFontColor, value); }
        }

        string _textEntryBorderColor = string.Empty;
        public string TextEntryBorderColor
        {
            get { return _textEntryBorderColor; }
            set { SetProperty(ref _textEntryBorderColor, value); }
        }

        string _textEntryLabelFocusColor = string.Empty;
        public string TextEntryLabelFocusColor
        {
            get { return _textEntryLabelFocusColor; }
            set { SetProperty(ref _textEntryLabelFocusColor, value); }
        }
        string _textEntryBorderFocusColor = string.Empty;
        public string TextEntryBorderFocusColor
        {
            get { return _textEntryBorderFocusColor; }
            set { SetProperty(ref _textEntryBorderFocusColor, value); }
        }

        string _textEntryBorderHoverColor = string.Empty;
        public string TextEntryBorderHoverColor
        {
            get { return _textEntryBorderHoverColor; }
            set { SetProperty(ref _textEntryBorderHoverColor, value); }
        }

        string _textEntryBorderDisabledColor = string.Empty;
        public string TextEntryBorderDisabledColor
        {
            get { return _textEntryBorderDisabledColor; }
            set { SetProperty(ref _textEntryBorderDisabledColor, value); }
        }
        #endregion

        #region Constant List

        List<EntityGenderModel> genderList = new List<EntityGenderModel>();
        public List<EntityGenderModel> GenderList
        {
            get { return genderList; }
            set { SetProperty(ref genderList, value); }
        }
        #endregion

        bool noRecordFound = false;
        public bool NoRecordFound
        {
            get { return noRecordFound; }
            set { SetProperty(ref noRecordFound, value); }
        }
        public bool IsExecuting { get; set; }
        bool isBusy = false;
        public bool IsBusy
        {
            get { return isBusy; }
            set { SetProperty(ref isBusy, value); }
        }

        string title = string.Empty;
        public string Title
        {
            get { return title; }
            set { SetProperty(ref title, value); }
        }
        public async Task WaitAndExecute(int milisec, Action actionToExecute)
        {
            await Task.Delay(milisec);
            actionToExecute();
        }

        public async void InitGenderList()
        {
            var lookup = AppSettingsHelper.goLookupSettings.Where(x => x.LookupName == SilupostSystemLookupTable.ENTITY_GENDER).FirstOrDefault();
            foreach(var gender in lookup.LookupData)
            {
                GenderList.Add(new EntityGenderModel() { GenderId = int.Parse(gender.Id), GenderName = gender.Name });
            }
        }

        public IProgressDialog ProgressDialog { get; set; }

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
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            var changed = PropertyChanged;
            if (changed == null)
                return;

            changed.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}

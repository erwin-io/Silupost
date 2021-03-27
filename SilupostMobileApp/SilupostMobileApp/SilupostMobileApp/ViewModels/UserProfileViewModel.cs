using Plugin.Toast;
using SilupostMobileApp.Common;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using SilupostMobileApp.BindingModels;
using SilupostMobileApp.Models;
using System.Collections.ObjectModel;
using System.IO;

namespace SilupostMobileApp.ViewModels
{

    public class UserProfileViewModel : BaseViewModel
    {
        public INavigation Navigation { get; set; }
        public Command LoginCommand { get; set; }
        public Command TextChangedCommand { get; set; }

        private SystemUserModel _systemUser;
        public SystemUserModel SystemUser
        {
            get => _systemUser;
            set => SetProperty(ref _systemUser, value);
        }

        private ObservableCollection<UserProfileSettingModel> _userProfileSetting;
        public ObservableCollection<UserProfileSettingModel> UserProfileSetting
        {
            get => _userProfileSetting;
            set => SetProperty(ref _userProfileSetting, value);
        }

        ObservableCollection<GroupingModel<string, UserProfileSettingModel>> _groupedUserProfileSetting;
        public ObservableCollection<GroupingModel<string, UserProfileSettingModel>> GroupedUserProfileSetting
        {
            get => _groupedUserProfileSetting;
            set => SetProperty(ref _groupedUserProfileSetting, value);
        }

        public UserProfileViewModel(INavigation navigation)
        {
            this.Navigation = navigation;
            this.UserProfileSetting = new ObservableCollection<UserProfileSettingModel>();
        }

        public async Task InitUserProfile()
        {
            try
            {
                this.SystemUser = new SystemUserModel()
                {
                    SystemUserId = AppSettingsHelper.AppSettings.UserSettings.SystemUserId,
                    SystemUserType = new SystemUserTypeModel()
                    {
                        SystemUserTypeId = 2
                    },
                    ProfilePicture = new FileModel()
                    {
                        ImageSource = string.Format("{0}File/getFile?FileId={1}", AppSettingsHelper.goSILUPOST_WEBAPI_URI, AppSettingsHelper.AppSettings.UserSettings.ProfilePictureFileId??string.Empty)
                    },
                    UserName = AppSettingsHelper.AppSettings.UserSettings.SystemUserId,
                    LegalEntity = new LegalEntityModel() 
                    {
                        FullName = AppSettingsHelper.AppSettings.UserSettings.FullName
                    }
                };
            }
            catch (Exception ex)
            {
                SilupostExceptionLogger.GetError(ex);
            }
        }

        public async Task InitUserProfileSettings()
        {
            try
            {
                this.SystemUser = await this.SystemUserService.Get(AppSettingsHelper.AppSettings.UserSettings.SystemUserId);
                var settings = await CreateSettings();
                this.UserProfileSetting = new ObservableCollection<UserProfileSettingModel>();
                foreach (var item in settings)
                {
                    this.UserProfileSetting.Add(new UserProfileSettingModel()
                    {
                        SettingIconSource = item.SettingIconSource,
                        SettingGroupName = item.SettingGroupName,
                        SettingName = item.SettingName,
                        SettingValue = item.SettingValue,
                        ShowSettingValue = item.ShowSettingValue,
                        Sequence = item.Sequence
                    });
                }

                GroupedUserProfileSetting = new ObservableCollection<GroupingModel<string, UserProfileSettingModel>>();
                var sorted = from report in UserProfileSetting
                             orderby report.SettingGroupName descending
                             group report by report.SettingGroupName into reportGroup
                             select new GroupingModel<string, UserProfileSettingModel>(reportGroup.Key, reportGroup);

                //create a new collection of groups
                GroupedUserProfileSetting = new ObservableCollection<GroupingModel<string, UserProfileSettingModel>>(sorted.OrderBy(x=>x.FirstOrDefault().Sequence).ToList());
            }
            catch (Exception ex)
            {
                SilupostExceptionLogger.GetError(ex);
            }
        }

        async Task<List<UserProfileSettingModel>> CreateSettings()
        {
            return new List<UserProfileSettingModel>()
            {
                new UserProfileSettingModel()
                {
                    Sequence = 1,
                    SettingIconSource = SilupostUserProfileSettingsMenuIcon.BASIC_INFO,
                    SettingGroupName = SilupostUserProfileSettingsMenuGroup.PROFILE,
                    SettingName = SilupostUserProfileSettingsMenu.BASIC_INFO,
                    SettingValue = string.Format($"{this.SystemUser.LegalEntity.FirstName} {this.SystemUser.LegalEntity.LastName}"),
                    ShowSettingValue = true
                },
                new UserProfileSettingModel()
                {
                    Sequence = 2,
                    SettingIconSource = SilupostUserProfileSettingsMenuIcon.EMAIL,
                    SettingGroupName = SilupostUserProfileSettingsMenuGroup.ACCOUNT,
                    SettingName = SilupostUserProfileSettingsMenu.EMAIL,
                    SettingValue = this.SystemUser.UserName,
                    ShowSettingValue = true
                },
                new UserProfileSettingModel()
                {
                    Sequence = 3,
                    SettingIconSource = SilupostUserProfileSettingsMenuIcon.CHANGE_PASSWORD,
                    SettingGroupName = SilupostUserProfileSettingsMenuGroup.ACCOUNT,
                    SettingName = SilupostUserProfileSettingsMenu.CHANGE_PASSWORD,
                    SettingValue = "*****",
                    ShowSettingValue = true
                },
                //new UserProfileSettingModel()
                //{
                //    Sequence = 4,
                //    SettingIconSource = SilupostUserProfileSettingsMenuIcon.ADDITIONAL_SETTINGS,
                //    SettingGroupName = SilupostUserProfileSettingsMenuGroup.ADDITIONAL_SETTINGS,
                //    SettingName = SilupostUserProfileSettingsMenu.REPORT_SETTINGS,
                //    SettingValue = ""
                //},
                new UserProfileSettingModel()
                {
                    Sequence = 5,
                    SettingIconSource = SilupostUserProfileSettingsMenuIcon.LOGOUT,
                    SettingGroupName = SilupostUserProfileSettingsMenuGroup.ACTIONS,
                    SettingName = SilupostUserProfileSettingsMenu.LOGOUT,
                },
            };
        }
    }
}

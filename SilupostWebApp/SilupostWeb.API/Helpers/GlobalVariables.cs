using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace SilupostWeb.API.Helpers
{
    public static class GlobalVariables
    {
        public static string goApplicationName { get; set; }
        public static string goCountryStateCityAPIWebURI { get; set; }
        public static string goCountryStateCityAPIAuthToken { get; set; }
        public static string goDefaultPetConfigProfilePicPath { get; set; }
        public static string goDefaultSystemUserProfilePicPath { get; set; }
        public static string goEmailVerificationTempPath { get; set; }
        public static string goChangePasswordTempPath { get; set; }
        public static string goForgotPasswordTempPath { get; set; }
        public static string goEmailTempProfilePath { get; set; }
        public static string goSiteSupportEmail { get; set; }
        public static string goSiteSupportEmailPassword { get; set; }
        
        public static string GetApplicationConfig(string pConfigurationkey)
        {
            return ConfigurationManager.AppSettings[pConfigurationkey].ToString();
        }
    }
}
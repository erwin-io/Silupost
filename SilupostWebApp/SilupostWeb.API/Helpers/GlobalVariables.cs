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
        public static string goDefaultSystemUserProfilePicPath { get; set; }
        public static string goDefaultCrimeIncidentTypeIconFilePath { get; set; }
        public static string goDefaultEnforcementTypeIconFilePath { get; set; }
        public static string goDefaultEnforcementUnitIconFilePicPath { get; set; }
        public static string goDefaultEnforcementStationIconFilePath { get; set; }

        public static string GetApplicationConfig(string pConfigurationkey)
        {
            return ConfigurationManager.AppSettings[pConfigurationkey].ToString();
        }
    }
}
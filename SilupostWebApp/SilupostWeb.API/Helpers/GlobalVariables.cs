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
        public static string goDefaultCrimeIncidentTypeProfilePicPath { get; set; }
        public static string goDefaultEnforcementTypeProfilePicPath { get; set; }
        public static string goDefaultEnforcementUnitProfilePicPath { get; set; }
        public static string goDefaultEnforcementStationProfilePicPath { get; set; }

        public static string GetApplicationConfig(string pConfigurationkey)
        {
            return ConfigurationManager.AppSettings[pConfigurationkey].ToString();
        }
    }
}
using Plugin.Toast;
using SilupostMobileApp.Common;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using SilupostMobileApp.Models;

namespace SilupostMobileApp.ViewModels
{

    public class MainViewModel : BaseViewModel
    {

        public MainViewModel()
        {
        }

        public async Task InitLookup()
        {
            try
            {
                AppSettingsHelper.goLookupSettings = new List<LookupTableModel>();
                var tableNames = string.Join(",", new List<string>() { SilupostSystemLookupTable.ENTITY_GENDER, SilupostSystemLookupTable.SYSTEM_CONFIG_TYPE, SilupostSystemLookupTable.CRIME_INCIDENT_TYPE, SilupostSystemLookupTable.CRIME_INCIDENT_CATEGORY });
                AppSettingsHelper.goLookupSettings = await SystemLookupService.GetLookup(tableNames);
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
    }
}

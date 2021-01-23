using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SilupostWeb.Domain.ViewModel
{
    public class CrimeIncidentCategoryViewModel
    {
        public string CrimeIncidentCategoryId { get; set; }
        public string CrimeIncidentCategoryName { get; set; }
        public string CrimeIncidentCategoryDescription { get; set; }
        public CrimeIncidentTypeViewModel CrimeIncidentType { get; set; }
        public SystemRecordManagerViewModel SystemRecordManager { get; set; }
        public EntityStatusViewModel EntityStatus { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SilupostWeb.Domain.ViewModel
{
    public class CrimeIncidentTypeViewModel
    {
        public string CrimeIncidentTypeId { get; set; }
        public string CrimeIncidentTypeName { get; set; }
        public string CrimeIncidentTypeDescription { get; set; }
        public FileViewModel IconFile { get; set; }
        public SystemRecordManagerViewModel SystemRecordManager { get; set; }
        public EntityStatusViewModel EntityStatus { get; set; }
    }
}

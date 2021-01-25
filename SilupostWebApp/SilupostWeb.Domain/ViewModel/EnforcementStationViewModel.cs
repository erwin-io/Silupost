using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SilupostWeb.Domain.ViewModel
{
    public class EnforcementStationViewModel
    {
        public string EnforcementStationId { get; set; }
        public string EnforcementStationName { get; set; }
        public FileViewModel IconFile { get; set; }
        public SystemRecordManagerViewModel SystemRecordManager { get; set; }
        public EntityStatusViewModel EntityStatus { get; set; }
    }
}

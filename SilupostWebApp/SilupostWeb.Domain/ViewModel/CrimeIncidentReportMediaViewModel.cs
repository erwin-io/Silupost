using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SilupostWeb.Domain.ViewModel
{
    public class CrimeIncidentReportMediaViewModel
    {
        public string CrimeIncidentReportMediaId { get; set; }
        public DocReportMediaTypeViewModel DocReportMediaType { get; set; }
        public FileViewModel File { get; set; }
        public CrimeIncidentReportViewModel CrimeIncidentReport { get; set; }
        public string Caption { get; set; }
        public EntityStatusViewModel EntityStatus { get; set; }
    }
}

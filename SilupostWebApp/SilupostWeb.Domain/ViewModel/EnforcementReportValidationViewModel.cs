using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SilupostWeb.Domain.ViewModel
{
    public class EnforcementReportValidationViewModel
    {
        public string EnforcementReportValidationId { get; set; }
        public CrimeIncidentReportViewModel CrimeIncidentReport { get; set; }
        public EnforcementUnitViewModel EnforcementUnit { get; set; }
        public DateTime DateSubmitted { get; set; }
        public string ReportNotes { get; set; }
        public string ValidationNotes { get; set; }
        public ReportValidationStatusViewModel ReportValidationStatus { get; set; }
        public SystemRecordManagerViewModel SystemRecordManager { get; set; }
        public EntityStatusViewModel EntityStatus { get; set; }
    }
}

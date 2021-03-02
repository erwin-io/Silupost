using System;
using System.Collections.Generic;

namespace SilupostWeb.Data.Entity
{
    public class EnforcementReportValidationModel
    {
        public string EnforcementReportValidationId { get; set; }
        public CrimeIncidentReportModel CrimeIncidentReport { get; set; }
        public EnforcementUnitModel EnforcementUnit { get; set; }
        public DateTime DateSubmitted { get; set; }
        public string ReportNotes { get; set; }
        public string ValidationNotes { get; set; }
        public ReportValidationStatusModel ReportValidationStatus { get; set; }
        public SystemRecordManagerModel SystemRecordManager { get; set; }
        public EntityStatusModel EntityStatus { get; set; }
        public PageResultsModel PageResult { get; set; }
    }
}

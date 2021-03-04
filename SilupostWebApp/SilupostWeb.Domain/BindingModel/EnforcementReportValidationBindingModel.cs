using System;
using System.Collections.Generic;

namespace SilupostWeb.Domain.BindingModel
{
    public class EnforcementReportValidationBindingModel
    {
        public string EnforcementUnitId { get; set; }
        public string ReportNotes { get; set; }
    }
    public class CreateEnforcementReportValidationBindingModel : EnforcementReportValidationBindingModel
    {
        public string CrimeIncidentReportId { get; set; }
    }
    public class UpdateEnforcementReportValidationBindingModel : EnforcementReportValidationBindingModel
    {
        public string EnforcementReportValidationId { get; set; }
        public string ValidationNotes { get; set; }
        public long ReportValidationStatusId { get; set; }
    }
}

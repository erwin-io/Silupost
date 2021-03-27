using System;
using System.Collections.Generic;

namespace SilupostMobileApp.Models
{
    public class CrimeIncidentReportMediaModel
    {
        public string CrimeIncidentReportMediaId { get; set; }
        public DocReportMediaTypeModel DocReportMediaType { get; set; }
        public FileModel File { get; set; }
        public CrimeIncidentReportModel CrimeIncidentReport { get; set; }
        public string Caption { get; set; }
        public bool IsNew { get; set; }
        public EntityStatusModel EntityStatus { get; set; }
    }
}

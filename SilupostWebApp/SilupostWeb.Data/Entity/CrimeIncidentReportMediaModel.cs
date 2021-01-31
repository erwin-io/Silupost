using System;
using System.Collections.Generic;

namespace SilupostWeb.Data.Entity
{
    public class CrimeIncidentReportMediaModel
    {
        public string CrimeIncidentReportMediaId { get; set; }
        public DocReportMediaTypeModel DocReportMediaType { get; set; }
        public FileModel File { get; set; }
        public CrimeIncidentReportModel CrimeIncidentReport { get; set; }
        public string Caption { get; set; }
        public EntityStatusModel EntityStatus { get; set; }
        public PageResultsModel PageResult { get; set; }
    }
}

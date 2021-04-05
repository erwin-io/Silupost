using SilupostMobileApp.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace SilupostMobileApp.Models
{
    public class SilupostMediaModel : FileModel
    {
        public string CrimeIncidentReportMediaId { get; set; }
        public bool IsNew { get; set; }
        public string IconSource { get; set; }
        public SilupostDocReportMediaTypeEnums MediaType { get; set; }
    }
}

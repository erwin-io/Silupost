using System.Collections.Generic;

namespace SilupostWeb.Domain.BindingModel
{
    public class CrimeIncidentReportMediaBindingModel
    {
        public long DocReportMediaTypeId { get; set; }
        public string Caption { get; set; }
    }
    public class CreateCrimeIncidentReportMediaBindingModel : CrimeIncidentReportMediaBindingModel
    {
        public FileBindingModel File { get; set; }
        public string CrimeIncidentReportId { get; set; }
    }
    public class UpdateCrimeIncidentReportMediaBindingModel : CrimeIncidentReportMediaBindingModel
    {
        public string CrimeIncidentReportMediaId { get; set; }
        public FileBindingModel File { get; set; }
        public string FileId { get; set; }
        //public UpdateFileBindingModel File { get; set; }
    }
}

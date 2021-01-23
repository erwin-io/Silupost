using System.Collections.Generic;

namespace SilupostWeb.Domain.BindingModel
{
    public class CrimeIncidentTypeBindingModel
    {
        public string CrimeIncidentTypeName { get; set; }
        public string CrimeIncidentTypeDescription { get; set; }
    }
    public class CreateCrimeIncidentTypeBindingModel : CrimeIncidentTypeBindingModel
    {
        public FileBindingModel IconFile { get; set; }
    }
    public class UpdateCrimeIncidentTypeBindingModel : CrimeIncidentTypeBindingModel
    {
        public string CrimeIncidentTypeId { get; set; }
        public UpdateFileBindingModel IconFile { get; set; }
    }
}

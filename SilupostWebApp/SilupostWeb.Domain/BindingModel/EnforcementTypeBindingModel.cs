using System.Collections.Generic;

namespace SilupostWeb.Domain.BindingModel
{
    public class EnforcementTypeBindingModel
    {
        public string EnforcementTypeName { get; set; }
    }
    public class CreateEnforcementTypeBindingModel : EnforcementTypeBindingModel
    {
        public FileBindingModel IconFile { get; set; }
    }
    public class UpdateEnforcementTypeBindingModel : EnforcementTypeBindingModel
    {
        public string EnforcementTypeId { get; set; }
        public UpdateFileBindingModel IconFile { get; set; }
    }
}

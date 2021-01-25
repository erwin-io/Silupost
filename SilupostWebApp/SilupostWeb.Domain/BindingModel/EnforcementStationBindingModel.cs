using System.Collections.Generic;

namespace SilupostWeb.Domain.BindingModel
{
    public class EnforcementStationBindingModel
    {
        public string EnforcementStationName { get; set; }
    }
    public class CreateEnforcementStationBindingModel : EnforcementStationBindingModel
    {
        public FileBindingModel IconFile { get; set; }
    }
    public class UpdateEnforcementStationBindingModel : EnforcementStationBindingModel
    {
        public string EnforcementStationId { get; set; }
        public UpdateFileBindingModel IconFile { get; set; }
    }
}

using System;
using System.Collections.Generic;

namespace SilupostWeb.Domain.ViewModel
{
    public class EnforcementUnitViewModel
    {
        public string EnforcementUnitId { get; set; }
        public EnforcementTypeViewModel EnforcementType { get; set; }
        public EnforcementStationViewModel EnforcementStation { get; set; }
        public FileViewModel ProfilePicture { get; set; }
        public LegalEntityViewModel LegalEntity { get; set; }
        public SystemRecordManagerViewModel SystemRecordManager { get; set; }
        public EntityStatusViewModel EntityStatus { get; set; }
    }
}

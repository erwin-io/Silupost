using System;
using System.Collections.Generic;

namespace SilupostWeb.Data.Entity
{
    public class EnforcementUnitModel
    {
        public string EnforcementUnitId { get; set; }
        public EnforcementTypeModel EnforcementType { get; set; }
        public EnforcementStationModel EnforcementStation { get; set; }
        public FileModel ProfilePicture { get; set; }
        public LegalEntityModel LegalEntity { get; set; }
        public SystemRecordManagerModel SystemRecordManager { get; set; }
        public EntityStatusModel EntityStatus { get; set; }
        public PageResultsModel PageResult { get; set; }
    }
}

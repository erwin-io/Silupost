using System;
using System.Collections.Generic;

namespace SilupostWeb.Data.Entity
{
    public class EnforcementStationModel
    {
        public string EnforcementStationId { get; set; }
        public string EnforcementStationName { get; set; }
        public FileModel IconFile { get; set; }
        public SystemRecordManagerModel SystemRecordManager { get; set; }
        public EntityStatusModel EntityStatus { get; set; }
        public PageResultsModel PageResult { get; set; }
    }
}

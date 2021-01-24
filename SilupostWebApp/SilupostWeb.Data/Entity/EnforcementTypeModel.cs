using System;
using System.Collections.Generic;

namespace SilupostWeb.Data.Entity
{
    public class EnforcementTypeModel
    {
        public string EnforcementTypeId { get; set; }
        public string EnforcementTypeName { get; set; }
        public FileModel IconFile { get; set; }
        public SystemRecordManagerModel SystemRecordManager { get; set; }
        public EntityStatusModel EntityStatus { get; set; }
        public PageResultsModel PageResult { get; set; }
    }
}

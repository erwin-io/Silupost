using System;
using System.Collections.Generic;

namespace SilupostWeb.Data.Entity
{
    public class CrimeIncidentTypeModel
    {
        public string CrimeIncidentTypeId { get; set; }
        public string CrimeIncidentTypeName { get; set; }
        public string CrimeIncidentTypeDescription { get; set; }
        public FileModel IconFile { get; set; }
        public SystemRecordManagerModel SystemRecordManager { get; set; }
        public EntityStatusModel EntityStatus { get; set; }
        public PageResultsModel PageResult { get; set; }
    }
}

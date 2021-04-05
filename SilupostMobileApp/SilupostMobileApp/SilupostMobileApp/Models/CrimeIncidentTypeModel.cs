using System;
using System.Collections.Generic;

namespace SilupostMobileApp.Models
{
    public class CrimeIncidentTypeModel
    {
        public string CrimeIncidentTypeId { get; set; }
        public string CrimeIncidentTypeName { get; set; }
        public string CrimeIncidentTypeDescription { get; set; }
        public FileModel IconFile { get; set; }
        public SystemRecordManagerModel SystemRecordManager { get; set; }
        public EntityStatusModel EntityStatus { get; set; }
    }
}

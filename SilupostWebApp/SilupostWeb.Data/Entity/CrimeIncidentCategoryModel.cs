using System;
using System.Collections.Generic;

namespace SilupostWeb.Data.Entity
{
    public class CrimeIncidentCategoryModel
    {
        public string CrimeIncidentCategoryId { get; set; }
        public string CrimeIncidentCategoryName { get; set; }
        public string CrimeIncidentCategoryDescription { get; set; }
        public CrimeIncidentTypeModel CrimeIncidentType { get; set; }
        public SystemRecordManagerModel SystemRecordManager { get; set; }
        public EntityStatusModel EntityStatus { get; set; }
        public PageResultsModel PageResult { get; set; }
    }
}

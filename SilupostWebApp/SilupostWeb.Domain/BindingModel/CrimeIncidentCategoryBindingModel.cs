using System.Collections.Generic;

namespace SilupostWeb.Domain.BindingModel
{
    public class CrimeIncidentCategoryBindingModel
    {
        public string CrimeIncidentTypeId { get; set; }
        public string CrimeIncidentCategoryName { get; set; }
        public string CrimeIncidentCategoryDescription { get; set; }
    }
    public class UpdateCrimeIncidentCategoryBindingModel : CrimeIncidentCategoryBindingModel
    {
        public string CrimeIncidentCategoryId { get; set; }
    }
}

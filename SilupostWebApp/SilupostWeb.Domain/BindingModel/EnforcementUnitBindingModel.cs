using System;
using System.Collections.Generic;

namespace SilupostWeb.Domain.BindingModel
{
    public class EnforcementUnitBindingModel
    {
        public string FirstName { get; set; }
        public string EnforcementTypeId { get; set; }
        public string EnforcementStationId { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public long? GenderId { get; set; }
        public DateTime? BirthDate { get; set; }
        public string EmailAddress { get; set; }
        public string MobileNumber { get; set; }
    }
    public class CreateEnforcementUnitBindingModel : EnforcementUnitBindingModel
    {
        public FileBindingModel ProfilePicture { get; set; }
        public List<LegalEntityAddressBindingModel> LegalEntityAddress { get; set; }
    }
    public class UpdateEnforcementUnitBindingModel : EnforcementUnitBindingModel
    {
        public string EnforcementUnitId { get; set; }
        public UpdateFileBindingModel ProfilePicture { get; set; }
    }
}

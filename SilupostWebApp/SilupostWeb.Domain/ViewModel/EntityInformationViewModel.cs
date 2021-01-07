using System;
using System.Collections.Generic;

namespace SilupostWeb.Domain.ViewModel
{
    public class EntityInformationViewModel
    {
        public string LegalEntityId { get; set; }
        public string FullName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public string EmailAddress { get; set; }
        public EntityCivilStatusTypeViewModel CivilStatusType { get; set; }
        public EntityGenderTypeViewModel Gender { get; set; }
        public List<EntityContactInformationViewModel> Contact { get; set; }
        public DateTime BirthDate { get; set; }
        public int? Age { get; set; }
        public LocationViewModel Location { get; set; }
        public SystemRecordManagerViewModel CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public SystemRecordManagerViewModel UpdatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }

    public class EntityContactInformationViewModel
    {
        public string LegalEntityContactId { get; set; }
        public EntityInformationViewModel EntityInformation { get; set; }
        public EntityContactTypeViewModel EntityContactType { get; set; }
        public string Value { get; set; }
        public SystemRecordManagerViewModel CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public SystemRecordManagerViewModel UpdatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }

    public class EntityContactTypeViewModel
    {
        public string LegalContactTypeId { get; set; }
        public string Name { get; set; }
        public SystemRecordManagerViewModel CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public SystemRecordManagerViewModel UpdatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }

    public class EntityCivilStatusTypeViewModel
    {
        public string CivilStatusTypeId { get; set; }
        public string Name { get; set; }
        public SystemRecordManagerViewModel CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public SystemRecordManagerViewModel UpdatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }

    public class EntityGenderTypeViewModel
    {
        public string GenderId { get; set; }
        public string Name { get; set; }
        public SystemRecordManagerViewModel CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public SystemRecordManagerViewModel UpdatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}

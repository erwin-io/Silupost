using System;
using System.Collections.Generic;

namespace POSWeb.POSAdmin.Data.Entity
{
    public class EntityInformationModel
    {
        public string LegalEntityId { get; set; }
        public string FullName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public string EmailAddress { get; set; }
        public EntityCivilStatusTypeModel CivilStatusType { get; set; }
        public EntityGenderTypeModel Gender { get; set; }
        public List<EntityContactInformationModel> Contact { get; set; }
        public DateTime BirthDate { get; set; }
        public int? Age { get; set; }
        public LocationModel Location { get; set; }
        public SystemRecordManagerModel CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public SystemRecordManagerModel UpdatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }

    public class EntityContactInformationModel
    {
        public string LegalEntityContactId { get; set; }
        public EntityInformationModel EntityInformation { get; set; }
        public EntityContactTypeModel EntityContactType { get; set; }
        public string Value { get; set; }
        public SystemRecordManagerModel CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public SystemRecordManagerModel UpdatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }

    public class EntityContactTypeModel
    {
        public string LegalContactTypeId { get; set; }
        public string Name { get; set; }
        public SystemRecordManagerModel CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public SystemRecordManagerModel UpdatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }

    public class EntityCivilStatusTypeModel
    {
        public string CivilStatusTypeId { get; set; }
        public string Name { get; set; }
        public SystemRecordManagerModel CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public SystemRecordManagerModel UpdatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }

    public class EntityGenderTypeModel
    {
        public string GenderId { get; set; }
        public string Name { get; set; }
        public SystemRecordManagerModel CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public SystemRecordManagerModel UpdatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}

using System;
using System.Collections.Generic;

namespace POSWeb.POSAdmin.Domain.BindingModel
{
    public class SystemUserBindingModel
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public string EmailAddress { get; set; }
        public string CivilStatusTypeId { get; set; }
        public string GenderId { get; set; }
        public DateTime BirthDate { get; set; }
        public DateTime CreatedAt { get; set; }
    }
    public class CreateSystemUserBindingModel : SystemUserBindingModel
    {
        public long LocationId { get; set; }
        public List<EntityContactInformationBindingModel> Contact { get; set; }
    }
    public class UpdateSystemUserBindingModel : SystemUserBindingModel
    {
        public string SystemUserId { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }

    public class SystemUserAccountApprovalBindingModel
    {
        public string SystemUserId { get; set; }
        public List<SystemUserRolesBindingModel> UserRoles { get; set; }
    }
}

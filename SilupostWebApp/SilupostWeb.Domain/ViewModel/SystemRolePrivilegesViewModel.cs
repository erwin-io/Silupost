using System;

namespace SilupostWeb.Domain.ViewModel
{
    public class SystemRolePrivilegesViewModel
    {
        public string SystemRolePrivilegesId { get; set; }
        public SystemRoleViewModel SystemRole { get; set; }
        public SystemPrivilegeViewModel SystemPrivilege { get; set; }
        public bool IsAllowed { get; set; }
        public LocationViewModel Location { get; set; }
        public SystemRecordManagerViewModel CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public SystemRecordManagerViewModel UpdatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}

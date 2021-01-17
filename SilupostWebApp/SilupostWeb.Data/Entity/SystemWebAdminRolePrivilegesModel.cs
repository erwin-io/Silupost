using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SilupostWeb.Data.Entity
{
    public class SystemWebAdminRolePrivilegesModel
    {
        public string SystemWebAdminRolePrivilegesId { get; set; }
        public SystemWebAdminRoleModel SystemWebAdminRole { get; set; }
        public bool IsAllowed { get; set; }
        public LocationModel Location { get; set; }
        public SystemRecordManagerModel CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public SystemRecordManagerModel UpdatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}

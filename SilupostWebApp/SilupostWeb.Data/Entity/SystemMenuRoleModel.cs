using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POSWeb.POSAdmin.Data.Entity
{
    public class SystemMenuRoleModel
    {
        public string SystemMenuRoleId { get; set; }
        public SystemRoleModel SystemRole { get; set; }
        public SystemMenuModel SystemMenu { get; set; }
        public bool IsAllowed { get; set; }
        public LocationModel Location { get; set; }
        public SystemRecordManagerModel CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public SystemRecordManagerModel UpdatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POSWeb.POSAdmin.Domain.ViewModel
{
    public class SystemMenuRoleViewModel
    {
        public string SystemMenuRoleId { get; set; }
        public SystemRoleViewModel SystemRole { get; set; }
        public SystemMenuViewModel SystemMenu { get; set; }
        public bool IsAllowed { get; set; }
        public LocationViewModel Location { get; set; }
        public SystemRecordManagerViewModel CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public SystemRecordManagerViewModel UpdatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}

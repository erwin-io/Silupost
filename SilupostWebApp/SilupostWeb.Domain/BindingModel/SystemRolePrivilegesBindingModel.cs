using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POSWeb.POSAdmin.Domain.BindingModel
{
    public class SystemRolePrivilegesBindingModel
    {
        public string SystemRoleId { get; set; }
        public string PrivilegesId { get; set; }
        public bool IsAllowed { get; set; }
        public string CreatedBy { get; set; }
    }
}

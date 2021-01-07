using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SilupostWeb.Data.Entity
{
    public class SystemMenuRoleBindingModel
    {
        public string SystemRoleId { get; set; }
        public string SystemMenuId { get; set; }
        public long LocationId { get; set; }
        public string CreatedBy { get; set; }
    }
}

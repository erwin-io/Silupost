using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
namespace SilupostWeb.Domain.ViewModel
{
    public class SystemWebAdminUserRolesViewModel
    {
        public string SystemWebAdminUserRoleId { get; set; }
        public SystemWebAdminRoleViewModel SystemWebAdminRole { get; set; }
        public SystemRecordManagerViewModel SystemRecordManager { get; set; }
        public EntityStatusViewModel EntityStatus { get; set; }
    }
}

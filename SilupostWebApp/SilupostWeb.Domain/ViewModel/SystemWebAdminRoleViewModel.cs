using System;

namespace SilupostWeb.Domain.ViewModel
{
    public class SystemWebAdminRoleViewModel
    {
        public string SystemWebAdminRoleId { get; set; }
        public string RoleName { get; set; }
        public SystemRecordManagerViewModel SystemRecordManager { get; set; }
        public EntityStatusViewModel EntityStatus { get; set; }
    }
}

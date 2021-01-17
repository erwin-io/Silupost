using System;
using System.Collections.Generic;

namespace SilupostWeb.Domain.ViewModel
{
    public class SystemUserViewModel
    {
        public string SystemUserId { get; set; }
        public SystemUserTypeViewModel SystemUserType { get; set; }
        public FileViewModel ProfilePicture { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public LegalEntityViewModel LegalEntity { get; set; }
        public List<SystemWebAdminUserRolesViewModel> SystemWebAdminUserRoles { get; set; }
        public SystemRecordManagerViewModel SystemRecordManager { get; set; }
        public EntityStatusViewModel EntityStatus { get; set; }

        public TokenViewModel Token { get; set; }
    }
}

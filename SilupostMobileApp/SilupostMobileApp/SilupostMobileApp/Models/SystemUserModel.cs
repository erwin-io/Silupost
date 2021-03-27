using System;
using System.Collections.Generic;

namespace SilupostMobileApp.Models
{
    public class SystemUserModel
    {
        public string SystemUserId { get; set; }
        public SystemUserTypeModel SystemUserType { get; set; }
        public FileModel ProfilePicture { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public SystemTokenModel Token { get; set; }
        public LegalEntityModel LegalEntity { get; set; }
    }
}

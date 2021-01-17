﻿using System;
using System.Collections.Generic;

namespace SilupostWeb.Data.Entity
{
    public class SystemUserModel
    {
        public string SystemUserId { get; set; }
        public SystemUserTypeModel SystemUserType { get; set; }
        public FileModel ProfilePicture { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public LegalEntityModel LegalEntity { get; set; }
        public List<SystemWebAdminUserRolesModel> SystemWebAdminUserRoles { get; set; }
        public SystemRecordManagerModel SystemRecordManager { get; set; }
        public EntityStatusModel EntityStatus { get; set; }
        public PageResultsModel PageResult { get; set; }
    }
}

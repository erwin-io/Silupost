using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SilupostWeb.Web.Models
{
    public class ApplicationStateModel
    {
        public ApplicationUserModel User { get; set; }
    }

    public class ApplicationUserModel
    {
        public string UserId { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string LegalEntityId { get; set; }
        public string Firstname { get; set; }
        public string Middlename { get; set; }
        public string Lastname { get; set; }
        public string FullName { get; set; }
    }

    public class ApplicationErrorModel
    {
        public ExceptionModel Exception { get; set; }
    }

    public class ApplicationSateUserViewAccess
    {
        public List<UserViewAccess> MenuRoles { get; set; }
    }

    public class UserViewAccess
    {
        public string MenuRoleId { get; set; }
        public string MenuId { get; set; }
        public string RoleId { get; set; }
        public string ModuleName { get; set; }
        public string PageName { get; set; }
    }

    public class ApplicationActionExcecutingContextModel
    {
        public string Action { get; set; }
    }
}
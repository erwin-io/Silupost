﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SilupostWeb.Web.Models;

namespace SilupostWeb.Web.Controllers
{
    public class SystemWebAdminRolePrivilegesController : Controller
    {

        public SystemWebAdminRolePrivilegesController()
        {
        }

        //
        // GET: /Home/
        [AuthorizationPrivilegeFilter(Pagename = "System Role Privileges", DisplayName = "System Role Privileges", EnablePrivilegeFilter = true)]
        public ActionResult Index()
        {
            var page = new PageModel();
            page.MenuName = "System Role Privileges";
            page.Module = "System Admin Security";
            page.Title = "System Role Privileges";
            ViewBag.Page = page;
            return View();
        }
	}
}
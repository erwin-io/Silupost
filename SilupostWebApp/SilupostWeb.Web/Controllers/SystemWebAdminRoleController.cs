﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SilupostWeb.Web.Models;

namespace SilupostWeb.Web.Controllers
{
    public class SystemWebAdminRoleController : Controller
    {

        public SystemWebAdminRoleController()
        {
        }

        //
        // GET: /Home/
        [AuthorizationPrivilegeFilter(Pagename = "System Role", DisplayName = "System Role", EnablePrivilegeFilter = true)]
        public ActionResult Index()
        {
            var page = new PageModel();
            page.MenuName = "System Role";
            page.Module = "System Admin Security";
            page.Title = "System Role";
            ViewBag.Page = page;
            return View();
        }
	}
}
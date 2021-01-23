﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SilupostWeb.Web.Models;

namespace SilupostWeb.Web.Controllers
{
    public class SystemWebAdminMenuRolesController : Controller
    {

        public SystemWebAdminMenuRolesController()
        {
        }

        //
        // GET: /Home/
        public ActionResult Index()
        {
            var page = new PageModel();
            page.MenuName = "System Menu Roles";
            page.Module = "System Admin Security";
            page.Title = "System Menu Roles";
            ViewBag.Page = page;
            return View();
        }
	}
}
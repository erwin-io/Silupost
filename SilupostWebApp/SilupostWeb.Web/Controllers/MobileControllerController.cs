using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SilupostWeb.Web.Models;

namespace SilupostWeb.Web.Controllers
{
    public class MobileControllerController : Controller
    {

        public MobileControllerController()
        {
        }

        [AuthorizationPrivilegeFilter(Pagename = "Mobile Controller", DisplayName = "Mobile Controller", EnablePrivilegeFilter = true)]
        public ActionResult Index()
        {
            var page = new PageModel();
            page.MenuName = "Mobile Controller";
            page.Module = "Mobile Controller";
            page.Title = "Mobile Controller";
            ViewBag.Page = page;
            return View();
        }

        [AuthorizationPrivilegeFilter(Pagename = "Mobile Controller", DisplayName = "Mobile Controller", EnablePrivilegeFilter = true)]
        public ActionResult Details(string id)
        {
            var page = new PageModel();
            page.MenuName = "Mobile Controller";
            page.Module = "Mobile Controller";
            page.Title = "Mobile Controller";
            ViewBag.Page = page;

            Dictionary<string, object> appSettings = new Dictionary<string, object>();
            appSettings.Add("SystemUserId", id);
            ViewBag.AppSettings = appSettings;
            return View();
        }
    }
}
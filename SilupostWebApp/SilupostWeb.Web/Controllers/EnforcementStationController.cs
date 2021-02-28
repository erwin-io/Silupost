using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SilupostWeb.Web.Models;

namespace SilupostWeb.Web.Controllers
{
    public class EnforcementStationController : Controller
    {

        public EnforcementStationController()
        {
        }

        //
        // GET: /Home/
        [AuthorizationPrivilegeFilter(Pagename = "Enforcement Station", DisplayName = "Enforcement Station", EnablePrivilegeFilter = true)]
        public ActionResult Index()
        {
            var page = new PageModel();
            page.MenuName = "Enforcement Station";
            page.Module = "System Setup";
            page.Title = "Enforcement Station";
            ViewBag.Page = page;
            return View();
        }
	}
}
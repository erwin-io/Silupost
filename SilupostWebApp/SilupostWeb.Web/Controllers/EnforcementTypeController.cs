using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SilupostWeb.Web.Models;

namespace SilupostWeb.Web.Controllers
{
    public class EnforcementTypeController : Controller
    {

        public EnforcementTypeController()
        {
        }

        //
        // GET: /Home/
        [AuthorizationPrivilegeFilter(Pagename = "Enforcement Type", DisplayName = "Enforcement Type", EnablePrivilegeFilter = true)]
        public ActionResult Index()
        {
            var page = new PageModel();
            page.MenuName = "Enforcement Type";
            page.Module = "System Setup";
            page.Title = "Enforcement Type";
            ViewBag.Page = page;
            return View();
        }
	}
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SilupostWeb.Web.Models;

namespace SilupostWeb.Web.Controllers
{
    public class CrimeIncidentCategoryController : Controller
    {

        public CrimeIncidentCategoryController()
        {
        }

        //
        // GET: /Home/
        [AuthorizationPrivilegeFilter(Pagename = "Crime Incident Category", DisplayName = "Crime Incident Category", EnablePrivilegeFilter = true)]
        public ActionResult Index()
        {
            var page = new PageModel();
            page.MenuName = "Crime Incident Category";
            page.Module = "System Setup";
            page.Title = "Crime Incident Category";
            ViewBag.Page = page;
            return View();
        }
	}
}
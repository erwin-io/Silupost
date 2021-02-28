using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SilupostWeb.Web.Models;

namespace SilupostWeb.Web.Controllers
{
    public class CrimeIncidentTypeController : Controller
    {

        public CrimeIncidentTypeController()
        {
        }

        //
        // GET: /Home/
        [AuthorizationPrivilegeFilter(Pagename = "Crime Incident Type", DisplayName = "Crime Incident Type", EnablePrivilegeFilter = true)]
        public ActionResult Index()
        {
            var page = new PageModel();
            page.MenuName = "Crime Incident Type";
            page.Module = "System Setup";
            page.Title = "Crime Incident Type";
            ViewBag.Page = page;
            return View();
        }
	}
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SilupostWeb.Web.Models;

namespace SilupostWeb.Web.Controllers
{
    public class CrimeIncidentReportController : Controller
    {

        public CrimeIncidentReportController()
        {
        }

        //
        // GET: /Home/
        [AuthorizationPrivilegeFilter(Pagename = "Crime Incident Report", DisplayName = "Crime Incident Report", EnablePrivilegeFilter = true)]
        public ActionResult Index()
        {
            var page = new PageModel();
            page.MenuName = "Crime Incident Report";
            page.Module = "Report Library";
            page.Title = "Crime Incident Report";
            ViewBag.Page = page;
            return View();
        }
        [AuthorizationPrivilegeFilter(Pagename = "Crime Incident Report", DisplayName = "Crime Incident Report", EnablePrivilegeFilter = true)]
        public ActionResult Details(string id)
        {
            var page = new PageModel();
            page.MenuName = "Crime Incident Report";
            page.Module = "Report Library";
            page.Title = "Crime Incident Report";
            ViewBag.Page = page;

            Dictionary<string, object> appSettings = new Dictionary<string, object>();
            appSettings.Add("CrimeIncidentReportId", id);
            ViewBag.AppSettings = appSettings;
            return View();
        }
    }
}
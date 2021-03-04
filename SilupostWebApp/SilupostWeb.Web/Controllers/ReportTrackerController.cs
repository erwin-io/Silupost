using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SilupostWeb.Web.Models;

namespace SilupostWeb.Web.Controllers
{
    public class ReportTrackerController : Controller
    {

        public ReportTrackerController()
        {
        }

        [AuthorizationPrivilegeFilter(Pagename = "Report Tracker", DisplayName = "Report Tracker", EnablePrivilegeFilter = true)]
        public ActionResult Index()
        {
            var page = new PageModel();
            page.MenuName = "Report Tracker";
            page.Module = "Report Tracker";
            page.Title = "Report Tracker";
            ViewBag.Page = page;
            return View();
        }
    }
}
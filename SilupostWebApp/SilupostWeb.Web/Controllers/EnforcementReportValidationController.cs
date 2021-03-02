using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SilupostWeb.Web.Models;

namespace SilupostWeb.Web.Controllers
{
    public class EnforcementReportValidationController : Controller
    {

        public EnforcementReportValidationController()
        {
        }

        //
        // GET: /Home/
        [AuthorizationPrivilegeFilter(Pagename = "Enforcement Report Validation", DisplayName = "Enforcement Report Validation", EnablePrivilegeFilter = true)]
        public ActionResult Index()
        {
            var page = new PageModel();
            page.MenuName = "Enforcement Report Validation";
            page.Module = "Report Library";
            page.Title = "Enforcement Report Validation";
            ViewBag.Page = page;
            return View();
        }
    }
}
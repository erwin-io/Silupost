using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SilupostWeb.Web.Models;

namespace SilupostWeb.Web.Controllers
{
    public class EnforcementUnitController : Controller
    {

        public EnforcementUnitController()
        {
        }

        //
        // GET: /Home/
        public ActionResult Index()
        {
            var page = new PageModel();
            page.MenuName = "Enforcement Unit";
            page.Module = "System Setup";
            page.Title = "Enforcement Unit";
            ViewBag.Page = page;
            return View();
        }
	}
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SilupostWeb.Web.Models;

namespace SilupostWeb.Web.Controllers
{
    public class SystemUserController : Controller
    {

        public SystemUserController()
        {
        }

        //
        // GET: /Home/
        public ActionResult Index()
        {
            var page = new PageModel();
            page.MenuName = "System User";
            page.Module = "Web Admin Security";
            page.Title = "System User";
            ViewBag.Page = page;
            return View();
        }
	}
}
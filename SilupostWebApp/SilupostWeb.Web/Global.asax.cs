using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using SilupostWeb.Web.Models;
using SilupostWeb.Web.App_Start;

namespace SilupostWeb.Web
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            ApplicationSettings.gConnectionString = GlobalFunctions.ConnectionString();
            ApplicationSettings.gBranchId = GlobalFunctions.GetApplicationConfig("BranchId");
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            GlobalFunctions.ResetVisitors_Online();
        }

        protected void Application_End(object sender, EventArgs e)
        {
            //GlobalFunctions.SetCloseLog();
        }

        protected void Session_Start(object sender, EventArgs e)
        {
            GlobalFunctions.SetVisitors_Online();
        }

        protected void Session_End(object sender, EventArgs e)
        {
            GlobalFunctions.SetVisitors_Offline();
        }
    }
}

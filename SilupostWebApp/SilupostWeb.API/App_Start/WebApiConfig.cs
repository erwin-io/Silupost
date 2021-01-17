using Microsoft.Owin.Security.OAuth;
using SilupostWeb.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace SilupostWeb.API
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services
            AutoMapperConfig.Configure("SilupostWeb.Mapping");

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            config.SuppressDefaultHostAuthentication();
            config.Filters.Add(new HostAuthenticationFilter(OAuthDefaults.AuthenticationType));
        }
    }
}

using Microsoft.Owin;
using Microsoft.Owin.Security.DataHandler.Encoder;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.OAuth;
using Microsoft.Owin.Security.Jwt;
using Owin;
using SilupostWeb.Data;
using SilupostWeb.Data.Interface;
using SilupostWeb.Facade;
using SilupostWeb.Facade.Interface;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Data;
using System.Data.SqlClient;
using SilupostWeb.API.Helpers;

[assembly: OwinStartup(typeof(SilupostWeb.API.Startup))]
namespace SilupostWeb.API
{
    /// <summary>
    /// Startup
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// Configuration
        /// </summary>
        /// <param name="app"></param>
        public void Configuration(IAppBuilder app)
        {
            GlobalVariables.goIssuer = GlobalVariables.GetApplicationConfig("Issuer");
            GlobalVariables.goAudienceId = GlobalVariables.GetApplicationConfig("audienceID");
            GlobalVariables.goAudienceSecret = GlobalVariables.GetApplicationConfig("audienceSecret");
            GlobalVariables.goClientId = GlobalVariables.GetApplicationConfig("ClientId");

            ConfigureOAuth(app);
            HttpConfiguration config = new HttpConfiguration();
            
            // Web API routes
            //config.MapHttpAttributeRoutes();

            app.UseCors(Microsoft.Owin.Cors.CorsOptions.AllowAll);
            //app.MapSignalR();

            app.UseWebApi(config);
        }

        private void ConfigureOAuth(IAppBuilder app)
        {
            var issuer = GlobalVariables.goIssuer;
            var audienceId = GlobalVariables.goAudienceId;
            byte[] audienceSecret = TextEncodings.Base64Url.Decode(GlobalVariables.goAudienceSecret);

            // Api controllers with an [Authorize] attribute will be validated with JWT
            app.UseJwtBearerAuthentication(
                new JwtBearerAuthenticationOptions
                {
                    AuthenticationMode = AuthenticationMode.Active,
                    AllowedAudiences = new[] { audienceId },
                    IssuerSecurityKeyProviders = new IIssuerSecurityKeyProvider[]
                    {
                        new SymmetricKeyIssuerSecurityKeyProvider(issuer, audienceSecret)
                    }
                });


        }
    }
}
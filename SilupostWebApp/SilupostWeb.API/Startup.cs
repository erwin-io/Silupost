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
            string connectionString = Helpers.Configuration.ConnectionString();
            IDbConnection dbConnection = new SqlConnection(connectionString);
            //DAC
            ISystemUserRepository _systemUserRepository = new SystemUserDAC(dbConnection);
            //Facade
            IUserAuthFacade _userAuthFacade = new UserAuthFacade(_systemUserRepository);


            var issuer = "http://pos.azurewebsites.net";
            string audienceId = ConfigurationManager.AppSettings["audienceId"];
            byte[] audienceSecret = TextEncodings.Base64Url.Decode(ConfigurationManager.AppSettings["audienceSecret"]);

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
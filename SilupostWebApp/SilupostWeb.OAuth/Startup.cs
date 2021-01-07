using Microsoft.Owin;
using Microsoft.Owin.Security.DataHandler.Encoder;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.OAuth;
using Microsoft.Owin.Security.Jwt;
using Owin;
using SilupostWeb.OAuth.Providers;
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

[assembly: OwinStartup(typeof(SilupostWeb.OAuth.Startup))]
namespace SilupostWeb.OAuth
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

            OAuthAuthorizationServerOptions OAuthServerOptions = new OAuthAuthorizationServerOptions()
            {
                //For Dev enviroment only (on production should be AllowInsecureHttp = false)
                AllowInsecureHttp = true,
                TokenEndpointPath = new PathString("/oauth2/token"),
                AccessTokenExpireTimeSpan = TimeSpan.FromDays(1),
                Provider = new AppOAuthProvider(_userAuthFacade),
                AccessTokenFormat = new AppOAuthJWTFormat(issuer),
                RefreshTokenProvider = new AppRefreshTokenProvider()
            };

            // OAuth 2.0 Bearer Access Token Generation
            app.UseOAuthAuthorizationServer(OAuthServerOptions);

        }
    }
}
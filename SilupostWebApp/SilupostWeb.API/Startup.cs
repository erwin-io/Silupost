using Microsoft.Owin;
using Microsoft.Owin.Security.DataHandler.Encoder;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.OAuth;
using Microsoft.Owin.Security.Jwt;
using Owin;
using POSWeb.POS.API.Providers;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Http;


[assembly: OwinStartup(typeof(POSWeb.POS.API.Startup))]
namespace POSWeb.POS.API
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
            //string connectionString = Helpers.Configuration.ConnectionString();


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

            OAuthAuthorizationServerOptions OAuthServerOptions = new OAuthAuthorizationServerOptions()
            {
                //For Dev enviroment only (on production should be AllowInsecureHttp = false)
                AllowInsecureHttp = true,
                TokenEndpointPath = new PathString("/oauth2/token"),
                AccessTokenExpireTimeSpan = TimeSpan.FromDays(1),
                Provider = new AppOAuthProvider(),
                AccessTokenFormat = new AppOAuthJWTFormat(issuer),
                RefreshTokenProvider = new AppRefreshTokenProvider()
            };

            // OAuth 2.0 Bearer Access Token Generation
            app.UseOAuthAuthorizationServer(OAuthServerOptions);

        }
    }
}
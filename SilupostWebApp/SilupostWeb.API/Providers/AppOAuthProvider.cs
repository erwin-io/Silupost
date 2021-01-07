using Microsoft.Owin.Security;
using Microsoft.Owin.Security.OAuth;
using POSWeb.POS.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;

namespace POSWeb.POS.API.Providers
{
    public class AppOAuthProvider : OAuthAuthorizationServerProvider
    {

        #region CONSTRUCTORS
        public AppOAuthProvider() { 
        }
        #endregion

        public override Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            context.Validated();
            return Task.FromResult<object>(null);
        }

        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            var identity = new ClaimsIdentity("JWT");

            identity.AddClaim(new Claim(ClaimTypes.Name, context.UserName));
            identity.AddClaim(new Claim("Password", context.Password));

            var props = new AuthenticationProperties(new Dictionary<string, string>());
            props = new AuthenticationProperties(new Dictionary<string, string> { { "as:clientRefreshTokenLifeTime", "60" }, { "userId", "1" }, });


            AuthenticationTicket ticket = new AuthenticationTicket(identity, props);
            context.Validated(ticket);
        }

        public override async Task GrantRefreshToken(OAuthGrantRefreshTokenContext context)
        {
            // Change auth ticket for refresh token requests
            var newIdentity = new ClaimsIdentity(context.Ticket.Identity);

            var newTicket = new AuthenticationTicket(newIdentity, context.Ticket.Properties);
            context.Validated(newTicket);
        }
    }
}
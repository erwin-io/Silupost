using Microsoft.Owin.Security.Infrastructure;
using Newtonsoft.Json;
using SilupostWeb.OAuth.Helpers;
using SilupostWeb.OAuth.Models;
using SilupostWeb.Facade.Interface;
using SilupostWeb.Domain.ViewModel;
using SilupostWeb.Domain.BindingModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.Owin.Security;
using System.Security.Claims;
using System.Configuration;

namespace SilupostWeb.OAuth.Providers
{
    public class AppRefreshTokenProvider : IAuthenticationTokenProvider
    {

        private readonly ISystemTokenFacade _systemTokenFacade;
        #region CONSTRUCTORS
        public AppRefreshTokenProvider(ISystemTokenFacade systemTokenFacade)
        {
            _systemTokenFacade = systemTokenFacade ?? throw new ArgumentNullException(nameof(systemTokenFacade));
        }
        #endregion

        public void Create(AuthenticationTokenCreateContext context)
        {
            throw new NotImplementedException();
        }

        public async Task CreateAsync(AuthenticationTokenCreateContext context)
        {
            string userId = context.Ticket.Properties.Dictionary["SystemUserId"];
            string clientId = GlobalVariables.goClientId;
            double refreshTokenLifeTime = Convert.ToDouble(context.Ticket.Properties.Dictionary["as:clientRefreshTokenLifeTime"]);

            var refreshTokenId = Guid.NewGuid().ToString("n");

            var token = new SystemRefreshTokenBindingModel()
            {
                TokenId = Helpers.Configuration.GetHash(refreshTokenId),
                UserId = userId,
                ClientId = clientId,
                Subject = context.Ticket.Identity.Name,
                IssuedUtc = DateTime.Now,
                ExpiresUtc = DateTime.Now.AddHours(refreshTokenLifeTime),
                TokenType = "REFRESH"
            };

            context.Ticket.Properties.IssuedUtc = token.IssuedUtc;
            context.Ticket.Properties.ExpiresUtc = token.ExpiresUtc;

            token.ProtectedTicket = context.SerializeTicket();

            var tokenId = await Task.Run(() => this._systemTokenFacade.Add(token));
            if (!string.IsNullOrEmpty(tokenId))
            {
                context.SetToken(refreshTokenId);
            }
        }

        public void Receive(AuthenticationTokenReceiveContext context)
        {
            throw new NotImplementedException();
        }
        public async Task ReceiveAsync(AuthenticationTokenReceiveContext context)
        {
            string hashedTokenId = Helpers.Configuration.GetHash(context.Token);

            SystemRefreshTokenViewModel token = await Task.Run(() => this._systemTokenFacade.Find(hashedTokenId));

            if (!string.IsNullOrEmpty(token.ProtectedTicket))
            {
                context.DeserializeTicket(token.ProtectedTicket);
            }
        }
    }
}
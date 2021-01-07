using Microsoft.Owin.Security.Infrastructure;
using Newtonsoft.Json;
using POSWeb.POS.API.Helpers;
using POSWeb.POS.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace POSWeb.POS.API.Providers
{
    public class AppRefreshTokenProvider : IAuthenticationTokenProvider
    {

        public AppRefreshTokenProvider()
        {
        }

        public void Create(AuthenticationTokenCreateContext context)
        {
            throw new NotImplementedException();
        }

        public async Task CreateAsync(AuthenticationTokenCreateContext context)
        {
        }

        public void Receive(AuthenticationTokenReceiveContext context)
        {
            throw new NotImplementedException();
        }

        public async Task ReceiveAsync(AuthenticationTokenReceiveContext context)
        {
        }
    }
}
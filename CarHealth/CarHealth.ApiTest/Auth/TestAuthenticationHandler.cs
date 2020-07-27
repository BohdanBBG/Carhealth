using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace CarHealth.ApiTest.Auth
{
    public class TestAuthenticationHandler : AuthenticationHandler<TestAuthenticationOptions>
    {
        public TestAuthenticationHandler(
            IOptionsMonitor<TestAuthenticationOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock
        ) : base(options, logger, encoder, clock)
        {

        }

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            // if header presented consider endpoint as auth required
            // otherwise return NoResult
            if (this.Context.Request.Headers.ContainsKey(HeaderNames.Authorization))
            {
                var accessToken = Request.Headers["Authorization"].ToString().Replace($"{JwtBearerDefaults.AuthenticationScheme} ", "");

                if (TestAuthenticationHelper.ValidateAccessToken(accessToken))
                {
                    var identity = TestAuthenticationHelper.GetIdentityFromAccessToken(accessToken);
                    var authenticationTicket = new AuthenticationTicket(
                       new ClaimsPrincipal(identity),
                       new AuthenticationProperties(),
                       authenticationScheme: TestAuthenticationDefaults.AuthenticationScheme
                   );

                    return Task.FromResult(AuthenticateResult.Success(authenticationTicket));
                }

                return Task.FromResult(AuthenticateResult.Fail("Authorization failed."));
            }
            return Task.FromResult(AuthenticateResult.NoResult());
        }
    }
}

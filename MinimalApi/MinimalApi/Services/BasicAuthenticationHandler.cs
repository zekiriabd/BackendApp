using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System.Reflection.PortableExecutable;
using System;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;

namespace MinimalApi.Services
{
    public class BasicAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        private IConfiguration _Configuration;
        public BasicAuthenticationHandler(
            IOptionsMonitor<AuthenticationSchemeOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock,
             IConfiguration configuration
            ) : base(options, logger, encoder, clock)
        {
            _Configuration = configuration;
        }

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            var credentials = GetHTTPBasicCredentials();

            if (credentials[0] == "MyUserName" && credentials[1] == "MyPass"
                ||
                credentials[0] == "AdminUser" && credentials[1] == "AdminPass")
            {
                return Task.FromResult(AuthenticateResult.Success(new AuthenticationTicket(CreateClaimsPrincipal(credentials[0]), Scheme.Name)));
            }
            else
            {
                Response.StatusCode = 401;
                Response.Headers.Add("WWW-Authenticate", "Basic");
                return Task.FromResult(AuthenticateResult.Fail("Invalid Authorization Header"));
            }
        }

        private static ClaimsPrincipal CreateClaimsPrincipal(string username)
        {
            var claims = new[] { 
                new Claim("name", username), 
                new Claim(ClaimTypes.Role, username=="AdminUser"? "Admin" :"Basic") };
            var identity = new ClaimsIdentity(claims, "Basic");
            var claimsPrincipal = new ClaimsPrincipal(identity);
            return claimsPrincipal;
        }

        private string[] GetHTTPBasicCredentials()
        {
            var authHeader = Request.Headers["Authorization"].ToString();
            var credentialstring = Encoding.UTF8.GetString(Convert.FromBase64String(authHeader.Replace("Basic ", "")));
            var credentials = credentialstring.Split(':');
            return credentials;
        }
    }
}
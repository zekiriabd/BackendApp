using System.Security.Claims;

namespace MinimalApi.Services
{
    public class ApiKeyAuthMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IConfiguration _config;
        public ApiKeyAuthMiddleware(RequestDelegate next, IConfiguration config)
        {
            _next = next;
            _config = config;
        }
        public async Task InvokeAsync(HttpContext context)
        {
            string apiKey = context.Request.Headers["ApiKey"].FirstOrDefault();
            if (apiKey != _config["ApiKey"] && apiKey != _config["AdminApiKey"])
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await context.Response.WriteAsync("Invalid API Key");
                return;
            }
            else
            {
                var claims = new List<Claim>{
                    new Claim(ClaimTypes.Name, apiKey)
                };
                string roles = apiKey == _config["AdminApiKey"] ? "Admin" : "Basic";
                claims.AddRange(roles.Split(',').Select(role => new Claim(ClaimTypes.Role, role)));

                var identity = new ClaimsIdentity(claims, "ApiKey");
                context.User = new ClaimsPrincipal(identity);

                await _next(context);
            }
        }
    }
}
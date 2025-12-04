using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;

namespace BillingFile.API.Authentication;

public class BasicAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
{
    private readonly IConfiguration _configuration;

    public BasicAuthenticationHandler(
        IOptionsMonitor<AuthenticationSchemeOptions> options,
        ILoggerFactory logger,
        UrlEncoder encoder,
        IConfiguration configuration)
        : base(options, logger, encoder)
    {
        _configuration = configuration;
    }

    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        // Skip authentication for health check endpoint
        if (Request.Path.StartsWithSegments("/health"))
        {
            var healthClaims = new[] { new Claim(ClaimTypes.Name, "healthcheck") };
            var healthIdentity = new ClaimsIdentity(healthClaims, Scheme.Name);
            var healthPrincipal = new ClaimsPrincipal(healthIdentity);
            var healthTicket = new AuthenticationTicket(healthPrincipal, Scheme.Name);
            return Task.FromResult(AuthenticateResult.Success(healthTicket));
        }

        // Check if Authorization header exists
        if (!Request.Headers.ContainsKey("Authorization"))
        {
            return Task.FromResult(AuthenticateResult.Fail("Missing Authorization Header"));
        }

        try
        {
            var authHeader = AuthenticationHeaderValue.Parse(Request.Headers["Authorization"].ToString());
            
            if (authHeader.Scheme != "Basic")
            {
                return Task.FromResult(AuthenticateResult.Fail("Invalid Authorization Scheme"));
            }

            var credentialBytes = Convert.FromBase64String(authHeader.Parameter ?? string.Empty);
            var credentials = Encoding.UTF8.GetString(credentialBytes).Split(':', 2);
            var username = credentials[0];
            var password = credentials[1];

            // Get credentials from configuration
            var configUsername = _configuration["BasicAuth:Username"];
            var configPassword = _configuration["BasicAuth:Password"];

            // Validate credentials
            if (username == configUsername && password == configPassword)
            {
                var claims = new[] { new Claim(ClaimTypes.Name, username) };
                var identity = new ClaimsIdentity(claims, Scheme.Name);
                var principal = new ClaimsPrincipal(identity);
                var ticket = new AuthenticationTicket(principal, Scheme.Name);

                return Task.FromResult(AuthenticateResult.Success(ticket));
            }

            return Task.FromResult(AuthenticateResult.Fail("Invalid Username or Password"));
        }
        catch
        {
            return Task.FromResult(AuthenticateResult.Fail("Invalid Authorization Header"));
        }
    }

    protected override Task HandleChallengeAsync(AuthenticationProperties properties)
    {
        Response.Headers["WWW-Authenticate"] = "Basic realm=\"Billing File API\"";
        Response.StatusCode = 401;
        return Task.CompletedTask;
    }
}


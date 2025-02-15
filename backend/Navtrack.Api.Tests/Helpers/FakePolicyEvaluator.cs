using System.Security.Claims;
using System.Threading.Tasks;
using Duende.IdentityModel;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.AspNetCore.Http;

namespace Navtrack.Api.Tests.Helpers;

public sealed class FakePolicyEvaluator(string userId) : IPolicyEvaluator
{
    public Task<AuthenticateResult> AuthenticateAsync(AuthorizationPolicy policy, HttpContext context)
    {
        ClaimsPrincipal claimsPrincipal = new(new ClaimsIdentity(new[]
        {
            new Claim(JwtClaimTypes.Subject, userId)
        }, "TestScheme"));

        return Task.FromResult(AuthenticateResult.Success(new AuthenticationTicket(claimsPrincipal,
            new AuthenticationProperties(), "TestScheme")));
    }

    public Task<PolicyAuthorizationResult> AuthorizeAsync(AuthorizationPolicy policy,
        AuthenticateResult authenticationResult, HttpContext context, object? resource)
    {
        return Task.FromResult(PolicyAuthorizationResult.Success());
    }
}
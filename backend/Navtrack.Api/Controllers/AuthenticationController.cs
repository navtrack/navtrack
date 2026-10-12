using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Navtrack.Api.Services.Common.Authentication;
using Navtrack.Database.Model.Users;
using Navtrack.Database.Services.Users;
using OpenIddict.Abstractions;
using OpenIddict.Server.AspNetCore;
using static OpenIddict.Abstractions.OpenIddictConstants;

namespace Navtrack.Api.Controllers;

[ApiExplorerSettings(IgnoreApi = true)]
public class AuthenticationController(IEnumerable<IAuthenticationGrantHandler> handlers, IUserRepository users,
    IOpenIddictAuthorizationManager authorizations, IOpenIddictApplicationManager applications,
    IOpenIddictTokenManager tokens) : ControllerBase
{
    [AllowAnonymous]
    [HttpPost(ApiPaths.AuthToken)]
    public async Task<IActionResult> Exchange()
    {
        OpenIddictRequest request = HttpContext.GetOpenIddictServerRequest() ??
            throw new InvalidOperationException("The OpenIddict request is missing.");
        ClaimsPrincipal principal;
        UserEntity? user;
        if (request.IsRefreshTokenGrantType())
        {
            AuthenticateResult authentication = await HttpContext.AuthenticateAsync(
                OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
            if (!authentication.Succeeded || authentication.Principal == null) return InvalidGrant();
            principal = authentication.Principal;
            user = await users.GetById(principal.GetClaim(Claims.Subject));
            if (user == null) return InvalidGrant();
        }
        else
        {
            IAuthenticationGrantHandler? handler = handlers.SingleOrDefault(x => x.GrantType == request.GrantType);
            if (handler == null) return InvalidGrant();
            AuthenticationGrantResult result = await handler.Authenticate(request);
            if (result.Error != null)
            {
                // Preserve the application error fields consumed by the login/link-account screens.
                Dictionary<string, object> error = new()
                {
                    ["error"] = Errors.InvalidGrant,
                    ["code"] = result.Error.Code
                };
                if (result.LinkingToken != null) error["token"] = result.LinkingToken;
                Response.Headers.CacheControl = "no-store";
                Response.Headers.Pragma = "no-cache";
                return BadRequest(error);
            }
            user = await users.GetById(result.UserId);
            if (user == null) return InvalidGrant();
            principal = new ClaimsPrincipal(new ClaimsIdentity(
                OpenIddictServerAspNetCoreDefaults.AuthenticationScheme, Claims.Name, Claims.Role));
            principal.SetClaim(Claims.Subject, user.Id.ToString());
            principal.SetScopes(request.GetScopes());
            principal.SetResources(AuthenticationConstants.ApiAudience);

            object application = await applications.FindByClientIdAsync(request.ClientId!) ??
                throw new InvalidOperationException("The client is missing.");
            object authorization = await authorizations.CreateAsync(principal, user.Id.ToString(),
                (await applications.GetIdAsync(application))!, AuthorizationTypes.AdHoc, principal.GetScopes());
            principal.SetAuthorizationId(await authorizations.GetIdAsync(authorization));
        }

        principal.SetClaim(Claims.Email, user.Email);
        principal.SetDestinations(_ => [Destinations.AccessToken]);
        return SignIn(principal, OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
    }

    [Authorize]
    [HttpPost(ApiPaths.AuthLogout)]
    public async Task<IActionResult> Logout()
    {
        // Revoke the whole login session, including already-issued access tokens.
        string? authorizationId = User.GetAuthorizationId();
        if (authorizationId != null)
        {
            object? authorization = await authorizations.FindByIdAsync(authorizationId);
            if (authorization != null) await authorizations.TryRevokeAsync(authorization);
            await tokens.RevokeByAuthorizationIdAsync(authorizationId);
        }
        return NoContent();
    }

    private ForbidResult InvalidGrant() => Forbid(new AuthenticationProperties(new Dictionary<string, string?>
    {
        [OpenIddictServerAspNetCoreConstants.Properties.Error] = Errors.InvalidGrant,
        [OpenIddictServerAspNetCoreConstants.Properties.ErrorDescription] = "The credentials or session are no longer valid."
    }), OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
}

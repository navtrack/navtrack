using System.Net;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Navtrack.Api.Services.Assets;
using Navtrack.Api.Services.Exceptions;
using Navtrack.Shared.Library.DI;

namespace Navtrack.Api.Services.ActionFilters;

[Service(typeof(AuthorizeActionFilter))]
public class AuthorizeActionFilter : IAsyncAuthorizationFilter
{
    private readonly IAssetAuthorizationService assetAuthorizationService;

    public AuthorizeActionFilter(IAssetAuthorizationService assetAuthorizationService)
    {
        this.assetAuthorizationService = assetAuthorizationService;
    }

    public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
    {
        AuthorizeAssetAttribute? authorizePermissionAttribute =
            (context.ActionDescriptor as ControllerActionDescriptor)?.MethodInfo
            .GetCustomAttribute<AuthorizeAssetAttribute>();

        if (authorizePermissionAttribute != null)
        {
            string? assetId = ActionFilterHelpers.GetId("assetId", context);

            if (!string.IsNullOrEmpty(assetId))
            {
                bool hasRole = await assetAuthorizationService.CurrentUserHasRole(authorizePermissionAttribute.AssetRoleType, assetId);

                if (hasRole)
                {
                    return;
                }
            }

            throw new ApiException(HttpStatusCode.Unauthorized);
        }
    }
}
using System.Net;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Primitives;
using Navtrack.Api.Services.Assets;
using Navtrack.Api.Services.Exceptions;
using Navtrack.Api.Services.User;
using Navtrack.DataAccess.Model.Users;

namespace Navtrack.Api.Services.ActionFilters;

public class AuthorizeActionFilter : IAsyncAuthorizationFilter
{
    private readonly ICurrentUserAccessor currentUserAccessor;
    private readonly IAssetAuthorizationService assetAuthorizationService;

    public AuthorizeActionFilter(IAssetAuthorizationService assetAuthorizationService,
        ICurrentUserAccessor currentUserAccessor)
    {
        this.assetAuthorizationService = assetAuthorizationService;
        this.currentUserAccessor = currentUserAccessor;
    }

    public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
    {
        AuthorizeAssetAttribute? authorizePermissionAttribute =
            (context.ActionDescriptor as ControllerActionDescriptor)?.MethodInfo
            .GetCustomAttribute<AuthorizeAssetAttribute>();

        if (authorizePermissionAttribute != null)
        {
            string? assetId = GetId("assetId", context);

            if (!string.IsNullOrEmpty(assetId))
            {
                UserDocument currentUser = await currentUserAccessor.GetCurrentUser();
                bool hasRole = await assetAuthorizationService.CurrentUserHasRole(currentUser,
                    authorizePermissionAttribute.AssetRoleType, assetId);

                if (hasRole)
                {
                    return;
                }
            }

            throw new ApiException(HttpStatusCode.NotFound);
        }
    }

    private static string? GetId(string key, ActionContext context)
    {
        if (context.RouteData.Values.TryGetValue(key, out object? routeValue))
        {
            return routeValue?.ToString();
        }

        if (context.HttpContext.Request.Query.TryGetValue(key, out StringValues queryValue))
        {
            return queryValue;
        }

        return null;
    }
}
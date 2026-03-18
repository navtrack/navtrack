using System.Net;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Navtrack.Api.Services.Common.Exceptions;
using Navtrack.Api.Services.Common.RequestContext;
using Navtrack.Shared.Library.DI;

namespace Navtrack.Api.Services.Common.ActionFilters;

[Service(typeof(NavtrackAuthorizeActionFilter))]
public class NavtrackAuthorizeActionFilter(INavtrackRequestContextAccessor navtrackRequestContextAccessor)
    : IAsyncActionFilter
{
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        NavtrackAuthorizeAttribute? authorizeAssetAttribute =
            (context.ActionDescriptor as ControllerActionDescriptor)?.MethodInfo
            .GetCustomAttribute<NavtrackAuthorizeAttribute>();

        ValidateAssetUserRole(authorizeAssetAttribute);
        ValidateTeamUserRole(authorizeAssetAttribute);
        ValidateOrganizationUserRole(authorizeAssetAttribute);

        await next();
    }

    private void ValidateAssetUserRole(NavtrackAuthorizeAttribute? authorizeAssetAttribute)
    {
        if (authorizeAssetAttribute?.AssetUserRole != null)
        {
            bool? hasRole =
                navtrackRequestContextAccessor.NavtrackContext?.HasAssetUserRole(authorizeAssetAttribute.AssetUserRole
                    .Value);

            if (!hasRole.GetValueOrDefault())
            {
                throw new ApiException(HttpStatusCode.Forbidden);
            }
        }
    }

    private void ValidateTeamUserRole(NavtrackAuthorizeAttribute? authorizeAssetAttribute)
    {
        if (authorizeAssetAttribute?.TeamUserRole != null)
        {
            bool? hasRole =
                navtrackRequestContextAccessor.NavtrackContext?.HasTeamUserRole(authorizeAssetAttribute.TeamUserRole
                    .Value);

            if (!hasRole.GetValueOrDefault())
            {
                throw new ApiException(HttpStatusCode.Forbidden);
            }
        }
    }

    private void ValidateOrganizationUserRole(NavtrackAuthorizeAttribute? authorizeAssetAttribute)
    {
        if (authorizeAssetAttribute?.OrganizationUserRole != null)
        {
            bool? hasRole =
                navtrackRequestContextAccessor.NavtrackContext?.HasOrganizationUserRole(authorizeAssetAttribute
                    .OrganizationUserRole.Value);

            if (!hasRole.GetValueOrDefault())
            {
                throw new ApiException(HttpStatusCode.Forbidden);
            }
        }
    }
}
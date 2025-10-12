using System.Net;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Navtrack.Api.Services.Common.Context;
using Navtrack.Api.Services.Common.Exceptions;
using Navtrack.Database.Model.Assets;
using Navtrack.Database.Services.Assets;
using Navtrack.Shared.Library.DI;

namespace Navtrack.Api.Services.Common.ActionFilters;

[Service(typeof(AuthorizeAssetActionFilter))]
public class AuthorizeAssetActionFilter(
    INavtrackContextAccessor navtrackContextAccessor,
    IAssetRepository assetRepository) : IAsyncActionFilter
{
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        AuthorizeAssetAttribute? authorizeAssetAttribute =
            (context.ActionDescriptor as ControllerActionDescriptor)?.MethodInfo
            .GetCustomAttribute<AuthorizeAssetAttribute>();

        if (authorizeAssetAttribute != null)
        {
            string? assetId = ActionFilterHelpers.GetId(context.HttpContext, "assetId");

            AssetEntity? asset = await assetRepository.GetById(assetId);
            asset.Return404IfNull();

            bool hasRole =
                navtrackContextAccessor.NavtrackContext.HasAssetUserRole(asset, authorizeAssetAttribute.UserRole);

            if (!hasRole)
            {
                throw new ApiException(HttpStatusCode.Forbidden);
            }
        }

        await next();
    }
}
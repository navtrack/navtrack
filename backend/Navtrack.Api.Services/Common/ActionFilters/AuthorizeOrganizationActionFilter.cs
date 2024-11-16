using System.Net;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Navtrack.Api.Services.Common.Context;
using Navtrack.Api.Services.Common.Exceptions;

namespace Navtrack.Api.Services.Common.ActionFilters;

public class AuthorizeOrganizationActionFilter(INavtrackContextAccessor navtrackContextAccessor) : IAsyncActionFilter
{
    public Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        AuthorizeOrganizationAttribute? authorizeOrganizationAttribute =
            (context.ActionDescriptor as ControllerActionDescriptor)?.MethodInfo
            .GetCustomAttribute<AuthorizeOrganizationAttribute>();

        if (authorizeOrganizationAttribute != null)
        {
            string? organizationId = ActionFilterHelpers.GetId(context.HttpContext, "organizationId");

            bool hasRole = !string.IsNullOrEmpty(organizationId) &&
                navtrackContextAccessor.NavtrackContext.HasOrganizationUserRole(organizationId, authorizeOrganizationAttribute
                    .UserRole);

            if (!hasRole)
            {
                throw new ApiException(HttpStatusCode.Forbidden);
            }
        }

        return next();
    }
}
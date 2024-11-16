using System.Net;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Navtrack.Api.Services.Common.Context;
using Navtrack.Api.Services.Common.Exceptions;
using Navtrack.DataAccess.Model.Teams;
using Navtrack.DataAccess.Services.Teams;
using Navtrack.Shared.Library.DI;

namespace Navtrack.Api.Services.Common.ActionFilters;

[Service(typeof(AuthorizeTeamActionFilter))]
public class AuthorizeTeamActionFilter(INavtrackContextAccessor navtrackContextAccessor, ITeamRepository teamRepository)
    : IAsyncActionFilter
{
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        AuthorizeTeamAttribute? authorizeTeamAttribute =
            (context.ActionDescriptor as ControllerActionDescriptor)?.MethodInfo
            .GetCustomAttribute<AuthorizeTeamAttribute>();

        if (authorizeTeamAttribute != null)
        {
            string? teamId = ActionFilterHelpers.GetId(context.HttpContext, "teamId");

            TeamDocument? team = !string.IsNullOrEmpty(teamId) ? await teamRepository.GetById(teamId) : null;
            team.Return404IfNull();
                
            bool hasRole = navtrackContextAccessor.NavtrackContext.HasTeamUserRole(team, authorizeTeamAttribute.UserRole);

            if (!hasRole)
            {
                throw new ApiException(HttpStatusCode.Forbidden);
            }
        }

        await next();
    }
}
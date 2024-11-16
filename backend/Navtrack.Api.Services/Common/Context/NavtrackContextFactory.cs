using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Navtrack.Api.Services.Common.ActionFilters;
using Navtrack.Api.Services.Common.IdentityServer;
using Navtrack.DataAccess.Model.Users;
using Navtrack.DataAccess.Services.Users;
using Navtrack.Shared.Library.DI;

namespace Navtrack.Api.Services.Common.Context;

[Service(typeof(INavtrackContextFactory))]
public class NavtrackContextFactory(
    IHttpContextAccessor httpContextAccessor,
    IUserRepository userRepository,
    INavtrackContextAccessor navtrackContextAccessor) : INavtrackContextFactory
{
    public async Task CreateContext()
    {
        if (navtrackContextAccessor.NavtrackContext == null)
        {
            NavtrackContext navtrackContext = new()
            {
                User = await GetUser(),
                AssetId = ActionFilterHelpers.GetId(httpContextAccessor.HttpContext, "assetId"),
                OrganizationId = ActionFilterHelpers.GetId(httpContextAccessor.HttpContext, "organizationId"),
                TeamId = ActionFilterHelpers.GetId(httpContextAccessor.HttpContext, "teamId")
            };

            navtrackContextAccessor.NavtrackContext = navtrackContext;
        }
    }

    private async Task<UserDocument?> GetUser()
    {
        string? userId = httpContextAccessor.HttpContext?.User.GetId();

        UserDocument? user = userId != null ? await userRepository.GetById(userId) : null;

        return user;
    }
}
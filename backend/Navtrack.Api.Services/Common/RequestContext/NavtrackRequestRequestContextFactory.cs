using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Navtrack.Api.Services.Common.ActionFilters;
using Navtrack.Api.Services.Common.IdentityServer;
using Navtrack.Database.Model.Assets;
using Navtrack.Database.Model.Organizations;
using Navtrack.Database.Model.Teams;
using Navtrack.Database.Model.Users;
using Navtrack.Database.Services.Assets;
using Navtrack.Database.Services.Organizations;
using Navtrack.Database.Services.Teams;
using Navtrack.Database.Services.Users;
using Navtrack.Shared.Library.DI;

namespace Navtrack.Api.Services.Common.RequestContext;

[Service(typeof(INavtrackRequestContextFactory))]
public class NavtrackRequestRequestContextFactory(
    IHttpContextAccessor httpContextAccessor,
    IUserRepository userRepository,
    IAssetRepository assetRepository,
    IOrganizationRepository organizationRepository,
    ITeamRepository teamRepository,
    INavtrackRequestContextAccessor navtrackRequestContextAccessor) : INavtrackRequestContextFactory
{
    public async Task CreateContext()
    {
        if (navtrackRequestContextAccessor.NavtrackContext == null)
        {
            NavtrackRequestContext navtrackRequestContext = new();
            navtrackRequestContext.CurrentUser = await GetCurrentUser();
            navtrackRequestContext.Asset = await GetAsset();
            navtrackRequestContext.Team = await GetTeam();
            navtrackRequestContext.Organization = await GetOrganization();

            navtrackRequestContextAccessor.NavtrackContext = navtrackRequestContext;
        }
    }

    private async Task<AssetEntity?> GetAsset()
    {
        string? assetId = ActionFilterHelpers.GetId(httpContextAccessor.HttpContext, "assetId");
        
        AssetEntity? asset = await assetRepository.GetById(assetId);
        
        return asset;
    }

    private async Task<OrganizationEntity?> GetOrganization()
    {
        string? organizationId = ActionFilterHelpers.GetId(httpContextAccessor.HttpContext, "organizationId");
        
        OrganizationEntity? organization = await organizationRepository.GetById(organizationId);
        
        return organization;
    }

    private async Task<TeamEntity?> GetTeam()
    {
        string? teamId = ActionFilterHelpers.GetId(httpContextAccessor.HttpContext, "teamId");
        
        TeamEntity? team = await teamRepository.GetById(teamId);
        
        return team;
    }

    private async Task<UserEntity?> GetCurrentUser()
    {
        string? userId = httpContextAccessor.HttpContext?.User.GetId();

        UserEntity? user = await userRepository.GetById(userId);

        return user;
    }
}
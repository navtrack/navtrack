using System;
using System.Linq;
using Navtrack.Database.Model.Assets;
using Navtrack.Database.Model.Organizations;
using Navtrack.Database.Model.Teams;
using Navtrack.Database.Model.Users;

namespace Navtrack.Api.Services.Common.Context;

public class NavtrackContext
{
    public UserEntity? User { get; init; }
    public string? OrganizationId { get; set; }
    public string? AssetId { get; set; }
    public string? TeamId { get; set; }

    public bool HasAssetUserRole(AssetEntity? asset, AssetUserRole userRole)
    {
        if (asset == null || User == null)
        {
            return false;
        }

        if (HasOrganizationUserRole(asset.OrganizationId, OrganizationUserRole.Owner))
        {
            return true;
        }

        AssetUserEntity? userAsset = User.AssetUsers.FirstOrDefault(x => x.AssetId == asset.Id);

        return userRole switch
        {
            AssetUserRole.Owner => userAsset?.UserRole == AssetUserRole.Owner,
            AssetUserRole.Viewer => userAsset?.UserRole is AssetUserRole.Owner or AssetUserRole.Viewer ||
                                    User.Teams.Any(x => asset.TeamAssets.Any(y => y.TeamId == x.Id)),
            _ => false
        };
    }

    public bool HasTeamUserRole(TeamEntity team, TeamUserRole userRole)
    {
        if (HasOrganizationUserRole(team.OrganizationId, OrganizationUserRole.Owner))
        {
            return true;
        }

        TeamUserEntity? userTeam = User?.TeamUsers.FirstOrDefault(x => x.TeamId == team.Id);

        return userRole switch
        {
            TeamUserRole.Owner => userTeam?.UserRole == TeamUserRole.Owner,
            TeamUserRole.Member => userTeam?.UserRole is TeamUserRole.Owner or TeamUserRole.Member,
            _ => false
        };
    }

    public bool HasOrganizationUserRole(Guid organizationId, OrganizationUserRole userRole)
    {
        OrganizationUserEntity? userOrganization =
            User?.OrganizationUsers.FirstOrDefault(x => x.OrganizationId == organizationId);

        return userRole switch
        {
            OrganizationUserRole.Owner => userOrganization?.UserRole == OrganizationUserRole.Owner,
            OrganizationUserRole.Member => userOrganization?.UserRole is OrganizationUserRole.Owner
                or OrganizationUserRole.Member,
            _ => false
        };
    }
}
using System.Linq;
using MongoDB.Bson;
using Navtrack.DataAccess.Model.Assets;
using Navtrack.DataAccess.Model.Organizations;
using Navtrack.DataAccess.Model.Teams;
using Navtrack.DataAccess.Model.Users;

namespace Navtrack.Api.Services.Common.Context;

public class NavtrackContext
{
    public UserDocument? User { get; init; }
    public string? OrganizationId { get; set; }
    public string? AssetId { get; set; }
    public string? TeamId { get; set; }

    public bool HasAssetUserRole(AssetDocument? asset, AssetUserRole userRole)
    {
        if (asset == null || User == null)
        {
            return false;
        }

        if (HasOrganizationUserRole(asset.OrganizationId.ToString(), OrganizationUserRole.Owner))
        {
            return true;
        }

        UserAssetElement? userAsset = User.Assets?.FirstOrDefault(x =>
            x.AssetId == asset.Id);

        return userRole switch
        {
            AssetUserRole.Owner => userAsset?.UserRole == AssetUserRole.Owner,
            AssetUserRole.Viewer => userAsset?.UserRole is AssetUserRole.Owner or AssetUserRole.Viewer,
            _ => false
        };
    }

    public bool HasTeamUserRole(TeamDocument? team, TeamUserRole userRole)
    {
        if (team == null || User == null)
        {
            return false;
        }

        if (HasOrganizationUserRole(team.OrganizationId.ToString(), OrganizationUserRole.Owner))
        {
            return true;
        }

        UserTeamElement? userTeam = User.Teams?.FirstOrDefault(x =>
            x.TeamId == team.Id);

        return userRole switch
        {
            TeamUserRole.Owner => userTeam?.UserRole == TeamUserRole.Owner,
            TeamUserRole.Member => userTeam?.UserRole is TeamUserRole.Owner or TeamUserRole.Member,
            _ => false
        };
    }
    
    public bool HasOrganizationUserRole(string organizationId, OrganizationUserRole userRole)
    {
        if (!ObjectId.TryParse(organizationId, out ObjectId organizationObjectId))
        {
            return false;
        }
     
        UserOrganizationElement? userOrganization = User?.Organizations?.FirstOrDefault(x =>
            x.OrganizationId == organizationObjectId);

        return userRole switch
        {
            OrganizationUserRole.Owner => userOrganization?.UserRole == OrganizationUserRole.Owner,
            OrganizationUserRole.Member => userOrganization?.UserRole is OrganizationUserRole.Owner
                or OrganizationUserRole.Member,
            _ => false
        };
    }
}
using System;
using System.Linq;
using Navtrack.Database.Model.Assets;
using Navtrack.Database.Model.Organizations;
using Navtrack.Database.Model.Teams;
using Navtrack.Database.Model.Users;

namespace Navtrack.Api.Services.Common.RequestContext;

public class NavtrackRequestContext
{
    public UserEntity? CurrentUser { get; set; }
    public OrganizationEntity? Organization { get; set; }
    public Guid? CurrentOrganizationId => Organization?.Id ?? Asset?.OrganizationId ?? Team?.OrganizationId;
    public AssetEntity? Asset { get; set; }
    public TeamEntity? Team { get; set; }
    
    

    public bool HasAssetUserRole(AssetUserRole userRole)
    {
        if (Asset == null || CurrentUser == null)
        {
            return false;
        }

        if (HasOrganizationUserRole(OrganizationUserRole.Owner))
        {
            return true;
        }

        AssetUserEntity? userAsset = CurrentUser.AssetUsers.FirstOrDefault(x => x.AssetId == Asset.Id);

        return userRole switch
        {
            AssetUserRole.Owner => userAsset?.UserRole == AssetUserRole.Owner,
            AssetUserRole.Viewer => userAsset?.UserRole is AssetUserRole.Owner or AssetUserRole.Viewer ||
                                    CurrentUser.Teams.Any(x => Asset.TeamAssets.Any(y => y.TeamId == x.Id)),
            _ => false
        };
    }

    public bool HasTeamUserRole(TeamUserRole userRole)
    {
        if (Team != null && HasOrganizationUserRole(OrganizationUserRole.Owner))
        {
            return true;
        }

        TeamUserEntity? userTeam = CurrentUser?.TeamUsers.FirstOrDefault(x => x.TeamId == Team?.Id);

        return userRole switch
        {
            TeamUserRole.Owner => userTeam?.UserRole == TeamUserRole.Owner,
            TeamUserRole.Member => userTeam?.UserRole is TeamUserRole.Owner or TeamUserRole.Member,
            _ => false
        };
    }

    public bool HasOrganizationUserRole(OrganizationUserRole userRole)
    {
        OrganizationUserEntity? userOrganization =
            CurrentUser?.OrganizationUsers.FirstOrDefault(x => x.OrganizationId == CurrentOrganizationId);

        return userRole switch
        {
            OrganizationUserRole.Owner => userOrganization?.UserRole == OrganizationUserRole.Owner,
            OrganizationUserRole.Member => userOrganization?.UserRole is OrganizationUserRole.Owner
                or OrganizationUserRole.Member,
            _ => false
        };
    }
}
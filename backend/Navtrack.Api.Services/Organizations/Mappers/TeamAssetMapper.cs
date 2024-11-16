using System.Linq;
using Navtrack.Api.Model.Teams;
using Navtrack.DataAccess.Model.Assets;
using Navtrack.DataAccess.Model.Teams;

namespace Navtrack.Api.Services.Organizations.Mappers;

public static class TeamAssetMapper
{
    public static TeamAsset Map(AssetDocument asset, TeamDocument team)
    {
        AssetTeamElement assetTeam = asset.Teams!.First(x => x.TeamId == team.Id);
        
        return new TeamAsset
        {
            Name = asset.Name,
            CreatedDate = assetTeam.CreatedDate,
            AssetId = asset.Id.ToString()
        };
    }
}
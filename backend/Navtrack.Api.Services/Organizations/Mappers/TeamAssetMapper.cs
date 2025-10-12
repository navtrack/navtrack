using Navtrack.Api.Model.Teams;
using Navtrack.Database.Model.Teams;

namespace Navtrack.Api.Services.Organizations.Mappers;

public static class TeamAssetMapper
{
    public static TeamAssetModel Map(TeamAssetEntity source)
    {
        return new TeamAssetModel
        {
            Name = source.Asset.Name,
            CreatedDate = source.CreatedDate,
            AssetId = source.AssetId.ToString()
        };
    }
}
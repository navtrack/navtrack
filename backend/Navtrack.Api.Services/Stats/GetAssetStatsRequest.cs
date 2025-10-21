using Navtrack.Api.Model.Stats;

namespace Navtrack.Api.Services.Stats;

public class GetAssetStatsRequest
{
    public string AssetId { get; set; }
    public AssetStatsPeriod Period { get; set; }
}
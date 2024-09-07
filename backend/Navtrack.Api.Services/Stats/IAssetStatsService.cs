using System.Threading.Tasks;
using Navtrack.Api.Model.Stats;

namespace Navtrack.Api.Services.Stats;

public interface IAssetStatsService
{
    Task<AssetStatsListModel> GetStats(string assetId);
}
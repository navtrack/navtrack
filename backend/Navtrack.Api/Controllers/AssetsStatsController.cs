using Navtrack.Api.Services.Stats;
using Navtrack.Api.Shared.Controllers;

namespace Navtrack.Api.Controllers;

public class AssetsStatsController(IAssetStatsService service) : AssetsStatsControllerBase(service);
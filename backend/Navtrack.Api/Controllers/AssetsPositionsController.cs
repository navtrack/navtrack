using Navtrack.Api.Services.Positions;
using Navtrack.Api.Shared.Controllers;

namespace Navtrack.Api.Controllers;

public class AssetsPositionsController(IPositionService positionService)
    : AssetsPositionsControllerBase(positionService);
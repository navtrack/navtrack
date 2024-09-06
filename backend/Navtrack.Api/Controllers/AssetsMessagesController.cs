using Navtrack.Api.Services.Positions;
using Navtrack.Api.Shared.Controllers;

namespace Navtrack.Api.Controllers;

public class AssetsMessagesController(IPositionService positionService)
    : AssetsMessagesControllerBase(positionService);
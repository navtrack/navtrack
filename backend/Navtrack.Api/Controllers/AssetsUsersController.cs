using Navtrack.Api.Services.Assets;
using Navtrack.Api.Shared.Controllers;

namespace Navtrack.Api.Controllers;

public class AssetsUsersController : AssetsUsersControllerBase
{
    public AssetsUsersController(IAssetService assetService) : base(assetService)
    {
    }
}
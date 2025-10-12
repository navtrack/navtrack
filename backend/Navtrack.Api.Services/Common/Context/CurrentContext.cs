using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Navtrack.Api.Services.Common.ActionFilters;
using Navtrack.Database.Model.Assets;
using Navtrack.Database.Services.Assets;
using Navtrack.Shared.Library.DI;

namespace Navtrack.Api.Services.Common.Context;

[Service(typeof(ICurrentContext))]
public class CurrentContext(IHttpContextAccessor httpContextAccessor, IAssetRepository assetRepository)
    : ICurrentContext
{
    private AssetEntity? asset;

    public async Task<AssetEntity?> GetCurrentAsset()
    {
        if (asset == null && httpContextAccessor.HttpContext != null)
        {
            string? assetId = ActionFilterHelpers.GetId(httpContextAccessor.HttpContext, "assetId");

            asset = await assetRepository.GetById(assetId);
        }

        return asset;
    }
}
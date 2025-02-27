using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Navtrack.Api.Services.Common.ActionFilters;
using Navtrack.DataAccess.Model.Assets;
using Navtrack.DataAccess.Services.Assets;
using Navtrack.Shared.Library.DI;

namespace Navtrack.Api.Services.Common.Context;

[Service(typeof(ICurrentContext))]
public class CurrentContext(IHttpContextAccessor httpContextAccessor, IAssetRepository assetRepository)
    : ICurrentContext
{
    private AssetDocument? asset;

    public async Task<AssetDocument?> GetCurrentAsset()
    {
        if (asset == null && httpContextAccessor.HttpContext != null)
        {
            string? assetId = ActionFilterHelpers.GetId(httpContextAccessor.HttpContext, "assetId");

            if (!string.IsNullOrEmpty(assetId))
            {
                asset = await assetRepository.GetById(assetId);
            }
        }

        return asset;
    }
}
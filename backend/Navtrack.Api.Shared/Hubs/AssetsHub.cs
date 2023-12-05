using System.Threading.Tasks;
using IdentityServer4;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Navtrack.Api.Model.Assets;
using Navtrack.Api.Model.Common;
using Navtrack.Api.Services.Assets;

namespace Navtrack.Api.Shared.Hubs;

[Authorize(IdentityServerConstants.LocalApi.PolicyName)]
public class AssetsHub(IAssetService service) : Hub
{
    public async Task<ListModel<AssetModel>> GetAll()
    {
        ListModel<AssetModel> assets = await service.GetAssets();

        return assets;
    }
}
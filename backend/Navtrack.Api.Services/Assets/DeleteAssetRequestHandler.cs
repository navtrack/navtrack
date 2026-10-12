using System.Threading.Tasks;
using Navtrack.Api.Services.Common.Exceptions;
using Navtrack.Api.Services.Requests;
using Navtrack.Database.Model.Assets;
using Navtrack.Database.Services.Assets;
using Navtrack.Database.Services.Devices;
using Navtrack.Database.Services.Organizations;
using Navtrack.Database.Services.Users;
using Navtrack.Shared.Library.DI;

namespace Navtrack.Api.Services.Assets;

[Service(typeof(IRequestHandler<DeleteAssetRequest>))]
public class DeleteAssetRequestHandler(
    IAssetRepository assetRepository,
    IDeviceRepository deviceRepository,
    IDeviceMessageRepository deviceMessageRepository,
    IUserRepository userRepository,
    IOrganizationRepository organizationRepository) : BaseRequestHandler<DeleteAssetRequest>
{
    public override async Task Handle(DeleteAssetRequest request)
    {
        AssetEntity? asset = await assetRepository.GetById(request.AssetId);
        asset.Return404IfNull();

        await assetRepository.Delete(asset);
        await deviceRepository.DeleteByAssetId(asset.Id);
        await deviceMessageRepository.DeleteByAssetId(asset.Id);
        await userRepository.RemoveAssetFromUsers(asset.Id);
        await organizationRepository.UpdateAssetsCount(asset.OrganizationId);
    }
}

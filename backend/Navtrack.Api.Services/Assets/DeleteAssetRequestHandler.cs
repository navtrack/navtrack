using System.Threading.Tasks;
using Navtrack.Api.Services.Assets.Events;
using Navtrack.Api.Services.Common.Exceptions;
using Navtrack.Api.Services.Requests;
using Navtrack.Database.Model.Assets;
using Navtrack.Database.Services.Assets;
using Navtrack.Database.Services.Devices;
using Navtrack.Database.Services.Organizations;
using Navtrack.Database.Services.Users;
using Navtrack.Shared.Library.DI;
using Navtrack.Shared.Library.Events;

namespace Navtrack.Api.Services.Assets;

[Service(typeof(IRequestHandler<DeleteAssetRequest>))]
public class DeleteAssetRequestHandler(
    IAssetRepository assetRepository,
    IDeviceRepository deviceRepository,
    IDeviceMessageRepository deviceMessageRepository,
    IUserRepository userRepository,
    IOrganizationRepository organizationRepository) : BaseRequestHandler<DeleteAssetRequest>
{
    private AssetEntity? asset;

    public override async Task Validate(RequestValidationContext<DeleteAssetRequest> context)
    {
        asset = await assetRepository.GetById(context.Request.AssetId);
        asset.Return404IfNull();
    }

    public override async Task Handle(DeleteAssetRequest request)
    {
        await assetRepository.Delete(asset!);
        await deviceRepository.DeleteByAssetId(asset!.Id);
        await deviceMessageRepository.DeleteByAssetId(asset!.Id);
        await userRepository.RemoveAssetFromUsers(asset!.Id);
        await organizationRepository.UpdateAssetsCount(asset!.OrganizationId);
    }

    public override IEvent GetEvent(DeleteAssetRequest request)
    {
        return new AssetDeletedEvent(asset!.Id.ToString(), asset!.OrganizationId.ToString());
    }
}
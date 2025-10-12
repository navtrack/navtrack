using System.Threading.Tasks;
using Navtrack.Api.Model.Common;
using Navtrack.Api.Model.Errors;
using Navtrack.Api.Services.Assets.Events;
using Navtrack.Api.Services.Assets.Mappers;
using Navtrack.Api.Services.Common.Context;
using Navtrack.Api.Services.Common.Exceptions;
using Navtrack.Api.Services.Devices.Mappers;
using Navtrack.Api.Services.Requests;
using Navtrack.Database.Model.Assets;
using Navtrack.Database.Model.Devices;
using Navtrack.Database.Model.Organizations;
using Navtrack.Database.Services.Assets;
using Navtrack.Database.Services.Devices;
using Navtrack.Database.Services.Organizations;
using Navtrack.Shared.Library.DI;
using Navtrack.Shared.Library.Events;

namespace Navtrack.Api.Services.Assets;

[Service(typeof(IRequestHandler<CreateAssetRequest, Entity>))]
public class CreateAssetRequestHandler(
    IAssetRepository assetRepository,
    IDeviceTypeRepository deviceTypeRepository,
    IDeviceRepository deviceRepository,
    INavtrackContextAccessor navtrackContextAccessor,
    IOrganizationRepository organizationRepository)
    : BaseRequestHandler<CreateAssetRequest, Entity>
{
    private OrganizationEntity? organization;
    private DeviceType? deviceType;

    public override async Task Validate(RequestValidationContext<CreateAssetRequest> context)
    {
        organization = await organizationRepository.GetById(context.Request.OrganizationId);
        organization.Return404IfNull();

        context.ValidationException.AddErrorIfTrue(
            await assetRepository.NameIsUsed(organization.Id, context.Request.Model.Name),
            nameof(context.Request.Model.Name),
            ApiErrorCodes.Asset_000002_NameAlreadyUsed);

        deviceType = deviceTypeRepository.GetById(context.Request.Model.DeviceTypeId);

        context.ValidationException.AddErrorIfNull(
            deviceType,
            nameof(context.Request.Model.DeviceTypeId),
            ApiErrorCodes.Device_000001_DeviceTypeInvalid);

        context.ValidationException.AddErrorIfTrue(
            await deviceRepository.SerialNumberIsUsed(context.Request.Model.SerialNumber, deviceType.Protocol.Port),
            nameof(context.Request.Model.SerialNumber),
            ApiErrorCodes.Device_000002_SerialNumberUsed);
    }

    public override async Task<Entity> Handle(CreateAssetRequest request)
    {
        AssetEntity asset = await CreateAsset(request);

        return new Entity(asset.Id.ToString());
    }

    private async Task<AssetEntity> CreateAsset(CreateAssetRequest source)
    {
        AssetEntity asset = AssetEntityMapper.Map(organization.Id, source.Model, navtrackContextAccessor.NavtrackContext.User.Id);
        await assetRepository.Add(asset);

        DeviceEntity device = DeviceDocumentMapper.Map(asset, navtrackContextAccessor.NavtrackContext.User.Id,
            source.Model.SerialNumber, deviceType!);
        await deviceRepository.Add(device);

        await assetRepository.SetActiveDevice(asset.Id, device.Id);

        await organizationRepository.UpdateAssetsCount(asset.OrganizationId);

        return asset;
    }
    
    public override IEvent GetEvent(CreateAssetRequest _, Entity result) =>
        new AssetCreatedEvent(result.Id, organization!.Id.ToString());
}
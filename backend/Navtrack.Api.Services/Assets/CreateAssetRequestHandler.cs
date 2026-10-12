using System.Threading.Tasks;
using Navtrack.Api.Model.Common;
using Navtrack.Api.Model.Errors;
using Navtrack.Api.Services.Assets.Mappers;
using Navtrack.Api.Services.Common.Exceptions;
using Navtrack.Api.Services.Common.RequestContext;
using Navtrack.Api.Services.Devices.Mappers;
using Navtrack.Api.Services.Requests;
using Navtrack.Database.Model.Assets;
using Navtrack.Database.Model.Devices;
using Navtrack.Database.Model.Organizations;
using Navtrack.Database.Services.Assets;
using Navtrack.Database.Services.Devices;
using Navtrack.Database.Services.Organizations;
using Navtrack.Shared.Library.DI;

namespace Navtrack.Api.Services.Assets;

[Service(typeof(IRequestHandler<CreateAssetRequest, Entity>))]
public class CreateAssetRequestHandler(
    IAssetRepository assetRepository,
    IDeviceTypeRepository deviceTypeRepository,
    IDeviceRepository deviceRepository,
    INavtrackRequestContextAccessor navtrackRequestContextAccessor,
    IOrganizationRepository organizationRepository)
    : BaseRequestHandler<CreateAssetRequest, Entity>
{
    public override async Task<Entity> Handle(CreateAssetRequest request)
    {
        OrganizationEntity? organization = await organizationRepository.GetById(request.OrganizationId);
        organization.Return404IfNull();

        DeviceType? deviceType = deviceTypeRepository.GetById(request.Model.DeviceTypeId);
        ValidationApiException validationException = new();

        validationException.AddErrorIfTrue(
            await assetRepository.NameIsUsed(organization.Id, request.Model.Name),
            nameof(request.Model.Name),
            ApiErrorCodes.Asset_NameAlreadyUsed);

        validationException.AddErrorIfNull(
            deviceType,
            nameof(request.Model.DeviceTypeId),
            ApiErrorCodes.Device_DeviceTypeInvalid);

        validationException.ThrowIfInvalid();

        validationException.AddErrorIfTrue(
            await deviceRepository.SerialNumberIsUsed(request.Model.SerialNumber, deviceType.Protocol.Port),
            nameof(request.Model.SerialNumber),
            ApiErrorCodes.Device_SerialNumberUsed);

        validationException.ThrowIfInvalid();

        AssetEntity asset = await CreateAsset(request, organization, deviceType);

        return new Entity(asset.Id.ToString());
    }

    private async Task<AssetEntity> CreateAsset(CreateAssetRequest source, OrganizationEntity organization,
        DeviceType deviceType)
    {
        AssetEntity asset = AssetEntityMapper.Map(organization.Id, source.Model, navtrackRequestContextAccessor.NavtrackContext.CurrentUser.Id);
        await assetRepository.Add(asset);

        DeviceEntity device = DeviceDocumentMapper.Map(asset, navtrackRequestContextAccessor.NavtrackContext.CurrentUser.Id,
            source.Model.SerialNumber, deviceType);
        await deviceRepository.Add(device);

        await assetRepository.SetActiveDevice(asset.Id, device.Id);

        await organizationRepository.UpdateAssetsCount(asset.OrganizationId);

        return asset;
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Navtrack.Api.Model.Devices;
using Navtrack.Api.Services.Common.Exceptions;
using Navtrack.Api.Services.Devices.Mappers;
using Navtrack.Api.Services.Requests;
using Navtrack.Database.Model.Assets;
using Navtrack.Database.Model.Devices;
using Navtrack.Database.Services.Assets;
using Navtrack.Database.Services.Devices;
using Navtrack.Shared.Library.DI;
using DeviceType = Navtrack.Database.Model.Devices.DeviceType;

namespace Navtrack.Api.Services.Devices;

[Service(typeof(IRequestHandler<GetAssetDevicesRequest, Model.Common.ListModel<DeviceModel>>))]
public class GetAssetDevicesRequestHandler(
    IAssetRepository assetRepository,
    IDeviceRepository deviceRepository,
    IDeviceTypeRepository deviceTypeRepository,
    IDeviceMessageRepository deviceMessageRepository)
    : BaseRequestHandler<GetAssetDevicesRequest, Model.Common.ListModel<DeviceModel>>
{
    private AssetEntity? asset;

    public override async Task Validate(RequestValidationContext<GetAssetDevicesRequest> context)
    {
        asset = await assetRepository.GetById(context.Request.AssetId);
        asset.Return404IfNull();
    }
    
    public override async Task<Model.Common.ListModel<DeviceModel>> Handle(GetAssetDevicesRequest request)
    {
        List<DeviceEntity> devices = await deviceRepository.GetDevicesByAssetId(asset.Id);
        List<DeviceType> deviceTypes = deviceTypeRepository
            .GetDeviceTypes()
            .Where(x => devices.Any(y => y.DeviceTypeId == x.Id))
            .ToList();

        Dictionary<Guid, int> locationCount = await deviceMessageRepository
            .GetMessagesCountByDeviceIds(devices.Select(x => x.Id));

        Model.Common.ListModel<DeviceModel> result = DeviceListMapper.Map(devices, deviceTypes, locationCount, asset!);

        return result;
    }
}
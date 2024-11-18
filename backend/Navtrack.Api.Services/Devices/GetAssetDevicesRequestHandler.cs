using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using Navtrack.Api.Model.Devices;
using Navtrack.Api.Services.Common.Exceptions;
using Navtrack.Api.Services.Devices.Mappers;
using Navtrack.Api.Services.Requests;
using Navtrack.DataAccess.Model.Assets;
using Navtrack.DataAccess.Model.Devices;
using Navtrack.DataAccess.Services.Assets;
using Navtrack.DataAccess.Services.Devices;
using Navtrack.Shared.Library.DI;
using DeviceType = Navtrack.DataAccess.Model.Devices.DeviceType;

namespace Navtrack.Api.Services.Devices;

[Service(typeof(IRequestHandler<GetAssetDevicesRequest, Model.Common.List<Device>>))]
public class GetAssetDevicesRequestHandler(
    IAssetRepository assetRepository,
    IDeviceRepository deviceRepository,
    IDeviceTypeRepository deviceTypeRepository,
    IDeviceMessageRepository deviceMessageRepository)
    : BaseRequestHandler<GetAssetDevicesRequest, Model.Common.List<Device>>
{
    private AssetDocument? asset;

    public override async Task Validate(RequestValidationContext<GetAssetDevicesRequest> context)
    {
        asset = await assetRepository.GetById(context.Request.AssetId);
        asset.Return404IfNull();
    }
    
    public override async Task<Model.Common.List<Device>> Handle(GetAssetDevicesRequest request)
    {
        List<DeviceDocument> devices = await deviceRepository.GetDevicesByAssetId(request.AssetId);
        List<DeviceType> deviceTypes = deviceTypeRepository
            .GetDeviceTypes()
            .Where(x => devices.Any(y => y.DeviceTypeId == x.Id))
            .ToList();

        Dictionary<ObjectId, int> locationCount = await deviceMessageRepository
            .GetMessagesCountByDeviceIds(devices.Select(x => x.Id));

        Model.Common.List<Device> result = DeviceListMapper.Map(devices, deviceTypes, locationCount, asset!);

        return result;
    }
}
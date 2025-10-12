using System.Collections.Generic;
using System.Threading.Tasks;
using Navtrack.Api.Model.Devices;
using Navtrack.Api.Services.Devices.Mappers;
using Navtrack.Api.Services.Requests;
using Navtrack.Database.Model.Devices;
using Navtrack.Database.Services.Devices;
using Navtrack.Shared.Library.DI;

namespace Navtrack.Api.Services.Devices;

[Service(typeof(IRequestHandler<GetDeviceTypesRequest, Model.Common.ListModel<DeviceTypeModel>>))]
public class GetDeviceTypesRequestHandler(IDeviceTypeRepository deviceTypeRepository)
    : BaseRequestHandler<GetDeviceTypesRequest, Model.Common.ListModel<DeviceTypeModel>>
{
    public override Task<Model.Common.ListModel<DeviceTypeModel>> Handle(GetDeviceTypesRequest request)
    {
        IEnumerable<DeviceType> deviceTypes = deviceTypeRepository.GetDeviceTypes();

        Model.Common.ListModel<DeviceTypeModel> result = DeviceTypeListMapper.Map(deviceTypes);

        return Task.FromResult(result);
    }
}
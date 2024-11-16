using System.Collections.Generic;
using System.Threading.Tasks;
using Navtrack.Api.Model.Devices;
using Navtrack.Api.Services.Devices.Mappers;
using Navtrack.Api.Services.Requests;
using Navtrack.DataAccess.Services.Devices;
using Navtrack.Shared.Library.DI;

namespace Navtrack.Api.Services.Devices;

[Service(typeof(IRequestHandler<GetDeviceTypesRequest, Model.Common.List<DeviceType>>))]
public class GetDeviceTypesRequestHandler(IDeviceTypeRepository deviceTypeRepository)
    : BaseRequestHandler<GetDeviceTypesRequest, Model.Common.List<DeviceType>>
{
    public override Task<Model.Common.List<DeviceType>> Handle(GetDeviceTypesRequest request)
    {
        IEnumerable<DataAccess.Model.Devices.DeviceType> deviceTypes = deviceTypeRepository.GetDeviceTypes();

        Model.Common.List<DeviceType> result = DeviceTypeListMapper.Map(deviceTypes);

        return Task.FromResult(result);
    }
}
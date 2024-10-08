using Navtrack.Api.Model.Devices;
using Navtrack.Api.Services.Protocols.Mappers;
using Navtrack.DataAccess.Model.Devices;

namespace Navtrack.Api.Services.Devices.Mappers;

public static class DeviceTypeModelMapper
{
    public static DeviceTypeModel Map(DeviceType source)
    {
        return new DeviceTypeModel
        {
            Id = source.Id,
            Manufacturer = source.Manufacturer,
            Model = source.Type,
            Protocol = ProtocolModelMapper.Map(source.Protocol)
        };
    }
}
using Navtrack.Api.Services.Protocols.Mappers;
using DeviceType = Navtrack.Api.Model.Devices.DeviceType;

namespace Navtrack.Api.Services.Devices.Mappers;

public static class DeviceTypeMapper
{
    public static DeviceType Map(DataAccess.Model.Devices.DeviceType source)
    {
        return new DeviceType
        {
            Id = source.Id,
            Manufacturer = source.Manufacturer,
            Model = source.Type,
            Protocol = ProtocolMapper.Map(source.Protocol)
        };
    }
}
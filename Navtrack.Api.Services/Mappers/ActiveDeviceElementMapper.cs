using Navtrack.DataAccess.Model.Assets;
using Navtrack.DataAccess.Model.Devices;

namespace Navtrack.Api.Services.Mappers;

public static class ActiveDeviceElementMapper
{
    public static AssetDeviceElement Map(DeviceDocument deviceDocument)
    {
        return new AssetDeviceElement
        {
            Id = deviceDocument.Id,
            ProtocolPort = deviceDocument.ProtocolPort,
            DeviceTypeId = deviceDocument.DeviceTypeId,
            SerialNumber = deviceDocument.SerialNumber
        };
    }
}
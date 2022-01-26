using Navtrack.DataAccess.Model.Assets;
using Navtrack.DataAccess.Model.Devices;

namespace Navtrack.Api.Services.Mappers;

public static class ActiveDeviceElementMapper
{
    public static AssetDeviceElement Map(DeviceDocument deviceDocument, DeviceType deviceType)
    {
        return new AssetDeviceElement
        {
            Id = deviceDocument.Id,
            ProtocolPort = deviceType.Protocol.Port,
            DeviceTypeId = deviceDocument.DeviceTypeId,
            SerialNumber = deviceDocument.SerialNumber
        };
    }
}
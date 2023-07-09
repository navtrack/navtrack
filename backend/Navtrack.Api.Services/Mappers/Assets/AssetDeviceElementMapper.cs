using Navtrack.DataAccess.Model.Assets;
using Navtrack.DataAccess.Model.Devices;

namespace Navtrack.Api.Services.Mappers.Assets;

public static class AssetDeviceElementMapper
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
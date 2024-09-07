using Navtrack.DataAccess.Model.Devices.Messages;
using Navtrack.Listener.Models;

namespace Navtrack.Listener.Services.Mappers;

public static class PositionMetadataElementMapper
{
    public static MessageMetadataElement Map(Device source)
    {
        MessageMetadataElement destination = new()
        {
            AssetId = source.AssetId!.Value,
            DeviceId = source.DeviceId!.Value
        };

        return destination;
    }
}
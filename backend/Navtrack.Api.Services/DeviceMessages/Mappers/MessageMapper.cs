using Navtrack.Api.Model.Messages;
using Navtrack.Database.Model.Devices;

namespace Navtrack.Api.Services.DeviceMessages.Mappers;

public static class MessageMapper
{
    public static DeviceMessageModel Map(DeviceMessageEntity source)
    {
        DeviceMessageModel result = new()
        {
            Id = source.Id.ToString(),
            Priority = source.MessagePriority ?? MessagePriority.Low,
            CreatedDate = source.CreatedDate,
            Position = MessagePositionMapper.Map(source),
            Device = MessageDeviceMapper.Map(source),
            Vehicle = MessageVehicleMapper.Map(source),
            Gsm = MessageGsmMapper.Map(source),
        };

        return result;
    }
}
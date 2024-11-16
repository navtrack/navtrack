using Navtrack.Api.Model.Messages;
using Navtrack.DataAccess.Model.Devices.Messages;

namespace Navtrack.Api.Services.DeviceMessages.Mappers;

public static class MessageMapper
{
    public static Message Map(DeviceMessageDocument source)
    {
        Message result = new()
        {
            Id = source.Id.ToString(),
            Priority = source.MessagePriority ?? MessagePriority.Low,
            CreatedDate = source.CreatedDate,
            Position = MessagePositionMapper.Map(source.Position),
            Device = MessageDeviceMapper.Map(source.Device),
            Vehicle = MessageVehicleMapper.Map(source.Vehicle),
            Gsm = MessageGsmMapper.Map(source.Gsm),
        };

        return result;
    }
}
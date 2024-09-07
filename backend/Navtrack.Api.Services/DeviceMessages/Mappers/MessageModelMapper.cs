using Navtrack.Api.Model.Messages;
using Navtrack.DataAccess.Model.Devices.Messages;

namespace Navtrack.Api.Services.DeviceMessages.Mappers;

public static class MessageModelMapper
{
    public static MessageModel Map(DeviceMessageDocument source)
    {
        MessageModel message = new()
        {
            Id = source.Id.ToString(),
            Priority = source.MessagePriority ?? MessagePriority.Low,
            CreatedDate = source.CreatedDate,
            Position = PositionModelMapper.Map(source.Position),
            Device = MessageDeviceModelMapper.Map(source.Device),
            Vehicle = VehicleModelMapper.Map(source.Vehicle),
            Gsm = GsmModelMapper.Map(source.Gsm),
        };

        return message;
    }
}
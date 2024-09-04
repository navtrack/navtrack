using Navtrack.Api.Model.Messages;
using Navtrack.Api.Services.Mappers.Positions;
using Navtrack.DataAccess.Model.Devices.Messages;
using Navtrack.Shared.Library.Extensions;

namespace Navtrack.Api.Services.Mappers.Messages;

public static class MessageModelMapper
{
    public static MessageModel Map(DeviceMessageDocument source)
    {
        MessageModel position = new()
        {
            Id = source.Id.ToString(),
            Gsm = new GsmModel
            {
                // GsmSignal = source.Data.Test()
            }
        };

        return position;
    }
}
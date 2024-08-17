using System.Collections.Generic;
using Navtrack.DataAccess.Model.Devices.Messages;

namespace Navtrack.Listener.Server;

public interface ICustomMessageHandler
{
    DeviceMessageDocument Parse(MessageInput input);
    IEnumerable<DeviceMessageDocument>? ParseRange(MessageInput input);
}

public interface ICustomMessageHandler<T> : ICustomMessageHandler;
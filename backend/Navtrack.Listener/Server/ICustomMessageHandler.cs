using System.Collections.Generic;
using Navtrack.Database.Model.Devices;

namespace Navtrack.Listener.Server;

public interface ICustomMessageHandler
{
    DeviceMessageEntity? Parse(MessageInput input);
    IEnumerable<DeviceMessageEntity>? ParseRange(MessageInput input);
}

public interface ICustomMessageHandler<T> : ICustomMessageHandler;
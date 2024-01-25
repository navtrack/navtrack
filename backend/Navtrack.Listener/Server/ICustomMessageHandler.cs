using System.Collections.Generic;
using Navtrack.Listener.Models;

namespace Navtrack.Listener.Server;

public interface ICustomMessageHandler
{
    Position Parse(MessageInput input);
    IEnumerable<Position>? ParseRange(MessageInput input);
}

public interface ICustomMessageHandler<T> : ICustomMessageHandler
{
}
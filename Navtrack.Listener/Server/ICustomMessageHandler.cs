using System.Collections.Generic;
using Navtrack.Listener.Models;

namespace Navtrack.Listener.Server;

public interface ICustomMessageHandler
{
    Location Parse(MessageInput input);
    IEnumerable<Location> ParseRange(MessageInput input);
}
    
public interface ICustomMessageHandler<T> : ICustomMessageHandler
{
}
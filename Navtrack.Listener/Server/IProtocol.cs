using System.Collections.Generic;

namespace Navtrack.Listener.Server
{
    public interface IProtocol
    {
        int Port { get; }
        byte[] MessageStart { get; }
        IEnumerable<byte[]> MessageEnd { get; }
        int? GetMessageLength(byte[] bytes);
    }
}
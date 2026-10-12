using System.Collections.Generic;

namespace Navtrack.Listener.Server;

public interface IProtocol
{
    short Port { get; }
    byte[] MessageStart { get; }
    IEnumerable<byte[]> MessageEnd { get; }
    string SplitMessageBy { get; }
    bool HasMessageLength { get; }
    int? GetMessageLength(byte[] buffer, int bytesReadCount);
}

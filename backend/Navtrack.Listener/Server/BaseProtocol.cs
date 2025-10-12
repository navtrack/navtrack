using System;
using System.Collections.Generic;
using static System.String;

namespace Navtrack.Listener.Server;

public abstract class BaseProtocol : IProtocol
{
    public virtual short Port => throw new Exception("A protocol implementation must have a protocol set.");
    public virtual byte[] MessageStart => [];
    public virtual IEnumerable<byte[]> MessageEnd => new List<byte[]> { Array.Empty<byte>() };
    public virtual string SplitMessageBy => Empty;

    public virtual int? GetMessageLength(byte[] buffer, int bytesReadCount)
    {
        return null;
    }
}
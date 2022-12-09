using System;
using System.Collections.Generic;
using static System.String;

namespace Navtrack.Listener.Server;

public abstract class BaseProtocol : IProtocol
{
    public virtual int Port => throw new Exception("A protocol implementation must have a protocol set.");
    public virtual byte[] MessageStart => new byte[0];
    public virtual IEnumerable<byte[]> MessageEnd => new List<byte[]> {new byte[0]};
    public virtual string SplitMessageBy => Empty;

    public virtual int? GetMessageLength(byte[] bytes)
    {
        return null;
    }
}
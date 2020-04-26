using System;

namespace Navtrack.Listener.Server
{
    public abstract class BaseProtocol : IProtocol
    {
        public virtual int Port => throw new Exception("A protocol implementation must have a protocol set.");
        public virtual byte[] MessageStart => new byte[0];
        public virtual byte[] MessageEnd => new byte[0];

        public virtual int? GetMessageLength(byte[] bytes)
        {
            return null;
        }
    }
}
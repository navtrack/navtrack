using System;

namespace Navtrack.Listener.Server
{
    public abstract class BaseProtocol : IProtocol
    {
        public virtual int Port => throw new Exception("A protocol implementation must have a protocol set.");
        public virtual int[] AdditionalPorts => new int[0]; // TODO temporary, will be removed
        public virtual byte[] MessageStart => new byte[0];
        public virtual byte[] MessageEnd => DetectNewLine ? new byte[] {0x0A} : new byte[0];
        public virtual bool DetectNewLine => false;

        public int? GetLength()
        {
            return null;
        }
    }
}
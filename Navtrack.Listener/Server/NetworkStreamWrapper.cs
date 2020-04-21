using System.IO;
using System.Threading.Tasks;

namespace Navtrack.Listener.Server
{
    public class NetworkStreamWrapper : INetworkStreamWrapper
    {
        private readonly Stream baseStream;

        public NetworkStreamWrapper(Stream baseStream)
        {
            this.baseStream = baseStream;
        }

        public ValueTask DisposeAsync()
        {
            return baseStream.DisposeAsync();
        }

        public void Close()
        {
            baseStream.Close();
        }

        public bool CanRead => baseStream.CanRead;
        
        public int Read(byte[] buffer, int offset, int size)
        {
            return baseStream.Read(buffer, offset, size);
        }

        public void WriteByte(byte value)
        {
            baseStream.WriteByte(value);
        }

        public void Write(byte[] buffer)
        {
            baseStream.Write(buffer);
        }
    }
}
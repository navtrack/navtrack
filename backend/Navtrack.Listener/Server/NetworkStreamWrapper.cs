using System.IO;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace Navtrack.Listener.Server;

public class NetworkStreamWrapper(Stream stream) : INetworkStreamWrapper
{
    private readonly NetworkStream networkStream = stream as NetworkStream;

    public ValueTask DisposeAsync()
    {
        return stream.DisposeAsync();
    }

    public void Close()
    {
        stream.Close();
    }

    public bool CanRead => stream.CanRead;
    public bool DataAvailable => networkStream != null && networkStream.DataAvailable;

    public int Read(byte[] buffer, int offset, int size)
    {
        try
        {
            return stream.Read(buffer, offset, size);
        }
        catch (IOException)
        {
            return 0;
        }
    }

    public void WriteByte(byte value)
    {
        stream.WriteByte(value);
    }

    public void Write(byte[] buffer)
    {
        stream.Write(buffer);
    }
}
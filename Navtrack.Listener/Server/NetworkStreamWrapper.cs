using System.IO;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace Navtrack.Listener.Server;

public class NetworkStreamWrapper : INetworkStreamWrapper
{
    private readonly Stream baseStream;
    private readonly NetworkStream networkStream;

    public NetworkStreamWrapper(Stream baseStream)
    {
        this.baseStream = baseStream;
        networkStream = baseStream as NetworkStream;
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
    public bool DataAvailable => networkStream != null && networkStream.DataAvailable;

    public int Read(byte[] buffer, int offset, int size)
    {
        try
        {
            return baseStream.Read(buffer, offset, size);
        }
        catch (IOException)
        {
            return 0;
        }
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
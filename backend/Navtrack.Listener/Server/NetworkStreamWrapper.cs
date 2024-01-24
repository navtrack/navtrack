using System.IO;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace Navtrack.Listener.Server;

public class NetworkStreamWrapper(TcpClient tcpClient) : INetworkStreamWrapper
{
    private readonly NetworkStream networkStream = tcpClient.GetStream();
    public TcpClient TcpClient { get; } = tcpClient;
    public string? RemoteEndPoint => TcpClient.Client.RemoteEndPoint?.ToString();

    public ValueTask DisposeAsync()
    {
        TcpClient.Dispose();
        return networkStream.DisposeAsync();
    }

    public void Close()
    {
        networkStream.Close();
    }

    public bool CanRead => networkStream.CanRead;
    public bool DataAvailable => networkStream.DataAvailable;

    public int Read(byte[] buffer, int offset, int size)
    {
        try
        {
            return networkStream.Read(buffer, offset, size);
        }
        catch (IOException)
        {
            return 0;
        }
    }

    public void WriteByte(byte value)
    {
        networkStream.WriteByte(value);
    }

    public void Write(byte[] buffer)
    {
        networkStream.Write(buffer);
    }

}
using System;
using System.Net.Sockets;

namespace Navtrack.Listener.Server;

public class TcpClientAdapter : IDisposable
{
    private readonly TcpClient tcpClient;

    public TcpClientAdapter()
    {
    }
    
    public TcpClientAdapter(TcpClient tcpClient)
    {
        this.tcpClient = tcpClient;
    }
    
    public virtual NetworkStream GetStream()
    {
        return tcpClient.GetStream();
    }

    public void Dispose()
    {
        tcpClient.Dispose();
    }

    public virtual string? GetRemoteEndPoint()
    {
        return tcpClient.Client.RemoteEndPoint?.ToString();
    }

    public void Close()
    {
        tcpClient.Close();
    }
}
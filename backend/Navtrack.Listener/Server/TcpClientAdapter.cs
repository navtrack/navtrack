using System;
using System.Net.Sockets;

namespace Navtrack.Listener.Server;

public class TcpClientAdapter : IDisposable
{
    private readonly TcpClient? tcpClient;
    private readonly string? remoteEndPoint;

    public TcpClientAdapter()
    {
    }
    
    public TcpClientAdapter(TcpClient tcpClient)
    {
        this.tcpClient = tcpClient;
        remoteEndPoint = tcpClient.Client.RemoteEndPoint?.ToString();
    }
    
    public virtual NetworkStream GetStream()
    {
        return tcpClient?.GetStream() ?? throw new InvalidOperationException("The TCP client is not initialized.");
    }

    public void Dispose()
    {
        tcpClient?.Dispose();
    }

    public virtual string? GetRemoteEndPoint()
    {
        return remoteEndPoint;
    }

    public void Close()
    {
        tcpClient?.Close();
    }
}

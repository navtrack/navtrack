using System;
using System.Threading;
using System.Threading.Tasks;

namespace Navtrack.Listener.Server;

public interface INetworkStreamWrapper : IAsyncDisposable
{
    void Close();
    bool CanRead { get; }
    bool DataAvailable { get; }
    int Read(byte[] buffer, int offset, int size);
    ValueTask<int> ReadAsync(Memory<byte> buffer, CancellationToken cancellationToken);
    void WriteByte(byte value);
    void Write(byte[] buffer);
    TcpClientAdapter TcpClient { get; }
    string? RemoteEndPoint { get; }
}

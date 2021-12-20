using System;

namespace Navtrack.Listener.Server;

public interface INetworkStreamWrapper : IAsyncDisposable
{
    void Close();
    bool CanRead { get; }
    bool DataAvailable { get; }
    int Read(byte[] buffer, int offset, int size);
    void WriteByte(byte value);
    void Write(byte[] buffer);
}
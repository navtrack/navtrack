using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Bson;
using Navtrack.Listener.Server;

namespace Navtrack.Listener.Models;

public class ProtocolConnectionContext(INetworkStreamWrapper networkStream, IProtocol protocol, ObjectId connectionId) : IAsyncDisposable
{
    public IProtocol Protocol { get; } = protocol;
    public ObjectId ConnectionId { get; } = connectionId;
    public Device? Device { get; private set; }
    public INetworkStreamWrapper NetworkStream { get; } = networkStream;
    public DateTime? MaxDate { get; set; }

    public void SetDevice(string serialNumber)
    {
        if (!string.IsNullOrEmpty(serialNumber) && Device == null)
        {
            Device = new Device(serialNumber);
        }
    }

    public T? GetClientCache<T>(string key)
    {
        return ClientCache.TryGetValue(key, out object? value) && value != null ? (T)value : default;
    }

    public void SetClientCache<T>(string key, T? value)
    {
        ClientCache[key] = value;
    }

    private Dictionary<string, object?> ClientCache { get; } = new();

    public async ValueTask DisposeAsync()
    {
        await NetworkStream.DisposeAsync();
    }
}
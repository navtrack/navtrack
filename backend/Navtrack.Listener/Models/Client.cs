using System.Collections.Generic;
using System.Net.Sockets;
using Navtrack.DataAccess.Model.Devices;
using Navtrack.Listener.Server;

namespace Navtrack.Listener.Models;

public class Client
{
    public Client()
    {
        ClientCache = new Dictionary<string, object>();
    }

    public TcpClient TcpClient { get; set; }
    public IProtocol Protocol { get; set; }
    public Device Device { get; private set; }
    public DeviceConnectionDocument DeviceConnection { get; set; }

    public void SetDevice(Device device)
    {
        Device = device;
    }

    public void SetDevice(string imei)
    {
        if (!string.IsNullOrEmpty(imei) && Device == null)
        {
            Device = new Device
            {
                IMEI = imei
            };
        }
    }

    public T GetClientCache<T>(string key)
    {
        if (ClientCache.TryGetValue(key, out object value))
        {
            return (T)value;
        }

        return default;
    }

    public void SetClientCache<T>(string key, T value)
    {
        ClientCache[key] = value;
    }

    private Dictionary<string, object> ClientCache { get; set; }
}
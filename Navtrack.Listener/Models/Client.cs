using System.Net.Sockets;
using Navtrack.DataAccess.Model.Devices;
using Navtrack.Listener.Server;

namespace Navtrack.Listener.Models;

public class Client
{
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
}
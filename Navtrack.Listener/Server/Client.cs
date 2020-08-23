using System.Net.Sockets;
using Navtrack.DataAccess.Model;
using Navtrack.Listener.Models;

namespace Navtrack.Listener.Server
{
    public class Client
    {
        public TcpClient TcpClient { get; set; }
        public IProtocol Protocol { get; set; }
        public Device Device { get; private set; }
        public DeviceConnectionEntity DeviceConnection { get; set; }

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
}
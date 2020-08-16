using System.Net.Sockets;
using Navtrack.DataAccess.Model;
using Navtrack.Listener.Models;

namespace Navtrack.Listener.Server
{
    public class Client
    {
        public TcpClient TcpClient { get; set; }
        public IProtocol Protocol { get; set; }
        public Device Device { get; set; }
        public DeviceConnectionEntity DeviceConnection { get; set; }
    }
}
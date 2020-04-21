using System.Collections.Generic;
using Navtrack.Listener.Models;
using Navtrack.Listener.Server;

namespace Navtrack.Listener.Tests.Protocols
{
    public interface IProtocolTester
    {
        void SendHexFromDevice(string value);
        void SendBytesFromDevice(byte[] value);
        void SendStringFromDevice(string value);
        string ReceiveInDevice();
        Client Client { get; }
        IEnumerable<Location> ParsedLocations { get; }
        Location ParsedLocation { get; }
    }
}
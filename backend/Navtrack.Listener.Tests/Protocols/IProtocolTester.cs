using System.Collections.Generic;
using Navtrack.Listener.Models;

namespace Navtrack.Listener.Tests.Protocols;

public interface IProtocolTester
{
    void SendHexFromDevice(string value);
    void SendBytesFromDevice(byte[] value);
    void SendStringFromDevice(string value);
    string ReceiveHexInDevice();
    string ReceiveStringInDevice();
    ProtocolConnectionContext ConnectionContext { get; }
    List<Location> TotalParsedLocations { get; }
    List<Location?>? LastParsedLocations { get; }
    Location? LastParsedLocation { get; }
}
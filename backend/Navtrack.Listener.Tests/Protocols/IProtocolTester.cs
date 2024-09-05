using System.Collections.Generic;
using Navtrack.DataAccess.Model.Devices.Messages;
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
    List<DeviceMessageDocument> TotalParsedMessages { get; }
    List<DeviceMessageDocument?>? LastParsedMessages { get; }
    DeviceMessageDocument? LastParsedMessage { get; }
}
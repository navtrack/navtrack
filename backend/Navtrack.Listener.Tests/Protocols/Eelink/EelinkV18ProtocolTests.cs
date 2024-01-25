using Navtrack.Listener.Protocols.Eelink;
using Xunit;

namespace Navtrack.Listener.Tests.Protocols.Eelink;

public class EelinkV18ProtocolTests : BaseProtocolTests<EelinkProtocol, EelinkMessageHandler>
{
    [Fact]
    public void DeviceSendsLoginPackage_ServerAcknowledges()
    {
        ProtocolTester.SendHexFromDevice("676701000C000101234567890123450120");

        Assert.Equal("67670100020001", ProtocolTester.ReceiveHexInDevice());
    }

    [Fact]
    public void DeviceSendsHeartbeatPackage_ServerAcknowledges()
    {
        ProtocolTester.SendHexFromDevice("6767030004001A0001");

        Assert.Equal("6767030002001A", ProtocolTester.ReceiveHexInDevice());
    }

    [Fact]
    public void DeviceSendsLocationPackage_LocationIsParsed()
    {
        // Login
        ProtocolTester.SendHexFromDevice("676701000C000101234567890123450120");

        // Location
        ProtocolTester.SendHexFromDevice("676702001C02B259AE7387FCD360D6034332B2000000028F000A4F64002EB10101");

        Assert.NotNull(ProtocolTester.LastParsedPosition);
    }

    [Fact]
    public void DeviceSendsWarningPackage_ServerAcknowledges()
    {
        // Login
        ProtocolTester.SendHexFromDevice("676701000C000101234567890123450120");

        // Warning
        ProtocolTester.SendHexFromDevice("676704001C00B7569FC3020517A2D7020B08E100000000D8001E005B0004460004");

        Assert.Equal("676704000200B7", ProtocolTester.ReceiveHexInDevice());
        Assert.NotNull(ProtocolTester.LastParsedPosition);
    }

    [Fact]
    public void DeviceSendsReportPackage_ServerAcknowledges()
    {
        // Login
        ProtocolTester.SendHexFromDevice("676701000C000101234567890123450120");

        // Report
        ProtocolTester.SendHexFromDevice(
            "676705002102B459AE7388FCD360D7034332B1000000028F000A4F64002EB101010159AE7388");

        Assert.Equal("676705000202B4", ProtocolTester.ReceiveHexInDevice());
    }
}
using Navtrack.Listener.Protocols.Smartrack;
using Xunit;

namespace Navtrack.Listener.Tests.Protocols.Smartrack;

public class SmartrackProtocolTests : BaseProtocolTests<SmartrackProtocol, SmartrackMessageHandler>
{
    [Fact]
    public void DeviceSendsLoginV1_ServerRespondsWithLoginConfirmation()
    {
        ProtocolTester.SendStringFromDevice(
            "(080524101241BP05000013632782450080524A2232.9806N11404.9355E000.1101241323.8700000000L000450AC)");

        Assert.Equal("(080524101241AP05)", ProtocolTester.ReceiveStringInDevice());
    }

    [Fact]
    public void DeviceSendsLoginV2_ServerRespondsWithLoginConfirmation()
    {
        ProtocolTester.SendStringFromDevice("(040331141830BP00000013612345678HSO)");

        Assert.Equal("(040331141830AP01HSO)", ProtocolTester.ReceiveStringInDevice());
    }

    [Fact]
    public void DeviceSendsLocation_LocationIsParsed()
    {
        // Login
        ProtocolTester.SendStringFromDevice("(040331141830BP00000013612345678HSO)");

        // Location
        ProtocolTester.SendStringFromDevice(
            "(080612022828BR00080612A2232.9828N11404.9297E000.0022828000.0000000000L000230AA)");

        Assert.NotNull(ProtocolTester.LastParsedPosition);
    }
}
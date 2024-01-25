using Navtrack.Listener.Protocols.Carscop;
using Xunit;

namespace Navtrack.Listener.Tests.Protocols.Carscop;

// ReSharper disable once InconsistentNaming
public class CarscopCC800ProtocolTests : BaseProtocolTests<CarscopProtocol, CarscopMessageHandler>
{
    [Fact]
    public void DeviceSendsLocationLoginMessage_LocationIsParsed()
    {
        ProtocolTester.SendStringFromDevice(
            "*040331141830UB05CW0800C12345678013255A2240.8419N11408.8178E000.104033129.2011111111L000023^");

        Assert.Equal("*040331141830DX061^", ProtocolTester.ReceiveStringInDevice());
    }

    [Fact]
    public void DeviceSendsLocationV1_LocationIsParsed()
    {
        // Login message
        ProtocolTester.SendStringFromDevice(
            "*040331141830UB05CW0800C12345678013255A2240.8419N11408.8178E000.104033129.2011111111L000023^");

        // Location message
        ProtocolTester.SendStringFromDevice(
            "*040331141830UD04013255A2267.6805N11415.1885E000.104033129.2011111111L000023^");

        Assert.NotNull(ProtocolTester.LastParsedPosition);
    }
}
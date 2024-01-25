using Navtrack.Listener.Protocols.KeSon;
using Xunit;

namespace Navtrack.Listener.Tests.Protocols.KeSon;

public class KeSonProtocolTests : BaseProtocolTests<KeSonProtocol, KeSonMessageHandler>
{
    [Fact]
    public void DeviceSendsLocationV1_LocationIsParsed()
    {
        ProtocolTester.SendStringFromDevice("#356823032089950##0#0000#AUT#1#24900FFB#11351.4634,E,2234.5076,N,001.66,347#290312#072851##");

        Assert.NotNull(ProtocolTester.LastParsedPosition);
    }
}
using Navtrack.Listener.Protocols.Freedom;
using Xunit;

namespace Navtrack.Listener.Tests.Protocols.Freedom;

public class GPSMarkerProtocolTests : BaseProtocolTests<FreedomProtocol, FreedomMessageHandler>
{
    [Fact]
    public void DeviceSendsLocationV1_LocationIsParsed()
    {
        ProtocolTester.SendStringFromDevice(
            "IMEI,353358011714362,2014/05/22, 20:49:32, N, Lat:4725.9624, E, Lon:01912.5483, Spd:5.05");

        Assert.NotNull(ProtocolTester.LastParsedPosition);
    }
}
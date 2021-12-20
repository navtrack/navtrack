using Navtrack.Listener.Protocols.GoPass;
using NUnit.Framework;

namespace Navtrack.Listener.Tests.Protocols.GoPass;

public class GlobalSatProtocolTests : BaseProtocolTests<GoPassProtocol, GoPassMessageHandler>
{
    [Test]
    public void DeviceSendsLocationV1_LocationIsParsed()
    {
        ProtocolTester.SendStringFromDevice(
            "123456789012345\r\n");
            
        ProtocolTester.SendStringFromDevice(
            "$GPRMC,204700,A,3403.868,N,11709.432,W,001.9,336.9,170698,013.6,E*6E");

        Assert.IsNotNull(ProtocolTester.LastParsedLocation);
    }
}
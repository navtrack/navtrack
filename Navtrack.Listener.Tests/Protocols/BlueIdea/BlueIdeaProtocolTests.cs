using Navtrack.Listener.Protocols.BlueIdea;
using NUnit.Framework;

namespace Navtrack.Listener.Tests.Protocols.BlueIdea;

public class BlueIdeaProtocolTests : BaseProtocolTests<BlueIdeaProtocol, BlueIdeaMessageHandler>
{
    [Test]
    public void DeviceSendsLocationV1_LocationIsParsed()
    {
        ProtocolTester.SendStringFromDevice(
            "$GPRMC,862106020628733,050859.000,A,1404.8573,N,08710.9967,W,0.00,0,080117,0,,00C8,00218,99,,,,,,0.00");

        Assert.IsNotNull(ProtocolTester.LastParsedLocation);
    }

    [Test]
    public void DeviceSendsLocationV2_LocationIsParsed()
    {
        ProtocolTester.SendStringFromDevice(
            "$GPRMC,10000000001,092313.299,A,2238.8947,N,11355.2253,E,0.00,311.19,010307,0,,");

        Assert.IsNotNull(ProtocolTester.LastParsedLocation);
    }

    [Test]
    public void DeviceSendsLocationV3_LocationIsParsed()
    {
        ProtocolTester.SendStringFromDevice(
            "$GPRMC,00000000000,092244.000,A,0000.0000,S,00000.0000,E,0.00,0.00,101016,0,,8000,0");

        Assert.IsNotNull(ProtocolTester.LastParsedLocation);
    }
}
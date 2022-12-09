using Navtrack.Listener.Protocols.Skypatrol;
using NUnit.Framework;

namespace Navtrack.Listener.Tests.Protocols.SkypatrolTests;

public class SkypatrolProtocolTests : BaseProtocolTests<SkypatrolProtocol, SkypatrolMessageHandler>
{
    [Test]
    public void DeviceSendsLocationV1_LocationIsParsed()
    {
        ProtocolTester.SendStringFromDevice(
            "*GS06,356496042329318,031417090613,,SYS:G6S;V1.00;V1.0.1,GPS:A;7;N23.164358;E113.428515;0;0;44;1.10#");

        Assert.IsNotNull(ProtocolTester.LastParsedLocation);
    }

    [Test]
    public void DeviceSendsLocationV2_LocationIsParsed()
    {
        ProtocolTester.SendStringFromDevice(
            "*GS06,356496042329318,031427090613,,SYS:G6S;V1.00;V1.0.1,GPS:A;7;N23.164358;E113.428515;0;0;45;1.10#");

        Assert.IsNotNull(ProtocolTester.LastParsedLocation);
    }
}
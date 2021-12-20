using Navtrack.Listener.Protocols.TkStar;
using NUnit.Framework;

namespace Navtrack.Listener.Tests.Protocols.TkStar;

public class TkStarProtocolTests : BaseProtocolTests<TkStarProtocol, TkStarMessageHandler>
{
    [Test]
    public void DeviceSendsLocationV1_LocationIsParsed()
    {
        ProtocolTester.SendStringFromDevice("*HQ,9171133403,V1,091019,V,2234.9299,N,11354.3998,E,000.00,000,010520,FBFFBBFF,226,10,8600,19869#");

        Assert.IsNotNull(ProtocolTester.LastParsedLocation);
    }
}
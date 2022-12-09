using Navtrack.Listener.Protocols.LKGPS;
using NUnit.Framework;

namespace Navtrack.Listener.Tests.Protocols.LKGPS;

public class LKGPSProtocolTests : BaseProtocolTests<LKGPSProtocol, LKGPSMessageHandler>
{
    [Test]
    public void DeviceSendsLocationV1_LocationIsParsed()
    {
        ProtocolTester.SendStringFromDevice("*TH,2020916012,V1,050316,A,2212.8745,N,11346.6574,E,14.28,028,220902,FFFFFBFF#");

        Assert.IsNotNull(ProtocolTester.LastParsedLocation);
    }
        
    [Test]
    public void DeviceSendsLocationV2_LocationIsParsed()
    {
        ProtocolTester.SendStringFromDevice("*HQ,4103000861,V1,092853,A,2234.2029,N,11351.4197,E,000.40,000,270215,FFFFFBFF,460,00,0,0,6#");

        Assert.IsNotNull(ProtocolTester.LastParsedLocation);
    }
}
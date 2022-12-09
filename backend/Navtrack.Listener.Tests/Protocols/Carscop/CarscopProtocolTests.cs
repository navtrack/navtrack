using Navtrack.Listener.Protocols.Carscop;
using NUnit.Framework;

namespace Navtrack.Listener.Tests.Protocols.Carscop;

// CC828, CC830, CC630 GPS Communication Protocol
public class CarscopProtocolTests : BaseProtocolTests<CarscopProtocol, CarscopMessageHandler>
{
    [Test]
    public void DeviceSendsLocationV1_LocationIsParsed()
    {
        ProtocolTester.SendStringFromDevice("*HQ,1400046168,V1,055600,A,2234.3066,N,11351.6829,E,000.0,000,080813,FFFFFBFF#");

        Assert.IsNotNull(ProtocolTester.LastParsedLocation);
    }
        
    [Test]
    public void DeviceSendsLocationV2_LocationIsParsed()
    {
        ProtocolTester.SendStringFromDevice("*HQ,1400046168,V1,055600,V,2234.3066,N,11351.6829,E,000.0,000,080813,FFFFFFFE#");

        Assert.IsNotNull(ProtocolTester.LastParsedLocation);
    }
}
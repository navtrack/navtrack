using Navtrack.Listener.Protocols.Arknav;
using NUnit.Framework;

namespace Navtrack.Listener.Tests.Protocols.Arknav;

public class ArknavProtocolTests : BaseProtocolTests<ArknavProtocol, ArknavMessageHandler>
{
    [Test]
    public void DeviceSendsLocationV1_LocationIsParsed()
    {
        ProtocolTester.SendStringFromDevice(
            "358266016278447,05*827,000,L001,V,4821.6584,N,01053.8650,E,000.0,000.0,00.0,08:46:04 17-03-16,9.5A,D7,0,79,0,,,,");

        Assert.IsNotNull(ProtocolTester.LastParsedLocation);
    }

    [Test]
    public void DeviceSendsLocationV2_LocationIsParsed()
    {
        ProtocolTester.SendStringFromDevice(
            "123456789012345,05*850,000,L001,A,2459.3640,N,12125.2958,E,000.0,224.8,00.8,07:47:26 09-09-05,9.00,D3,0,C4,1,,,,");

        Assert.IsNotNull(ProtocolTester.LastParsedLocation);
    }
        
    [Test]
    public void DeviceSendsLocationV3_LocationIsParsed()
    {
        ProtocolTester.SendStringFromDevice(
            "123456789012345,05*850,000,L001,A,2459.3640,N,12125.2958,E,000.0,224.8,00.8,07:47:26 09-09-05,9.00,D3,0,C4,1,,,00000000,");

        Assert.IsNotNull(ProtocolTester.LastParsedLocation);
    }
}
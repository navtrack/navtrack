using Navtrack.Listener.Protocols.Haicom;
using NUnit.Framework;

namespace Navtrack.Listener.Tests.Protocols.Haicom;

public class HaicomProtocolTests : BaseProtocolTests<HaicomProtocol, HaicomMessageHandler>
{
    [Test]
    public void DeviceSendsLocationV1_LocationIsParsed()
    {
        ProtocolTester.SendStringFromDevice(
            "$GPRS012497007097169,T100001,150618,230031,5402267400332464,0004,2014,000001,,,1,00#V040*");

        Assert.IsNotNull(ProtocolTester.LastParsedLocation);
    }

    [Test]
    public void DeviceSendsLocationV2_LocationIsParsed()
    {
        ProtocolTester.SendStringFromDevice(
            "$GPRS123456789012345,602S19A,100915,063515,7240649312041079,0019,3156,111000,10004,0000,11111,00LH#V037");

        Assert.IsNotNull(ProtocolTester.LastParsedLocation);
    }
        
    [Test]
    public void DeviceSendsLocationV3_LocationIsParsed()
    {
        ProtocolTester.SendStringFromDevice(
            "$GPRS123456789012345,T100001,141112,090751,7240649312041079,0002,1530,000001,,,1,00#V039*");

        Assert.IsNotNull(ProtocolTester.LastParsedLocation);
    }
        
    [Test]
    public void DeviceSendsLocationV4_LocationIsParsed()
    {
        ProtocolTester.SendStringFromDevice(
            "$GPRS012497007101250,T100001,141231,152235,7503733600305643,0000,2285,000001,,,1,00#V041*");

        Assert.IsNotNull(ProtocolTester.LastParsedLocation);
    }
}
using Navtrack.Listener.Protocols.Arusnavi;
using NUnit.Framework;

namespace Navtrack.Listener.Tests.Protocols.Arusnavi;

public class ArusnaviTextProtocolTests : BaseProtocolTests<ArusnaviProtocol, ArusnaviMessageHandler>
{
    [Test]
    public void DeviceSendsLocationV1_LocationIsParsed()
    {
        ProtocolTester.SendStringFromDevice(
            "$AV,V4,999999,12487,2277,203,65534,0,0,193,65535,65535,65535,65535,1,13,80.0,56.1,200741,5950.6773N,03029.1043E,300.0,360.0,121012,65535,65535,65535,SF*6E");

        Assert.IsNotNull(ProtocolTester.LastParsedLocation);
    }

    [Test]
    public void DeviceSendsLocationV2_LocationIsParsed()
    {
        ProtocolTester.SendStringFromDevice(
            "$AV,V2,32768,12487,2277,203,-1,0,0,193,0,0,1,13,200741,5950.6773N,03029.1043E,0.0,0.0,121012,*6E");

        Assert.IsNotNull(ProtocolTester.LastParsedLocation);
    }

    [Test]
    public void DeviceSendsLocationV3_LocationIsParsed()
    {
        ProtocolTester.SendStringFromDevice(
            "$AV,V3,999999,12487,2277,203,65534,0,0,193,65535,65535,65535,65535,1,13,200741,5950.6773N,03029.1043E,300.0,360.0,121012,65535,65535,65535,SF*6E");

        Assert.IsNotNull(ProtocolTester.LastParsedLocation);
    }
}
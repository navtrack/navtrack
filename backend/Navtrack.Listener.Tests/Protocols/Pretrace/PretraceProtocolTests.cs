using Navtrack.Listener.Protocols.Pretrace;
using NUnit.Framework;

namespace Navtrack.Listener.Tests.Protocols.Pretrace;

public class PretraceProtocolTests : BaseProtocolTests<PretraceProtocol, PretraceMessageHandler>
{
    [Test]
    public void DeviceSendsLocationV1_LocationIsParsed()
    {
        ProtocolTester.SendStringFromDevice(
            "(867967021915915U1110A1701201500102238.1700N11401.9324E000264000000000009001790000000,&P11A4,F1050^47");

        Assert.IsNotNull(ProtocolTester.LastParsedLocation);
    }

    [Test]
    public void DeviceSendsLocationV2_LocationIsParsed()
    {
        ProtocolTester.SendStringFromDevice(
            "(864244029498838U1110A1509250653072238.1641N11401.9213E000196000000000406002990000000,&P195%,T1050,F14A5,R104C51E47B^30");

        Assert.IsNotNull(ProtocolTester.LastParsedLocation);
    }
}
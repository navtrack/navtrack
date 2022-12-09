using Navtrack.Listener.Protocols.GPSMarker;
using NUnit.Framework;

namespace Navtrack.Listener.Tests.Protocols.GPSMarker;

public class GPSMarkerProtocolTests : BaseProtocolTests<GPSMarkerProtocol, GPSMarkerMessageHandler>
{
    [Test]
    public void DeviceSendsLocationV1_LocationIsParsed()
    {
        ProtocolTester.SendStringFromDevice(
            "$GM23D863071014445404T260816142611N55441051E037325071033063C0530304#");

        Assert.IsNotNull(ProtocolTester.LastParsedLocation);
    }

    [Test]
    public void DeviceSendsLocationV2_LocationIsParsed()
    {
        ProtocolTester.SendStringFromDevice(
            "$GM300350123456789012T100511123300G25000001772F185200000000000000005230298#");

        Assert.IsNull(ProtocolTester.LastParsedLocation);
    }

    [Test]
    public void DeviceSendsLocationV3_LocationIsParsed()
    {
        ProtocolTester.SendStringFromDevice(
            "$GM200350123456789012T100511123300N55516789E03756123400000035230298#");

        Assert.IsNotNull(ProtocolTester.LastParsedLocation);
    }

    [Test]
    public void DeviceSendsLocationV4_LocationIsParsed()
    {
        ProtocolTester.SendStringFromDevice(
            "$GM1350123456789012T1005111233N55516789E03756123400000035200298#");

        Assert.IsNull(ProtocolTester.LastParsedLocation);
    }

    [Test]
    public void DeviceSendsLocationV5_LocationIsParsed()
    {
        ProtocolTester.SendStringFromDevice(
            "$GM203863071014445404T150715202258N55481576E03729275300000040530301#");

        Assert.IsNotNull(ProtocolTester.LastParsedLocation);
    }
}
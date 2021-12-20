using Navtrack.Listener.Protocols.WondeProud;
using NUnit.Framework;

namespace Navtrack.Listener.Tests.Protocols.WondeProud;

public class WondeProudProtocolTests : BaseProtocolTests<WondeProudProtocol, WondeProudMessageHandler>
{
    [Test]
    public void DeviceSendsLocationV1_LocationIsParsed()
    {
        ProtocolTester.SendStringFromDevice(
            "2005010051,19990925063830,26.106181,44.440225,0,0,0,7,2,0.0,0,,,0");

        Assert.IsNotNull(ProtocolTester.LastParsedLocation);
    }

    [Test]
    public void DeviceSendsLocationV2_LocationIsParsed()
    {
        ProtocolTester.SendStringFromDevice(
            "2000000108,20151030145404,76.948633,43.354700,0,140,15,10,1,1325,125.4,10.5,0.0");

        Assert.IsNotNull(ProtocolTester.LastParsedLocation);
    }

    [Test]
    public void DeviceSendsLocationV3_LocationIsParsed()
    {
        ProtocolTester.SendStringFromDevice(
            "2000000257,20151030145351,69.379976,53.283905,0,0,16,2,0,0,469.1,58.9,0.0");

        Assert.IsNotNull(ProtocolTester.LastParsedLocation);
    }

    [Test]
    public void DeviceSendsLocationV4_LocationIsParsed()
    {
        ProtocolTester.SendStringFromDevice(
            "2000000232,20151030145206,51.166900,43.651353,0,132,11,2,0,0,0.0,0.0,0.0");

        Assert.IsNotNull(ProtocolTester.LastParsedLocation);
    }

    [Test]
    public void DeviceSendsLocationV5_LocationIsParsed()
    {
        ProtocolTester.SendStringFromDevice(
            "2000000259,20151030145653,69.380826,53.283890,9,10,15,2,1,695,1002.6,108.2,0.0");

        Assert.IsNotNull(ProtocolTester.LastParsedLocation);
    }

    [Test]
    public void DeviceSendsLocationV6_LocationIsParsed()
    {
        ProtocolTester.SendStringFromDevice(
            "1044989601,20130323074605,0.000000,90.000000,0,000,0,0,2");

        Assert.IsNotNull(ProtocolTester.LastParsedLocation);
    }

    [Test]
    public void DeviceSendsLocationV7_LocationIsParsed()
    {
        ProtocolTester.SendStringFromDevice(
            "210000001,20070313170040,121.123456,12.654321,0,233,0,9,2,0.0,0,0.00,0.00,0");

        Assert.IsNotNull(ProtocolTester.LastParsedLocation);
    }

    [Test]
    public void DeviceSendsLocationV8_LocationIsParsed()
    {
        ProtocolTester.SendStringFromDevice(
            "1044989601,20130322172647,13.572583,52.401070,22,204,49,0,2");

        Assert.IsNotNull(ProtocolTester.LastParsedLocation);
    }

    [Test]
    public void DeviceSendsLocationV9_LocationIsParsed()
    {
        ProtocolTester.SendStringFromDevice(
            "1044989601,20130322172647,13.572583,52.401070,22,204,-49,0,2");

        Assert.IsNotNull(ProtocolTester.LastParsedLocation);
    }

    [Test]
    public void DeviceSendsLocationV10_LocationIsParsed()
    {
        ProtocolTester.SendStringFromDevice(
            "3997324533,20140326074908,28.797603,47.041635,0,48,0,6,2,3.90V,0");

        Assert.IsNotNull(ProtocolTester.LastParsedLocation);
    }

    [Test]
    public void DeviceSendsLocationV11_LocationIsParsed()
    {
        ProtocolTester.SendStringFromDevice(
            "2000000001,20140529213210,-63.179111,9.781493,0,0,54.0,8,2,0.0,0,0.01,0.01,0,0,0,0");

        Assert.IsNotNull(ProtocolTester.LastParsedLocation);
    }
}
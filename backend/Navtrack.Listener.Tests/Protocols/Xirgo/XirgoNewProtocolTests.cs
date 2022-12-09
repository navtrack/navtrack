using Navtrack.Listener.Protocols.Xirgo;
using NUnit.Framework;

namespace Navtrack.Listener.Tests.Protocols.Xirgo;

public class XirgoNewProtocolTests : BaseProtocolTests<XirgoProtocol, XirgoMessageHandler>
{
    [Test]
    public void DeviceSendsLocationV1_LocationIsParsed()
    {
        ProtocolTester.SendStringFromDevice(
            "$$352054058132185,4001,2017/04/21,00:01:05,32.54659,-116.90670,143.2,0,0,0,598,0.0,12,0.9,765840,7.0,14.5,19,1,1,0011,8.5,63.2,5,21999,184,255,671,207,100,185##");

        Assert.IsNotNull(ProtocolTester.LastParsedLocation);
    }

    [Test]
    public void DeviceSendsLocationV2_LocationIsParsed()
    {
        ProtocolTester.SendStringFromDevice(
            "$$352054058132185,6011,2017/04/21,04:57:10,32.49658,-116.85957,250.9,0,0,0,602,0.0,12,0.8,765876,7.0,14.1,21,1,1,0011,10.1,0.0,5,170917890,280,255,627,0,100,167##");

        Assert.IsNotNull(ProtocolTester.LastParsedLocation);
    }

    [Test]
    public void DeviceSendsLocationV3_LocationIsParsed()
    {
        ProtocolTester.SendStringFromDevice(
            "$$355922061611345,6001,2016/08/25,20:10:51,51.13042,-114.22752,1197,44.7,0.0,0.0,2622,27,12,0.8,1,0.0,13.9,24,1,0,0.0,-70,-809,688##");

        Assert.IsNotNull(ProtocolTester.LastParsedLocation);
    }

    [Test]
    public void DeviceSendsLocationV4_LocationIsParsed()
    {
        ProtocolTester.SendStringFromDevice(
            "$$355922061611345,6001,2016/08/25,20:10:38,51.12948,-114.22637,1203,34.8,0.0,0.0,1377,215,12,0.8,1,0.0,13.8,28,1,0,0.0,-309,-566,754##");

        Assert.IsNotNull(ProtocolTester.LastParsedLocation);
    }

    [Test]
    public void DeviceSendsLocationV5_LocationIsParsed()
    {
        ProtocolTester.SendStringFromDevice(
            "$$354898045650537,6031,2015/02/26,15:47:26,33.42552,-112.30308,287.8,0,0,0,0,0.0,7,1.2,2,0.0,12.2,22,1,0,82.3");

        Assert.IsNotNull(ProtocolTester.LastParsedLocation);
    }

    [Test]
    public void DeviceSendsLocationV6_LocationIsParsed()
    {
        ProtocolTester.SendStringFromDevice(
            "$$355922060162167,6015,2016/04/21,17:26:52,39.83267,-76.66139,230,0.0,0.0,0.0,779,0,8,1.2,0,0.0,13.0,19,1,1C4BJWDG4GL191009,X0z1-1137CD1,0402,3GATT,0,83.9,-70,-715,738##");

        Assert.IsNotNull(ProtocolTester.LastParsedLocation);
    }

    [Test]
    public void DeviceSendsLocationV7_LocationIsParsed()
    {
        ProtocolTester.SendStringFromDevice(
            "$$355922060162167,4002,2016/04/21,17:04:50,39.83253,-76.66102,232,0.0,0.0,0.0,0,0,12,1.2,0,0.0,9.2,15,1,0,0.0,35,-8,1059##");

        Assert.IsNotNull(ProtocolTester.LastParsedLocation);
    }
}
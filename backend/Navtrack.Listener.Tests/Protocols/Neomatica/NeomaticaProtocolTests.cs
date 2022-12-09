using Navtrack.Listener.Protocols.Neomatica;
using NUnit.Framework;

namespace Navtrack.Listener.Tests.Protocols.Neomatica;

public class NeomaticaProtocolTests : BaseProtocolTests<NeomaticaProtocol, NeomaticaMessageHandler>
{
    [Test]
    public void DeviceSendsLocation_LocationIsParsed1()
    {
        ProtocolTester.SendHexFromDevice(
            "010042033836313331313030323639343838320501000000000000000000000000000000000000000000000000000000000000000000000000000000000000000073");
        ProtocolTester.SendHexFromDevice(
            "01002200333508202000000000000000007F0D9F030000000000E39A1056E24A8210");

        Assert.IsNotNull(ProtocolTester.LastParsedLocation);
    }

    [Test]
    public void DeviceSendsLocation_LocationIsParsed2()
    {
        ProtocolTester.SendHexFromDevice(
            "010042033836313331313030323639343838320501000000000000000000000000000000000000000000000000000000000000000000000000000000000000000073");
        ProtocolTester.SendHexFromDevice(
            "01002680336510002062A34C423DCF8E42A50B1700005801140767E30F568F2534107D220000");

        Assert.IsNotNull(ProtocolTester.LastParsedLocation);
    }

    [Test]
    public void DeviceSendsLocation_LocationIsParsed3()
    {
        ProtocolTester.SendHexFromDevice(
            "010042033836313331313030323639343838320501000000000000000000000000000000000000000000000000000000000000000000000000000000000000000073");
        ProtocolTester.SendHexFromDevice(
            "0100268033641080207AA34C424CCF8E4239030800005B01140755E30F560000F00F70220000");

        Assert.IsNotNull(ProtocolTester.LastParsedLocation);
    }

    [Test]
    public void DeviceSendsLocation_LocationIsParsed4()
    {
        ProtocolTester.SendHexFromDevice(
            "010042033836313331313030323639343838320501000000000000000000000000000000000000000000000000000000000000000000000000000000000000000073");
        ProtocolTester.SendHexFromDevice(
            "010022003300072020000000000000000044062A330000000000107F10565D4A8310");

        Assert.IsNotNull(ProtocolTester.LastParsedLocation);
    }

    [Test]
    public void DeviceSendsLocation_LocationIsParsed5()
    {
        ProtocolTester.SendHexFromDevice(
            "010042033836313331313030323639343838320501000000000000000000000000000000000000000000000000000000000000000000000000000000000000000073");
        ProtocolTester.SendHexFromDevice(
            "01002680336510002062A34C423DCF8E42A50B1700005801140767E30F568F2534107D220000");

        Assert.IsNotNull(ProtocolTester.LastParsedLocation);
    }
}
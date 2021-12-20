using Navtrack.Listener.Protocols.Navtelecom;
using NUnit.Framework;

namespace Navtrack.Listener.Tests.Protocols.Navtelecom;

public class NavtelecomProtocolV1Tests : BaseProtocolTests<NavtelecomProtocol, NavtelecomMessageHandler>
{
    [Test]
    public void SendLoginAndFlex_RepliesAreReceived()
    {
        // Send login package
        ProtocolTester.SendHexFromDevice(
            "404E5443010000000000000013004A412A3E533A313030303030303030303030303036");
        Assert.AreEqual("404E544300000000010000000300455E2A3C53", ProtocolTester.ReceiveHexInDevice());

        // Send flex package
        ProtocolTester.SendHexFromDevice(
            "404E544301000000000000001300D1DA2A3E464C4558B00A0A45F3EC30000800000000");
        Assert.AreEqual("404E544300000000010000000900B1A02A3C464C4558B00A0A",
            ProtocolTester.ReceiveHexInDevice());
    }
        
    [Test]
    public void DeviceSends_T_ReplyIsReceivedAnd1LocationIsParsed()
    {
        SendLoginAndFlex_RepliesAreReceived();

        // Send T
        ProtocolTester.SendHexFromDevice(
            "7E540C0000000C0000000010A449D45E000C33A449D45E30C45501F0A8FB01000048410C000000000000000000EC");

        Assert.AreEqual("7E540C0000000B", ProtocolTester.ReceiveHexInDevice());
        Assert.IsNotNull(ProtocolTester.LastParsedLocation);
    }

    [Test]
    public void DeviceSends_C_ReplyIsReceivedAnd1LocationIsParsed()
    {
        SendLoginAndFlex_RepliesAreReceived();

        // Send C
        ProtocolTester.SendHexFromDevice(
            "7E43FFFF00000010C649D45E000C33C649D45E30C45501F0A8FB01000048410C00000000000000000037");

        Assert.AreEqual("7E43B9", ProtocolTester.ReceiveHexInDevice());
        Assert.IsNotNull(ProtocolTester.LastParsedLocation);
    }

    [Test]
    public void DeviceSends_A_ReplyIsReceivedAnd3LocationsAreParsed()
    {
        SendLoginAndFlex_RepliesAreReceived();

        // Send A
        ProtocolTester.SendHexFromDevice(
            "7E41030D0000000010E349D45E000C33E349D45E30C45501F0A8FB01000048410C0000000000000000000E0000000010E349D45E000C33E349D45E30C45501F0A8FB01000048410C0000000000000000000F0000000010E349D45E000C33E349D45E30C45501F0A8FB01000048410C000000000000000000F0");

        Assert.AreEqual("7E4103BD", ProtocolTester.ReceiveHexInDevice());
        Assert.AreEqual(3, ProtocolTester.TotalParsedLocations.Count);
    }
}
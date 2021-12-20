using Navtrack.Listener.Protocols.Arusnavi;
using NUnit.Framework;

namespace Navtrack.Listener.Tests.Protocols.Arusnavi;

public class ArusnaviBinaryProtocolTests : BaseProtocolTests<ArusnaviProtocol, ArusnaviMessageHandler>
{
    [Test]
    public void DeviceSendLogin_ServerSendsConfirmation()
    {
        ProtocolTester.SendHexFromDevice("FF23E9EF782DE7120300");

        string confirmation = ProtocolTester.ReceiveHexInDevice();
        Assert.IsTrue(confirmation.StartsWith("7B0400"));
        Assert.IsTrue(confirmation.EndsWith("7D"));
    }

    [Test]
    public void DeviceSends2Locations_2LocationsAreParsedAndServerSendsConfirmation()
    {
        ProtocolTester.SendHexFromDevice("FF23E9EF782DE7120300");

        ProtocolTester.SendHexFromDevice(
            "5B01011400EA59AA5AFCFF0F0700FD67D2C829FE18A7950AFFA2680200E6012800EA59AA5A9766020000075FD69B130802FA001A090060C464010911B43A5B1F000F005C000036005D0000FC025E5D");

        Assert.AreEqual("7B00017D", ProtocolTester.ReceiveHexInDevice());
        Assert.AreEqual(2, ProtocolTester.TotalParsedLocations.Count);
    }

    [Test]
    public void DeviceSends1Location_1LocationsIsParsedAndServerSendsConfirmation1()
    {
        ProtocolTester.SendHexFromDevice("FF23E9EF782DE7120300");

        ProtocolTester.SendHexFromDevice(
            "5B01013C00FD5AAA5A03958B5E42048FD51442050015770097DB000000075FD69B130802FA00180900D0C564010F11EA3A5B070009015C00009C005D00008C00FA32010000A65D");

        Assert.AreEqual("7B00017D", ProtocolTester.ReceiveHexInDevice());
        Assert.IsNotNull(ProtocolTester.LastParsedLocation);
    }

    [Test]
    public void DeviceSends1Location_1LocationsIsParsedAndServerSendsConfirmation2()
    {
        ProtocolTester.SendHexFromDevice("FF23E9EF782DE7120300");

        ProtocolTester.SendHexFromDevice(
            "5B01013C00C361AA5A03848B5E420455D51442050015670097EE000000075FD69B130802FA00190900D0C464010F11E13A5B8D013F015C0000BC015D0000F603FA320100006D5D");

        Assert.AreEqual("7B00017D", ProtocolTester.ReceiveHexInDevice());
        Assert.IsNotNull(ProtocolTester.LastParsedLocation);
    }
}
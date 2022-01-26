using Navtrack.Listener.Protocols.Concox;
using NUnit.Framework;

namespace Navtrack.Listener.Tests.Protocols.Concox;

public class ConcoxProtocolTestsCrx3Mini : BaseProtocolTests<ConcoxProtocol, ConcoxMessageHandler>
{
    [Test]
    public void DeviceSendsLogin_ServerRespondsWithLoginConfirmation1()
    {
        ProtocolTester.SendHexFromDevice("78781101035592910082687520143201000FE5310D0A");

        Assert.AreEqual("787805010001D9DC0D0A", ProtocolTester.ReceiveHexInDevice());
    }

    [Test]
    public void DeviceSendsHeartbeat_ServerRespondsWithConfirmation()
    {
        ProtocolTester.SendHexFromDevice("787808134B040300010011061F0D0A");

        Assert.AreEqual("787805130011F9700D0A", ProtocolTester.ReceiveHexInDevice());
    }

    [Test]
    public void DeviceSendsLocation_LocationIsParsed()
    {
        // Login
        ProtocolTester.SendHexFromDevice("78781101035592910082687520143201000FE5310D0A");

        // Location
        ProtocolTester.SendHexFromDevice(
            "787822220F010100001EC50045712704BC5EF800493702D40B17D2003B1C000E000007169A0D0A");

        Assert.IsNotNull(ProtocolTester.LastParsedLocation);
    }


    [Test]
    public void DeviceSends3Locations_3LocationAreParsed()
    {
        // Login
        ProtocolTester.SendHexFromDevice("78781101035592910082687520143201000FE5310D0A");

        // Locations
        ProtocolTester.SendHexFromDevice(
            "78782222150C0E0F1310C300C62C7003DA22B803D94102D402512F00001F010700000DBBDB0D0A");

        ProtocolTester.SendHexFromDevice(
            "78782222150C0E0F1B0FC300C62C7003DA22B800C94102D402512F00001F010E00000E573C0D0A");

        ProtocolTester.SendHexFromDevice(
            "78782222150C0E0F1C0FC300C62C7003DA22B800C94102D402512F00001F010E0000154E870D0A");

        Assert.AreEqual(3, ProtocolTester.TotalParsedLocations.Count);
    }
}
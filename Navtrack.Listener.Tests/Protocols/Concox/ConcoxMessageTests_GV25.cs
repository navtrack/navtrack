using Navtrack.Listener.Protocols.Concox;
using NUnit.Framework;

namespace Navtrack.Listener.Tests.Protocols.Concox;

public class ConcoxProtocolTestsGv25 : BaseProtocolTests<ConcoxProtocol, ConcoxMessageHandler>
{
    [Test]
    public void DeviceSendsLogin_ServerRespondsWithLoginConfirmation1()
    {
        ProtocolTester.SendHexFromDevice("787811010355929100625814201412C900522C0F0D0A");

        Assert.AreEqual("787805010001D9DC0D0A", ProtocolTester.ReceiveHexInDevice());
    }

    [Test]
    public void DeviceSendsHeartbeat_ServerRespondsWithConfirmation1()
    {
        ProtocolTester.SendHexFromDevice("78780A1305060200020053AF080D0A");

        Assert.AreEqual("78780513005398660D0A", ProtocolTester.ReceiveHexInDevice());
    }

    [Test]
    public void DeviceSendsLocation_LocationIsParsed()
    {
        // Login
        ProtocolTester.SendHexFromDevice("787811010355929100625814201412C900522C0F0D0A");

        // Location
        ProtocolTester.SendHexFromDevice(
            "78782222150C17150402C302224ECC04B5CB0800493D02D41704CF002E23000E0000351CEE0D0A");

        Assert.IsNotNull(ProtocolTester.LastParsedLocation);
    }

    [Test]
    public void DeviceSends3Locations_3LocationAreParsed()
    {
        // Login
        ProtocolTester.SendHexFromDevice("787811010355929100625814201412C900522C0F0D0A");

        // Locations
        ProtocolTester.SendHexFromDevice(
            "78782222150C17150402C302224ECC04B5CB0800493D02D41704CF002E23000E0000351CEE0D0A");

        ProtocolTester.SendHexFromDevice(
            "78782222150C17150502C302224ECC04B5CB0800493D02D41704CF002E23000E00003895430D0A");

        ProtocolTester.SendHexFromDevice(
            "78782222150C17150602C302224ECC04B5CB0800493D02D41704CF002E23000E00003D34360D0A");

        ProtocolTester.SendHexFromDevice(
            "78782222150C17150702C302224ECC04B5CB0800493D02D41704CF002E23000E000040CE1C0D0A");

        ProtocolTester.SendHexFromDevice(
            "78782222150C17150802C302224ECC04B5CB0800493D02D41704CF0029E1000E000044758E0D0A");

        ProtocolTester.SendHexFromDevice(
            "78782222150C17150902C302224ECC04B5CB0800493D02D41704CF0029E1000E000048EDAA0D0A");

        Assert.AreEqual(6, ProtocolTester.TotalParsedLocations.Count);
    }
}
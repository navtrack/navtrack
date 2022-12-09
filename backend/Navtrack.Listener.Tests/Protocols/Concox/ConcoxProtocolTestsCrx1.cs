using Navtrack.Listener.Protocols.Concox;
using NUnit.Framework;

namespace Navtrack.Listener.Tests.Protocols.Concox;

public class ConcoxProtocolTestsCrx1 : BaseProtocolTests<ConcoxProtocol, ConcoxMessageHandler>
{
    [Test]
    public void DeviceSendsLogin_ServerRespondsWithLoginConfirmation1()
    {
        ProtocolTester.SendHexFromDevice("787811010355929100767103201412C90001E4420D0A");

        Assert.AreEqual("787805010001D9DC0D0A", ProtocolTester.ReceiveHexInDevice());
    }

    [Test]
    public void DeviceSendsLocation_LocationIsParsed()
    {
        // Login
        ProtocolTester.SendHexFromDevice("787811010355929100767103201412C90001E4420D0A");

        // Location
        ProtocolTester.SendHexFromDevice(
            "787822220F010100001EC50045712704BC5EF800493702D40B17D2003B1C000E000007169A0D0A");

        Assert.IsNotNull(ProtocolTester.LastParsedLocation);
    }


    [Test]
    public void DeviceSends3Locations_3LocationAreParsed()
    {
        // Login
        ProtocolTester.SendHexFromDevice("787811010355929100767103201412C90001E4420D0A");

        // Locations
        ProtocolTester.SendHexFromDevice(
            "78782222150B120D241FC60045716A04BC5F1800580402D40B2006003BBD00000100011F390D0A");

        ProtocolTester.SendHexFromDevice(
            "78782222150B120D242BC60045716A04BC5F1800480402D40B2006003BBD000E010002DA320D0A");

        ProtocolTester.SendHexFromDevice(
            "78782222150B120D3807C60045716A04BC5F180048040000000000000000000E010000DD380D0A");

        Assert.AreEqual(3, ProtocolTester.TotalParsedLocations.Count);
    }
}
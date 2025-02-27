using Navtrack.Listener.Protocols.Concox;
using Xunit;

namespace Navtrack.Listener.Tests.Protocols.Concox;

public class ConcoxProtocolTests : BaseProtocolTests<ConcoxProtocol, ConcoxMessageHandler>
{
    [Fact]
    public void DeviceSendsLogin_ServerRespondsWithLoginConfirmation()
    {
        ProtocolTester.SendHexFromDevice("78780D01012345678901234500018CDD0D0A");

        Assert.Equal("787805010001D9DC0D0A", ProtocolTester.ReceiveHexInDevice());
    }

    [Fact]
    public void DeviceSendsHeartbeat_ServerRespondsWithAcknowledge()
    {
        ProtocolTester.SendHexFromDevice("787808134B040300010011061F0D0A");

        Assert.Equal("787805130011F9700D0A", ProtocolTester.ReceiveHexInDevice());
    }

    [Fact]
    public void DeviceSendsLocation_LocationIsParsed()
    {
        // Login
        ProtocolTester.SendHexFromDevice("78780D01012345678901234500018CDD0D0A");

        // Location
        ProtocolTester.SendHexFromDevice(
            "7878 1F 12 0A03170F3217 CC 026B3F3E 0C465849 00 154C 01CC 00 287D 001FB8 0003 8081 0D0A");
        ProtocolTester.SendHexFromDevice(
            "7878 22 22 0F0101051814 C4 0222518C 04B5AA48 00 4826 02D4 17 04CF 003F35 000E 0100 128F8B0D0A");
        
        Assert.NotNull(ProtocolTester.LastParsedMessage);
    }
    
    [Fact]
    public void DeviceSendsLocation2_LocationIsParsed()
    {
        // Login
        ProtocolTester.SendHexFromDevice("787811010355929100625814201412C900151AB40D0A");

        // Location
        ProtocolTester.SendHexFromDevice(
            "787822220F0101051814C40222518C04B5AA4800482602D41704CF003F35000E0100128F8B0D0A");

        Assert.NotNull(ProtocolTester.LastParsedMessage);
    }
}
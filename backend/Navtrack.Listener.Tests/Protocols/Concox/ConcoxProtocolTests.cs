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
            "78781F120B081D112E10CC027AC7EB0C46584900148F01CC00287D001FB8000380810D0A");

        Assert.NotNull(ProtocolTester.LastParsedLocation);
    }
}
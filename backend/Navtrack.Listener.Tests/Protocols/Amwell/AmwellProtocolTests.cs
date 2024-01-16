using Navtrack.Listener.Protocols.Amwell;
using Xunit;

namespace Navtrack.Listener.Tests.Protocols.Amwell;

public class AmwellProtocolTests : BaseProtocolTests<AmwellProtocol, AmwellMessageHandler>
{
    [Fact]
    public void DeviceSendsLogin_ServerRespondsWithLoginConfirmation()
    {
        ProtocolTester.SendHexFromDevice("2929B100070A9F95380C820D");

        Assert.Equal("292921000582B106110D", ProtocolTester.ReceiveHexInDevice());
    }

    [Fact]
    public void DeviceSendsLocation_LocationIsParsed()
    {
        // Login            
        ProtocolTester.SendHexFromDevice("2929B100070A9F95380C820D");

        // Location
        ProtocolTester.SendHexFromDevice(
            "29298100280A9F9538081228160131022394301140372500000330FF0000007FFC0F00001E000000000034290D");

        Assert.NotNull(ProtocolTester.LastParsedLocation);
    }
}
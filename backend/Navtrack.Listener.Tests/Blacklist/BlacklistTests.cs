using Navtrack.Listener.Protocols.Teltonika;
using Navtrack.Listener.Tests.Protocols;
using Xunit;

namespace Navtrack.Listener.Tests.Blacklist;

public class BlacklistTests : BaseProtocolTests<TeltonikaProtocol, TeltonikaMessageHandler>
{
    [Fact]
    public void Codec8Extended_SendHttpMessage_ServerIgnoresIt()
    {
        ProtocolTester.SendHexFromDevice(
            "474554202F20485454502F312E310D0A486F73743A203131362E3230332E3134332E3136343A373030320D0A0D0A");

        Assert.Null(ProtocolTester.ConnectionContext.Device);
    }
}
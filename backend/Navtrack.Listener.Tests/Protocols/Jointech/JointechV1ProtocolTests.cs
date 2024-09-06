using Navtrack.Listener.Protocols.Jointech;
using Xunit;

namespace Navtrack.Listener.Tests.Protocols.Jointech;

public class JointechV1ProtocolTests : BaseProtocolTests<JointechProtocol, JointechMessageHandler>
{
    [Fact]
    public void DeviceSendsLocationV1_LocationIsParsed()
    {
        ProtocolTester.SendHexFromDevice("24608111888821001B09060908045322564025113242329F0598000001003F0000002D00AB");

        Assert.NotNull(ProtocolTester.LastParsedMessage);
    }
}
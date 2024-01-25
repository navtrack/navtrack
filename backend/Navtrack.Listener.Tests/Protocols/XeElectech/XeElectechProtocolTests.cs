using Navtrack.Listener.Protocols.XeElectech;
using Xunit;

namespace Navtrack.Listener.Tests.Protocols.XeElectech;

public class XeElectechProtocolTests : BaseProtocolTests<XeElectechProtocol, XeElectechMessageHandler>
{
    [Fact]
    public void DeviceSendsLocationV1_LocationIsParsed()
    {
        ProtocolTester.SendStringFromDevice("*HQ,4106016320,V1,090458,A,5257.4318,N,15840.4221,E,000.00,000,101115,FFFFFBFF,250,01,0,0,5#");

        Assert.NotNull(ProtocolTester.LastParsedPosition);
    }
}
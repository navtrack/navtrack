using Navtrack.Listener.Protocols.Topfly;
using NUnit.Framework;

namespace Navtrack.Listener.Tests.Protocols.Topfly;

public class TopflyProtocolTests : BaseProtocolTests<TopflyProtocol, TopflyMessageHandler>
{
    [Test]
    public void DeviceSendsLocationV1_LocationIsParsed()
    {
        ProtocolTester.SendStringFromDevice(
            "(880316890094910BP00XG00b600000000L00074b54S00000000R0C0F0014000100f0130531152205A0706.1395S11024.0965E000.0251.25");

        Assert.IsNotNull(ProtocolTester.LastParsedLocation);
    }
}
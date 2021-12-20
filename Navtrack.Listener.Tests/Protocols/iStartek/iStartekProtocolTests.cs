using Navtrack.Listener.Protocols.iStartek;
using NUnit.Framework;

namespace Navtrack.Listener.Tests.Protocols.iStartek;

// ReSharper disable once InconsistentNaming
public class iStartekProtocolTests : BaseProtocolTests<iStartekProtocol, iStartekMessageHandler>
{
    [Test]
    public void DeviceSendsLocationV1_LocationIsParsed()
    {
        ProtocolTester.SendHexFromDevice(
            "24240060123456FFFFFFFF99553033353634342E3030302C412C323233322E363038332C4E2C31313430342E383133372C452C302E30302C2C3031303830392C2C2A31437C31312E357C3139347C303030307C303030302C3030303069620D0A");

        Assert.IsNotNull(ProtocolTester.LastParsedLocation);
    }
}
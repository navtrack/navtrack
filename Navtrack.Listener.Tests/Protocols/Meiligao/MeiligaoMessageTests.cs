using Navtrack.Listener.Protocols.Meiligao;
using NUnit.Framework;

namespace Navtrack.Listener.Tests.Protocols.Meiligao;

public class MeiligaoProtocolTests : BaseProtocolTests<MeiligaoProtocol, MeiligaoMessageHandler>
{
    [Test]
    public void DeviceSendsLoginCommand_ServerRespondsWithLoginConfirmation()
    {
        ProtocolTester.SendHexFromDevice("24240011123456FFFFFFFF50008B9B0D0A");

        Assert.AreEqual("40400012123456FFFFFFFF400001A99B0D0A", ProtocolTester.ReceiveHexInDevice());
    }

    [Test]
    public void DeviceSendsLocationV1_LocationIsParsed()
    {
        ProtocolTester.SendHexFromDevice(
            "24240060123456FFFFFFFF99553033353634342E3030302C412C323233322E363038332C4E2C31313430342E383133372C452C302E30302C2C3031303830392C2C2A31437C31312E357C3139347C303030307C303030302C3030303069620D0A");

        Assert.IsNotNull(ProtocolTester.LastParsedLocation);
    }
        
    [Test]
    public void DeviceSendsLocationV2_LocationIsParsed()
    {
        ProtocolTester.SendHexFromDevice(
            "24240060123456FFFFFFFF99553033353634342E3030302C412C323233322E363038332C4E2C31313430342E383133372C452C302E30302C2C3031303830392C2C2A31437C31312E357C3139347C303030307C303030302C3030303069620D0A2C2C2A31437C31312E357C31393470");

        Assert.IsNotNull(ProtocolTester.LastParsedLocation);
    }
        
    [Test]
    public void DeviceSendsLocationV3_LocationIsParsed()
    {
        ProtocolTester.SendHexFromDevice(
            "FF99553033353634342E303024240060123456FFFFFFFF99553033353634342E3030302C412C323233322E363038332C4E2C31313430342E383133372C452C302E30302C2C3031303830392C2C2A31437C31312E357C3139347C303030307C303030302C3030303069620D0A2C2C2A31437C31312E357C31393470");

        Assert.IsNotNull(ProtocolTester.LastParsedLocation);
    }
}
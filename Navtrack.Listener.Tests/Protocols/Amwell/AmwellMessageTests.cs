using Navtrack.Listener.Protocols.Amwell;
using NUnit.Framework;

namespace Navtrack.Listener.Tests.Protocols.Amwell;

public class AmwellProtocolTests : BaseProtocolTests<AmwellProtocol, AmwellMessageHandler>
{
    [Test]
    public void DeviceSendsLogin_ServerRespondsWithLoginConfirmation()
    {
        ProtocolTester.SendHexFromDevice("2929B100070A9F95380C820D");

        Assert.AreEqual("292921000582B106110D", ProtocolTester.ReceiveHexInDevice());
    }

    [Test]
    public void DeviceSendsLocation_LocationIsParsed()
    {
        // Login            
        ProtocolTester.SendHexFromDevice("2929B100070A9F95380C820D");

        // Location
        ProtocolTester.SendHexFromDevice(
            "29298100280A9F9538081228160131022394301140372500000330FF0000007FFC0F00001E000000000034290D");

        Assert.IsNotNull(ProtocolTester.LastParsedLocation);
    }
}
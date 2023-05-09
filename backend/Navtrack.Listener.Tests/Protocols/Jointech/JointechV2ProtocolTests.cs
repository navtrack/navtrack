using Navtrack.Listener.Protocols.Jointech;
using NUnit.Framework;

namespace Navtrack.Listener.Tests.Protocols.Jointech;

public class JointechV2ProtocolTests : BaseProtocolTests<JointechProtocol, JointechMessageHandler>
{
    [Test]
    public void DeviceSendsLocationV2_LocationIsParsedAndResponseIsSent()
    {
        ProtocolTester.SendHexFromDevice(
            "283737353231303430383030302C312C3030312C424153452C322C54494D4529");

        ProtocolTester.SendHexFromDevice(
            "2400000000018000000000000000000000000000000000230114172752300163310100D40144D50200A0DA03000100DB02002BDC0400000000FD0902EC01000002790002F8020307AB7E");
        
        ProtocolTester.SendHexFromDevice(
            "7E020000477752104080000046000000000104100E021436960357B26600000000000023032022393030011C310108D4012FD5020050DA03000605DB0201B0DC0400000000FD0902EC07000022A32168F8020330EE7E");

        Assert.IsNotNull(ProtocolTester.LastParsedLocation);

        Assert.AreEqual("7E8001000577521040800000000046020000357E", ProtocolTester.ReceiveHexInDevice());
    }
}
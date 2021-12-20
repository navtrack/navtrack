using Navtrack.Listener.Protocols.Laipac;
using NUnit.Framework;

namespace Navtrack.Listener.Tests.Protocols.Laipac;

public class LaipacProtocolTests : BaseProtocolTests<LaipacProtocol, LaipacMessageHandler>
{
    [Test]
    public void DeviceSendsLogin_AcknowledgeIsSentBack()
    {
        ProtocolTester.SendStringFromDevice(
            "$AVSYS,99999999,V1.50,SN0000103,32768*15\r\n");
        ProtocolTester.SendStringFromDevice(
            "$ECHK,99999999,0*35\r\n");

        Assert.AreEqual("$ECHK,99999999,0*35\r\n", ProtocolTester.ReceiveStringInDevice());
    }

    [Test]
    public void DeviceSendsLocationV1_LocationIsParsed()
    {
        ProtocolTester.SendStringFromDevice(
            "$AVRMC,99999999,164339,A,4351.0542,N,07923.5445,W,0.29,78.66,180703,0,3.727,17,1,0,0*37\r\n");

        Assert.IsNotNull(ProtocolTester.LastParsedLocation);
    }
        
    [Test]
    public void DeviceSendsLocationV2_LocationIsParsed()
    {
        ProtocolTester.SendStringFromDevice(
            "$AVRMC,99999999,164339,a,4351.0542,N,07923.5445,W,0.29,78.66,180703,0,3.727,17,1,0,0*17\r\n");

        Assert.IsNotNull(ProtocolTester.LastParsedLocation);
        Assert.AreEqual("$EAVACK,0,17*2D\r\n", ProtocolTester.ReceiveStringInDevice());
    }

    [Test]
    public void DeviceSendsLocationV3_LocationIsParsed()
    {
        ProtocolTester.SendStringFromDevice(
            "$AVRMC,99999999,164339,v,4351.0542,N,07923.5445,W,0.29,78.66,180703,0,3.727,17,1,0,0*00\r\n");

        Assert.IsNotNull(ProtocolTester.LastParsedLocation);
        Assert.AreEqual("$EAVACK,0,00*2B\r\n", ProtocolTester.ReceiveStringInDevice());
    }

    [Test]
    public void DeviceSendsLocationV4_LocationIsParsed()
    {
        ProtocolTester.SendStringFromDevice(
            "$AVRMC,99999999,164339,r,4351.0542,N,07923.5445,W,0.29,78.66,180703,0,3.727,17,1,0,0*04\r\n");

        Assert.IsNotNull(ProtocolTester.LastParsedLocation);
        Assert.AreEqual("$EAVACK,0,04*2F\r\n", ProtocolTester.ReceiveStringInDevice());
    }
}
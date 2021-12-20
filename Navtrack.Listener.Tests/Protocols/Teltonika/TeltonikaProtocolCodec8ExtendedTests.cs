using Navtrack.Listener.Protocols.Teltonika;
using NUnit.Framework;

namespace Navtrack.Listener.Tests.Protocols.Teltonika;

public class TeltonikaProtocolCodec8ExtendedTests : BaseProtocolTests<TeltonikaProtocol, TeltonikaMessageHandler>
{
    [Test]
    public void Codec8Extended_DeviceSendImei_ServerReturnsAcknowledge()
    {
        ProtocolTester.SendHexFromDevice("000F333536333037303432343431303133");

        Assert.AreEqual("01", ProtocolTester.ReceiveHexInDevice());
    }

    [Test]
    public void Codec8Extended_DeviceSendsLocationV1_ServerConfirmsDataReceived()
    {
        ProtocolTester.SendHexFromDevice("000F333536333037303432343431303133");

        Assert.AreEqual("01", ProtocolTester.ReceiveHexInDevice());
        Assert.IsNotNull(ProtocolTester.Client.Device);

        ProtocolTester.SendHexFromDevice(
            "000000000000004A8E010000016B412CEE000100000000000000000000000000000000010005000100010100010011001D00010010015E2C880002000B000000003544C87A000E000000001DD7E06A00000100002994");

        Assert.AreEqual("00000001", ProtocolTester.ReceiveHexInDevice());
        Assert.AreEqual(1, ProtocolTester.LastParsedLocations.Count);
    }
        
    [Test]
    public void Codec8Extended_DeviceSendsLocationV2_ServerConfirmsDataReceived()
    {
        ProtocolTester.SendHexFromDevice("000F333536333037303432343431303133");

        Assert.AreEqual("01", ProtocolTester.ReceiveHexInDevice());
        Assert.IsNotNull(ProtocolTester.Client.Device);

        ProtocolTester.SendHexFromDevice(
            "000000000000002808010000016B40D9AD80010000000000000000000000000000000103021503010101425E100000010000F22A");

        Assert.AreEqual("00000001", ProtocolTester.ReceiveHexInDevice());
        Assert.AreEqual(1, ProtocolTester.LastParsedLocations.Count);
    }
}
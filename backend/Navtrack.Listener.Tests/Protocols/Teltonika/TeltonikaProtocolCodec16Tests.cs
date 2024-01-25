using Navtrack.Listener.Protocols.Teltonika;
using Xunit;

namespace Navtrack.Listener.Tests.Protocols.Teltonika;

public class TeltonikaProtocolCodec16Tests : BaseProtocolTests<TeltonikaProtocol, TeltonikaMessageHandler>
{
    [Fact]
    public void Codec16_DeviceSendImei_ServerReturnsAcknowledge()
    {
        ProtocolTester.SendHexFromDevice("000F333536333037303432343431303133");

        Assert.Equal("01", ProtocolTester.ReceiveHexInDevice());
    }

    [Fact]
    public void Codec16_DeviceSendsLocationV1_ServerConfirmsDataReceived()
    {
        ProtocolTester.SendHexFromDevice("000F333536333037303432343431303133");

        Assert.Equal("01", ProtocolTester.ReceiveHexInDevice());
        Assert.NotNull(ProtocolTester.ConnectionContext.Device);

        ProtocolTester.SendHexFromDevice(
            "000000000000005F10020000016BDBC7833000000000000000000000000000000000000B05040200010000030002000B00270042563A00000000016BDBC7871800000000000000000000000000000000000B05040200010000030002000B00260042563A00000200005FB3");

        Assert.Equal("00000002", ProtocolTester.ReceiveHexInDevice());
        Assert.Equal(2, ProtocolTester.LastParsedPositions.Count);
    }
        
    [Fact]
    public void Codec8_DeviceSendsLocationV2_ServerConfirmsDataReceived()
    {
        ProtocolTester.SendHexFromDevice("000F333536333037303432343431303133");

        Assert.Equal("01", ProtocolTester.ReceiveHexInDevice());
        Assert.NotNull(ProtocolTester.ConnectionContext.Device);

        ProtocolTester.SendHexFromDevice(
            "000000000000002808010000016B40D9AD80010000000000000000000000000000000103021503010101425E100000010000F22A");

        Assert.Equal("00000001", ProtocolTester.ReceiveHexInDevice());
        Assert.Single(ProtocolTester.LastParsedPositions);
    }

    [Fact]
    public void Codec8_DeviceSendsLocationV3_ServerConfirmsDataReceived()
    {
        ProtocolTester.SendHexFromDevice("000F333536333037303432343431303133");

        Assert.Equal("01", ProtocolTester.ReceiveHexInDevice());
        Assert.NotNull(ProtocolTester.ConnectionContext.Device);

        ProtocolTester.SendHexFromDevice(
            "000000000000004308020000016B40D57B480100000000000000000000000000000001010101000000000000016B40D5C198010000000000000000000000000000000101010101000000020000252C");

        Assert.Equal("00000002", ProtocolTester.ReceiveHexInDevice());
        Assert.Equal(2, ProtocolTester.LastParsedPositions.Count);
    }
}
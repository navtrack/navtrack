using Navtrack.Listener.Protocols.Teltonika;
using Xunit;

namespace Navtrack.Listener.Tests.Protocols.Teltonika;

public class TeltonikaProtocolCodec8ExtendedTests : BaseProtocolTests<TeltonikaProtocol, TeltonikaMessageHandler>
{
    [Fact]
    public void Codec8Extended_DeviceSendImei_ServerReturnsAcknowledge()
    {
        ProtocolTester.SendHexFromDevice("000F333536333037303432343431303133");

        Assert.Equal("01", ProtocolTester.ReceiveHexInDevice());
    }

    [Fact]
    public void Codec8Extended_DeviceSendsLocationV1_ServerConfirmsDataReceived()
    {
        ProtocolTester.SendHexFromDevice("000F333536333037303432343431303133");

        Assert.Equal("01", ProtocolTester.ReceiveHexInDevice());
        Assert.NotNull(ProtocolTester.ConnectionContext.Device);

        ProtocolTester.SendHexFromDevice(
            "000000000000004A8E010000016B412CEE000100000000000000000000000000000000010005000100010100010011001D00010010015E2C880002000B000000003544C87A000E000000001DD7E06A00000100002994");

        Assert.Equal("00000001", ProtocolTester.ReceiveHexInDevice());
        Assert.Single(ProtocolTester.LastParsedMessages);
    }
        
    [Fact]
    public void Codec8Extended_DeviceSendsLocationV2_ServerConfirmsDataReceived()
    {
        ProtocolTester.SendHexFromDevice("000F333536333037303432343431303133");

        Assert.Equal("01", ProtocolTester.ReceiveHexInDevice());
        Assert.NotNull(ProtocolTester.ConnectionContext.Device);

        ProtocolTester.SendHexFromDevice(
            "000000000000002808010000016B40D9AD80010000000000000000000000000000000103021503010101425E100000010000F22A");

        Assert.Equal("00000001", ProtocolTester.ReceiveHexInDevice());
        Assert.Single(ProtocolTester.LastParsedMessages);
    }
    
    [Fact]
    public void Codec8Extended_DeviceSendsLocationV3_ServerConfirmsDataReceived()
    {
        ProtocolTester.SendHexFromDevice("000F333532383438303236343231383232");

        Assert.Equal("01", ProtocolTester.ReceiveHexInDevice());
        Assert.NotNull(ProtocolTester.ConnectionContext.Device);

        ProtocolTester.SendHexFromDevice(
            "00000000000001D70809000001914BE897ED000E127CD01BE2EEA00140006006001300070301014501F00102B600114234C802C700000031F10000585200000001914BE8705B000E1276C01BE2FD600131007B06000000070301014501F00102B6001142356302C700000008F10000585200000001914BE84833000E1275901BE2FE2000EB007405001400070301014501F00102B6001242341202C70000004EF10000585200000001914BE820A3000E1252D01BE30AC0009F006905001400070301014501F00102B6001242359C02C70000005EF10000585200000001914BE7F90F000E1225201BE30F600069006405001F00070301014501F00102B6001242358402C70000001CF10000585200000001914BE7D17D000E1214D01BE31540005E00C704000000070301014501F00102B600244235E502C700000000F10000585200000001914BE7A9EB000E1214D01BE31540005D00C704000000070301014501F00102B6002442352002C700000019F10000585200000001914BE7825B000E1213B01BE31DE0001800D404000900070301014501F00102B6002442353F02C700000000F10000000000000001914BD74193000E1248C01BE2EE00014E012108000700070301014501F00102B6000C42355A02C700000000F100005852000900009607");

        Assert.Equal("00000009", ProtocolTester.ReceiveHexInDevice());
        Assert.Equal(9, ProtocolTester.LastParsedMessages.Count);
    }
    
    [Fact]
    public void Codec8Extended_DeviceSendsLocationV4_ServerConfirmsDataReceived()
    {
        ProtocolTester.SendHexFromDevice("000F333532383438303236343231383232");

        Assert.Equal("01", ProtocolTester.ReceiveHexInDevice());
        Assert.NotNull(ProtocolTester.ConnectionContext.Device);

        ProtocolTester.SendHexFromDevice(
            "00000000000000370801000001914BE8BF43000E12D3601BE2E5E0013E005E06002B00070301014501F00102B6001142355802C7000000A3F100005852000100009A2D");

        Assert.Equal("00000001", ProtocolTester.ReceiveHexInDevice());
        Assert.Single(ProtocolTester.LastParsedMessages);
    }
    
    [Fact]
    public void Codec8Extended_DeviceSendsLocationV5_ServerConfirmsDataReceived()
    {
        ProtocolTester.SendHexFromDevice("000F333532383438303236343231383232");

        Assert.Equal("01", ProtocolTester.ReceiveHexInDevice());
        Assert.NotNull(ProtocolTester.ConnectionContext.Device);

        ProtocolTester.SendHexFromDevice(
            "00000000000000370801000001914BE8E6D5000E1307301BE2E7800141003406002000070301014501F00102B6001142355002C70000006CF10000585200010000B00C");

        Assert.Equal("00000001", ProtocolTester.ReceiveHexInDevice());
        Assert.Single(ProtocolTester.LastParsedMessages);
    }
}
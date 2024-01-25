using Navtrack.Listener.Protocols.Navtelecom;
using Xunit;

namespace Navtrack.Listener.Tests.Protocols.Navtelecom;

public class NavtelecomProtocolV2Tests : BaseProtocolTests<NavtelecomProtocol, NavtelecomMessageHandler>
{
    [Fact]
    public void SendLoginAndFlex_RepliesAreReceived()
    {
        // Send login package
        ProtocolTester.SendHexFromDevice(
            "404E5443010000000000000013004A412A3E533A313030303030303030303030303036");
        Assert.Equal("404E544300000000010000000300455E2A3C53", ProtocolTester.ReceiveHexInDevice());

        // Send flex package
        ProtocolTester.SendHexFromDevice(
            "404E544301000000000000001A002D2F2A3E464C4558B014147AFFFFFC0FFF9F0FFFBFFF80007FF807C7");
        Assert.Equal("404E544300000000010000000900B1A02A3C464C4558B01414",
            ProtocolTester.ReceiveHexInDevice());
    }
        
    [Fact]
    public void DeviceSends_T_ReplyIsReceivedAnd1LocationIsParsed()
    {
        SendLoginAndFlex_RepliesAreReceived();

        // Send T
        ProtocolTester.SendHexFromDevice(
            "7E5411000000110000001119B94AD45E000100140BB94AD45E9BC155011CAAFB01140000000000E04108000000624300000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000808080800080000080BFFFFF80000080BFFFFFFFFFFFFFFFFFFFFFFFFFFF00000000FFFFFF0000000000000000010100805B06370500000080E90BBF070000000000000000000000000000000000000000000000DD00000000000000000000DD00000000000000000000DDB94AD45E00000000000000000000000000B94AD45E00FFFFFFFFFFFF00000000000000000000000000000000000000000000E8");

        Assert.Equal("7E5411000000BA", ProtocolTester.ReceiveHexInDevice());
        Assert.NotNull(ProtocolTester.LastParsedPosition);
    }

    [Fact]
    public void DeviceSends_C_ReplyIsReceivedAnd1LocationIsParsed()
    {
        SendLoginAndFlex_RepliesAreReceived();

        // Send C
        ProtocolTester.SendHexFromDevice(
            "7E43FFFF00001119DA4AD45E000100140BDA4AD45E9BC155011CAAFB01140000000000E04108000000624300000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000808080800080000080BFFFFF80000080BFFFFFFFFFFFFFFFFFFFFFFFFFFF00000000FFFFFF0000000000000000010100805B06370500000080E90BBF070000000000000000000000000000000000000000000000DD00000000000000000000DD00000000000000000000DDDA4AD45E00000000000000000000000000DA4AD45E00FFFFFFFFFFFF0000000000000000000000000000000000000000000082");

        Assert.Equal("7E43B9", ProtocolTester.ReceiveHexInDevice());
        Assert.NotNull(ProtocolTester.LastParsedPosition);
    }

    [Fact]
    public void DeviceSends_X_ReplyIsReceivedAnd1LocationIsParsed()
    {
        SendLoginAndFlex_RepliesAreReceived();

        // Send X
        ProtocolTester.SendHexFromDevice(
            "7E581200000027000A25120000001119F94AD45E0BF94AD45E9BC155011CAAFB01140000000000E041080000006243BB");

        Assert.Equal("7E581200000007", ProtocolTester.ReceiveHexInDevice());
        Assert.NotNull(ProtocolTester.LastParsedPosition);
    }

    [Fact]
    public void DeviceSends_A_ReplyIsReceivedAnd3LocationsAreParsed()
    {
        SendLoginAndFlex_RepliesAreReceived();

        // Send A
        ProtocolTester.SendHexFromDevice(
            "7E41031300000011192A4BD45E000100140B2A4BD45E9BC155011CAAFB01140000000000E04108000000624300000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000808080800080000080BFFFFF80000080BFFFFFFFFFFFFFFFFFFFFFFFFFFF00000000FFFFFF0000000000000000010100805B06370500000080E90BBF070000000000000000000000000000000000000000000000DD00000000000000000000DD00000000000000000000DD2A4BD45E000000000000000000000000002A4BD45E00FFFFFFFFFFFF000000000000000000000000000000000000000000001400000011192A4BD45E000100140B2A4BD45E9BC155011CAAFB01140000000000E04108000000624300000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000808080800080000080BFFFFF80000080BFFFFFFFFFFFFFFFFFFFFFFFFFFF00000000FFFFFF0000000000000000010100805B06370500000080E90BBF070000000000000000000000000000000000000000000000DD00000000000000000000DD00000000000000000000DD2A4BD45E000000000000000000000000002A4BD45E00FFFFFFFFFFFF000000000000000000000000000000000000000000001500000011192A4BD45E000100140B2A4BD45E9BC155011CAAFB01140000000000E04108000000624300000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000808080800080000080BFFFFF80000080BFFFFFFFFFFFFFFFFFFFFFFFFFFF00000000FFFFFF0000000000000000010100805B06370500000080E90BBF070000000000000000000000000000000000000000000000DD00000000000000000000DD00000000000000000000DD2A4BD45E000000000000000000000000002A4BD45E00FFFFFFFFFFFF000000000000000000000000000000000000000000000D");

        Assert.Equal("7E4103BD", ProtocolTester.ReceiveHexInDevice());
        Assert.Equal(3, ProtocolTester.TotalParsedPositions.Count);
    }

    [Fact]
    public void DeviceSends_E_ReplyIsReceivedAnd3LocationsAreParsed()
    {
        SendLoginAndFlex_RepliesAreReceived();

        // Send E
        ProtocolTester.SendHexFromDevice(
            "7E450327000A251600000011194A4BD45E0B4A4BD45E9BC155011CAAFB01140000000000E04108000000624327000A251700000011194A4BD45E0B4A4BD45E9BC155011CAAFB01140000000000E04108000000624327000A251800000011194A4BD45E0B4A4BD45E9BC155011CAAFB01140000000000E04108000000624310");

        Assert.Equal("7E45033E", ProtocolTester.ReceiveHexInDevice());
        Assert.Equal(3, ProtocolTester.TotalParsedPositions.Count);
    }

    [Fact]
    public void DeviceSends_E_ReplyIsReceivedAnd1LocationIsParsed()
    {
        SendLoginAndFlex_RepliesAreReceived();

        // Send E
        ProtocolTester.SendHexFromDevice(
            "7E450127000A251900000011196A4BD45E0B6A4BD45E9BC155011CAAFB01140000000000E0410800000062433C");

        Assert.Equal("7E45015C", ProtocolTester.ReceiveHexInDevice());
        Assert.Single(ProtocolTester.TotalParsedPositions);
    }
}
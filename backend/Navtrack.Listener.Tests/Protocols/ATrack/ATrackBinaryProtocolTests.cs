using Navtrack.Listener.Protocols.ATrack;
using Xunit;

namespace Navtrack.Listener.Tests.Protocols.ATrack;

public class ATrackBinaryProtocolTests : BaseProtocolTests<ATrackProtocol, ATrackMessageHandler>
{
    [Fact]
    public void DeviceSendsKeepAlive_ReplyIsSent()
    {
        ProtocolTester.SendHexFromDevice("FE0200014104D8F196820001");

        Assert.Equal("00014104D8F196820001", ProtocolTester.ReceiveHexInDevice());
    }
        
    [Fact]
    public void DeviceSends1Location_1LocationIsParsed_1()
    {
        ProtocolTester.SendHexFromDevice(
            "40501E58003301E000014104D8F19682525ECD5D525EE344525EE35EFFC88815026AB4D70000020000104403DE01000B0000000007D007D000000000000000");

        Assert.Single(ProtocolTester.TotalParsedPositions);
    }

    [Fact]
    public void DeviceSends1Location_1LocationIsParsedAndResponseIsSent_2()
    {
        ProtocolTester.SendHexFromDevice(
            "4050B5ED004A2523000310C83713F8C05A88B43E5A88B43F5A88B43F021E0AD5FFFDC0A800F3020003059100080000000000000007D007D046554C533A463D3230393120743D3137204E3D3039303100");

        Assert.Single(ProtocolTester.TotalParsedPositions);
        Assert.Equal("FE02000310C83713F8C02523", ProtocolTester.ReceiveHexInDevice());
    }

    [Fact]
    public void DeviceSends1Location_1LocationIsParsedAndResponseIsSent_3()
    {
        ProtocolTester.SendHexFromDevice(
            "0203B494003C00EB00014104D8DD3A3E07DE011B0B1F0307DE011B0B1F0307DE011B0B1F0300307F28030574D30000020000000600160100020000000007D007D000");

        Assert.Single(ProtocolTester.TotalParsedPositions);
        Assert.Equal("FE0200014104D8DD3A3E00EB", ProtocolTester.ReceiveHexInDevice());
    }

    [Fact]
    public void DeviceSends1Location_1LocationIsParsed_4()
    {
        ProtocolTester.SendHexFromDevice(
            "405AD77C01670410000144A77A21281D5B74D2335B74D2335B74D233FABAF3BC02A38D3D010C0200030F8E000701001A0000000007D007D00025434925434525434E25475125475325464C254D4C25564E25504425464325454C254554254344254154254D46254D5625425625434D25445425474C25474E254756254C43254D4525524C25525025534125534D255452254941254D5000000000000004BBF41C0900003254314B523332453238433730363138350000000000004800543839303132363038383132313532343737353900000000EC06A50089002900011A2E3DA1882700687474703A2F2F6D6170732E676F6F676C652E636F6D2F6D6170733F713D34342E3237323935372C2D38382E34313132303120000075FF4903FB006FFF63040A004DFF5D04080030FFA10407003B001304060026000503F9001E002504020078FF6204000073FF7D03F9007AFF6903F3FFC0001804040000000144A77A21281D00073F0C001A182400");

        Assert.Single(ProtocolTester.TotalParsedPositions);
    }

    [Fact]
    public void DeviceSends1Location_1LocationIsParsed_5()
    {
        ProtocolTester.SendHexFromDevice(
            "40503835003300070001441C3D8ED1C400000000000000C9000000C900000000000000000000020000000003DE0100000000000007D007D000");

        Assert.Single(ProtocolTester.TotalParsedPositions);
    }

    [Fact]
    public void DeviceSends1Location_1LocationIsParsed_6()
    {
        ProtocolTester.SendHexFromDevice(
            "4050993F005C000200014104D8F19682525666C252568C3C52568C63FFC8338402698885000002000009CF03DE0100000000000007D007D000525666C252568C5A52568C63FFC8338402698885000002000009CF03DE0100000000000007D007D000");

        Assert.Single(ProtocolTester.TotalParsedPositions);
    }

    [Fact]
    public void DeviceSends1Location_1LocationIsParsed_7()
    {
        ProtocolTester.SendHexFromDevice(
            "40501E58003301E000014104D8F19682525ECD5D525EE344525EE35EFFC88815026AB4D70000020000104403DE01000B0000000007D007D000");

        Assert.Single(ProtocolTester.TotalParsedPositions);
    }
        
    [Fact]
    public void DeviceSends2Locations_2LocationAreParsed_1()
    {
        ProtocolTester.SendHexFromDevice(
            "4050B63B02C401AF000144A77A21281D5B79D8EF5B79D8EF5B79D8F0FAB84831029F35580056020003144D00080100130000000007D007D00025434925434525434E25475125475325464C254D4C25564E25504425464325454C254554254344254154254D46254D5625425625434D25445425474C25474E254756254C43254D4525524C25525025534125534D255452254941254D5000000000000004BBF41F0900003254314B5233324532384337303631383500000000000053005F3839303132363038383132313532343737353900000000FD078E0085002900011A2E3DA1882700687474703A2F2F6D6170732E676F6F676C652E636F6D2F6D6170733F713D34332E3938383331322C2D38382E35383631383920000013FF4E04190023FF13041100A3FFDC0402009AFFDE03FC00A4FFE3040C0093FFAB03EE004DFFBB04130012FF7A04180010FF6E04100037FF4D0402FFD8000C04140000000144A77A21281D0009470C00131B2B005B79D8F05B79D8F05B79D8F0FAB8488E029F356F0043020003144D00080100170000000007D007D00025434925434525434E25475125475325464C254D4C25564E25504425464325454C254554254344254154254D46254D5625425625434D25445425474C25474E254756254C43254D4525524C25525025534125534D255452254941254D5000000000000004BBF41F0900003254314B5233324532384337303631383500000000000052005F3839303132363038383132313532343737353900000000FD09190085002900011A2E3DA1882700687474703A2F2F6D6170732E676F6F676C652E636F6D2F6D6170733F713D34332E3938383333352C2D38382E35383630393820000013FF4E04190023FF1304110017FF0C0424000CFF30041A00A4FFE3040C0093FFAB03EE004DFFBB04130012FF7A04180010FF6E04100037FF4D0402FFD8000C04140000000144A77A21281D0009470C00171C2B00");

        Assert.Equal(2, ProtocolTester.TotalParsedPositions.Count);
    }

    [Fact]
    public void DeviceSends2Locations_2LocationAreParsed_2()
    {
        ProtocolTester.SendHexFromDevice(
            "405025e30096eb730001452efaf6a7d6562fe4f8562fe4f7562fe52c02a006d902273f810064650000e0f5000a0100000000000007d007d000254349255341254d5625425625475125415400090083002a1a000001a8562fe4f8562fe4f7562fe52c02a006d902273f810064020000e0f5000a0100000000000007d007d000254349255341254d5625425625475125415400090083002a1a000001a8");

        Assert.Equal(2, ProtocolTester.TotalParsedPositions.Count);
    }
}
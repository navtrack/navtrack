using Navtrack.Listener.Protocols.Teltonika;
using Xunit;

namespace Navtrack.Listener.Tests.Protocols.Teltonika;

public class TeltonikaProtocolCodec8Tests : BaseProtocolTests<TeltonikaProtocol, TeltonikaMessageHandler>
{
    [Fact]
    public void Codec8_DeviceSendImeiV1_ServerReturnsAcknowledge()
    {
        ProtocolTester.SendHexFromDevice("000F333536333037303432343431303133");

        Assert.Equal("01", ProtocolTester.ReceiveHexInDevice());
    }

    [Fact]
    public void Codec8_DeviceSendImeiV2_ServerReturnsAcknowledge()
    {
        ProtocolTester.SendHexFromDevice("000F333532383438303236333839393631");

        Assert.Equal("01", ProtocolTester.ReceiveHexInDevice());
    }
        
    [Fact]
    public void Codec8_DeviceSendsLocationV1_ServerConfirmsDataReceived()
    {
        ProtocolTester.SendHexFromDevice("000F333536333037303432343431303133");

        Assert.Equal("01", ProtocolTester.ReceiveHexInDevice());
        Assert.NotNull(ProtocolTester.Client.Device);

        ProtocolTester.SendHexFromDevice(
            "000000000000003608010000016B40D8EA30010000000000000000000000000000000105021503010101425E0F01F10000601A014E0000000000000000010000C7CF");

        Assert.Equal("00000001", ProtocolTester.ReceiveHexInDevice());
        Assert.Single(ProtocolTester.LastParsedLocations);
    }
        
    [Fact]
    public void Codec8_DeviceSendsLocationV2_ServerConfirmsDataReceived()
    {
        ProtocolTester.SendHexFromDevice("000F333536333037303432343431303133");

        Assert.Equal("01", ProtocolTester.ReceiveHexInDevice());
        Assert.NotNull(ProtocolTester.Client.Device);

        ProtocolTester.SendHexFromDevice(
            "000000000000002808010000016B40D9AD80010000000000000000000000000000000103021503010101425E100000010000F22A");

        Assert.Equal("00000001", ProtocolTester.ReceiveHexInDevice());
        Assert.Single(ProtocolTester.LastParsedLocations);
    }

    [Fact]
    public void Codec8_DeviceSendsLocationV3_ServerConfirmsDataReceived()
    {
        ProtocolTester.SendHexFromDevice("000F333536333037303432343431303133");

        Assert.Equal("01", ProtocolTester.ReceiveHexInDevice());
        Assert.NotNull(ProtocolTester.Client.Device);

        ProtocolTester.SendHexFromDevice(
            "000000000000004308020000016B40D57B480100000000000000000000000000000001010101000000000000016B40D5C198010000000000000000000000000000000101010101000000020000252C");

        Assert.Equal("00000002", ProtocolTester.ReceiveHexInDevice());
        Assert.Equal(2, ProtocolTester.LastParsedLocations.Count);
    }
        
    [Fact]
    public void Codec8_DeviceSendsLocationV4_ServerConfirmsDataReceived()
    {
        ProtocolTester.SendHexFromDevice("000F333532383438303236333839393631");

        Assert.Equal("01", ProtocolTester.ReceiveHexInDevice());
        Assert.NotNull(ProtocolTester.Client.Device);

        ProtocolTester.SendHexFromDevice(
            "00000000000003E108120000011733FAD2C80000000000000000000000000000000000080301014501F00003B60000422F5343004002C700000000F100005852000000011733FAAB2C0000000000000000000000000000000000080301014501F00003B60000422F7A43004602C700000000F100005852000000011733FA83720000000000000000000000000000000000080301014501F00003B60000422F6E43003D02C700000000F100005852000000011733FA5BD60000000000000000000000000000000000080301014501F00003B60000422F7843004902C700000000F100005852000000011733FA343A0000000000000000000000000000000000080301014501F00003B60000422F6D43004D02C700000000F100000000000000011733FA0C940000000000000000000000000000000000080301014501F00003B60000422F7643004302C700000000F100000000000000011733F9E4EE0000000000000000000000000000000000080301014501F00003B60000422F7B43004202C700000000F100000000000000011733F9BD520000000000000000000000000000000000080301014501F00003B60000422F7E43004602C700000000F100000000000000011733F995990000000000000000000000000000000000080301014501F00003B60000422F7543004102C700000000F100005852000000011733F96DE80000000000000000000000000000000000080301014501F00003B60000422F6743003E02C700000000F100005852000000011733F9464C0000000000000000000000000000000000080301014501F00003B60000422F6E43004A02C700000000F100005852000000011733F91E9C0000000000000000000000000000000000080301014501F00003B60000422F6943004602C700000000F100000000000000011733F8F6F60000000000000000000000000000000000080301014501F00003B60000422F7643004602C700000000F100000000000000011733F8CF5A0000000000000000000000000000000000080301014501F00003B60000422F6443004902C700000000F100000000000000011733F8A7B40000000000000000000000000000000000080301014501F00003B60000422F7B43004102C700000000F100000000000000011733F8800E0000000000000000000000000000000000080301014501F00003B60000422F6F43005002C700000000F100005852000000011733F8586A0000000000000000000000000000000000080301014501F00003B60000422F4F43004002C700000000F100005852000000011733F830C20000000000000000000000000000000000080301014501F00003B60000422F7643004102C700000000F100005852001200004EC8");

        Assert.Equal("00000012", ProtocolTester.ReceiveHexInDevice());
        Assert.Equal(18, ProtocolTester.LastParsedLocations.Count);
    }
}
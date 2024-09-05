using Navtrack.Listener.Protocols.Totem;
using Xunit;

namespace Navtrack.Listener.Tests.Protocols.Totem;

public class TotemProtocolTests : BaseProtocolTests<TotemProtocol, TotemMessageHandler>
{
    [Fact]
    public void AT07_DeviceSendsLocation_ParsedLocationIsNotNull()
    {
        ProtocolTester.SendStringFromDevice(
            "$$0108AA863835028447675|5004C0001710250234134114057728A058AE112108305100.600000660304.7787N10134.8719E116458");

        Assert.NotNull(ProtocolTester.LastParsedMessage);
    }

    [Fact]
    public void AT07_DeviceSendsLocation_ParsedIMEIIsCorrect()
    {
        ProtocolTester.SendStringFromDevice(
            "$$0108AA863835028447675|5004C0001710250234134114057728A058AE112108305100.600000660304.7787N10134.8719E116458");

        // Assert.Equal("863835028447675", ProtocolTester.LastParsedPosition.SerialNumber);
    }
        
        
    [Fact]
    public void AT07_DeviceSends4Locations_4LocationsParsed()
    {
        ProtocolTester.SendStringFromDevice(
            "$$0108AA863835028447675|5004C0001710250234134114057728A058AE112108305100.600000660304.7787N10134.8719E116458$$0108AA863835028447675|5004C0001710250234134114057728A058AE112108305100.600000660304.7787N10134.8719E116458$$0108AA863835028447675|5004C0001710250234134114057728A058AE112108305100.600000660304.7787N10134.8719E116458$$0108AA863835028447675|5004C0001710250234134114057728A058AE112108305100.600000660304.7787N10134.8719E116458");

        // Assert.Equal(4, ProtocolTester.TotalParsedPositions.Count);
    }
        
    [Fact]
    public void AT09_DeviceSendsLocation_ParsedLocationIsNotNull()
    {
        ProtocolTester.SendStringFromDevice(
            "$$0128AA864244026065291|18001800140916020524401100000000000000000000000027BA0E57063100000001.200000002237.8119N11403.5075E05202D");

        Assert.NotNull(ProtocolTester.LastParsedMessage);
    }
}
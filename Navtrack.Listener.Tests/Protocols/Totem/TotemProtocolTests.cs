using Navtrack.Listener.Protocols.Totem;
using NUnit.Framework;

namespace Navtrack.Listener.Tests.Protocols.Totem
{
    public class TotemProtocolTests
    {
        private IProtocolTester protocolTester;

        [SetUp]
        public void Setup()
        {
            protocolTester = new ProtocolTester(new TotemProtocol(), new TotemMessageHandler());
        }

        [Test]
        public void AT07_DeviceSendsLocation_ParsedLocationIsNotNull()
        {
            protocolTester.SendStringFromDevice(
                "$$0108AA863835028447675|5004C0001710250234134114057728A058AE112108305100.600000660304.7787N10134.8719E116458");

            Assert.IsNotNull(protocolTester.LastParsedLocation);
        }

        [Test]
        public void AT07_DeviceSendsLocation_ParsedIMEIIsCorrect()
        {
            protocolTester.SendStringFromDevice(
                "$$0108AA863835028447675|5004C0001710250234134114057728A058AE112108305100.600000660304.7787N10134.8719E116458");

            Assert.AreEqual("863835028447675", protocolTester.LastParsedLocation.Device.IMEI);
        }
        
        
        [Test]
        public void AT07_DeviceSends4Locations_4LocationsParsed()
        {
            protocolTester.SendStringFromDevice(
                "$$0108AA863835028447675|5004C0001710250234134114057728A058AE112108305100.600000660304.7787N10134.8719E116458$$0108AA863835028447675|5004C0001710250234134114057728A058AE112108305100.600000660304.7787N10134.8719E116458$$0108AA863835028447675|5004C0001710250234134114057728A058AE112108305100.600000660304.7787N10134.8719E116458$$0108AA863835028447675|5004C0001710250234134114057728A058AE112108305100.600000660304.7787N10134.8719E116458");

            Assert.AreEqual(4, protocolTester.TotalParsedLocations.Count);
        }
        
        [Test]
        public void AT09_DeviceSendsLocation_ParsedLocationIsNotNull()
        {
            protocolTester.SendStringFromDevice(
                "$$0128AA864244026065291|18001800140916020524401100000000000000000000000027BA0E57063100000001.200000002237.8119N11403.5075E05202D");

            Assert.IsNotNull(protocolTester.LastParsedLocation);
        }
    }
}
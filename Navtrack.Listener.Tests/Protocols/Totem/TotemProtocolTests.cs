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
        public void DeviceSendsLocation_ParsedLocationIsNotNull()
        {
            protocolTester.SendStringFromDevice(
                "$$0108AA863835028447675|5004C0001710250234134114057728A058AE112108305100.600000660304.7787N10134.8719E116458");

            Assert.IsNotNull(protocolTester.ParsedLocation);
        }

        [Test]
        public void DeviceSendsLocation_ParsedIMEIIsCorrect()
        {
            protocolTester.SendStringFromDevice(
                "$$0108AA863835028447675|5004C0001710250234134114057728A058AE112108305100.600000660304.7787N10134.8719E116458");

            Assert.AreEqual("863835028447675", protocolTester.ParsedLocation.Device.IMEI);
        }
    }
}
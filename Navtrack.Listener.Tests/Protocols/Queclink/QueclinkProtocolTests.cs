using Navtrack.Listener.Protocols.Queclink;
using NUnit.Framework;

namespace Navtrack.Listener.Tests.Protocols.Queclink
{
    public class QueclinkProtocolTests
    {
        private IProtocolTester protocolTester;

        [SetUp]
        public void Setup()
        {
            protocolTester = new ProtocolTester(new QueclinkProtocol(), new QueclinkMessageHandler());
        }

        [Test]
        public void DeviceSendsLocationV1_ParsedLocationIsNotNull()
        {
            protocolTester.SendStringFromDevice("+RESP:GTMON,060100,135790246811220,,+8613812341234,15,0,0,4.3,92,70.0,121.354335,31.222073,20090214013254,0460,0000,18d8,6141,00,20090214093254,11F0$");

            Assert.IsNotNull(protocolTester.LastParsedLocation);
        }
        
        [Test]
        public void DeviceSendsLocationV2_ParsedLocationIsNotNull()
        {
            protocolTester.SendStringFromDevice("+RESP:GTMPN,060100,135790246811220,,0,4.3,92,70.0,121.354335,31.222073,20090214013254,0460,0000,18d8,6141,00,20090214093254,11F0$");

            Assert.IsNotNull(protocolTester.LastParsedLocation);
        }
        
        [Test]
        public void DeviceSendsLocationV3_ParsedLocationIsNotNull()
        {
            protocolTester.SendStringFromDevice("+RESP:GTBTC,060100,135790246811220,,0,4.3,92,70.0,121.354335,31.222073,20090214013254,0460,0000,18d8,6141,00,20090214093254,11F0$");

            Assert.IsNotNull(protocolTester.LastParsedLocation);
        }
    }
}
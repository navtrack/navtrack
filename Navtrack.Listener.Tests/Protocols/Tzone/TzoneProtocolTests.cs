using Navtrack.Listener.Protocols.Tzone;
using NUnit.Framework;

namespace Navtrack.Listener.Tests.Protocols.Tzone
{
    public class TzoneProtocolTests
    {
        private IProtocolTester protocolTester;

        [SetUp]
        public void Setup()
        {
            protocolTester = new ProtocolTester(new TzoneProtocol(), new TzoneMessageHandler());
        }

        [Test]
        public void DeviceSendsLocationV1_ParsedLocationIsNotNull()
        {
            protocolTester.SendStringFromDevice("$$B0353358019462410|AA$GPRMC,102156.000,A,2232.4690,N,11403.6847,E,0.00,,180909,,*15|02.0|01.2|01.6|000000001010|20090918102156|14181353|00000000|279311AA|0000|0.7614|0080|D2B5\r\n");

            Assert.IsNotNull(protocolTester.LastParsedLocation);
        }
        
        [Test]
        public void DeviceSendsLocationV2_ParsedLocationIsNotNull()
        {
            protocolTester.SendStringFromDevice("$$A7355296038400938|AA$GPRMC,055605.000,A,2232.5946,N,11403.9026,E,0.00,,140710,,*18|02.5|02.3|01.0|000000000000|20100714055605|03780000|25337837|0000|0.0000|0205|D410\r\n");

            Assert.IsNotNull(protocolTester.LastParsedLocation);
        }
        
        [Test]
        public void DeviceSends3Locations_3LocationsParsed()
        {
            protocolTester.SendStringFromDevice("$$B0353358019462410|AA$GPRMC,102156.000,A,2232.4690,N,11403.6847,E,0.00,,180909,,*15|02.0|01.2|01.6|000000001010|20090918102156|14181353|00000000|279311AA|0000|0.7614|0080|D2B5\r\n$$B0353358019462410|AA$GPRMC,102156.000,A,2232.4690,N,11403.6847,E,0.00,,180909,,*15|02.0|01.2|01.6|000000001010|20090918102156|14181353|00000000|279311AA|0000|0.7614|0080|D2B5\r\n$$B0353358019462410|AA$GPRMC,102156.000,A,2232.4690,N,11403.6847,E,0.00,,180909,,*15|02.0|01.2|01.6|000000001010|20090918102156|14181353|00000000|279311AA|0000|0.7614|0080|D2B5\r\n");

            Assert.AreEqual(3, protocolTester.TotalParsedLocations.Count);
        }
    }
}
using Navtrack.Listener.Protocols.Carscop;
using NUnit.Framework;

namespace Navtrack.Listener.Tests.Protocols.Carscop
{
    // CC828, CC830, CC630 GPS Communication Protocol
    public class CarscopProtocolTests
    {
        private IProtocolTester protocolTester;

        [SetUp]
        public void Setup()
        {
            protocolTester = new ProtocolTester(new CarscopProtocol(), new CarscopMessageHandler());
        }

        [Test]
        public void DeviceSendsLocationV1_LocationIsParsed()
        {
            protocolTester.SendStringFromDevice("*HQ,1400046168,V1,055600,A,2234.3066,N,11351.6829,E,000.0,000,080813,FFFFFBFF#");

            Assert.IsNotNull(protocolTester.LastParsedLocation);
        }
        
        [Test]
        public void DeviceSendsLocationV2_LocationIsParsed()
        {
            protocolTester.SendStringFromDevice("*HQ,1400046168,V1,055600,V,2234.3066,N,11351.6829,E,000.0,000,080813,FFFFFFFE#");

            Assert.IsNotNull(protocolTester.LastParsedLocation);
        }
    }
}
using Navtrack.Listener.Protocols.LKGPS;
using NUnit.Framework;

namespace Navtrack.Listener.Tests.Protocols.LKGPS
{
    public class SinoTrackProtocolTests
    {
        private IProtocolTester protocolTester;

        [SetUp]
        public void Setup()
        {
            protocolTester = new ProtocolTester(new LKGPSProtocol(), new LKGPSMessageHandler());
        }

        [Test]
        public void DeviceSendsLocationV1_LocationIsParsed()
        {
            protocolTester.SendStringFromDevice("*TH,2020916012,V1,050316,A,2212.8745,N,11346.6574,E,14.28,028,220902,FFFFFBFF#");

            Assert.IsNotNull(protocolTester.LastParsedLocation);
        }
        
        [Test]
        public void DeviceSendsLocationV2_LocationIsParsed()
        {
            protocolTester.SendStringFromDevice("*HQ,4103000861,V1,092853,A,2234.2029,N,11351.4197,E,000.40,000,270215,FFFFFBFF,460,00,0,0,6#");

            Assert.IsNotNull(protocolTester.LastParsedLocation);
        }
    }
}
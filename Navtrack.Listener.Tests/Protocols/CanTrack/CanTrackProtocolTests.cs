using Navtrack.Listener.Protocols.CanTrack;
using NUnit.Framework;

namespace Navtrack.Listener.Tests.Protocols.CanTrack
{
    public class SinoTrackProtocolTests
    {
        private IProtocolTester protocolTester;

        [SetUp]
        public void Setup()
        {
            protocolTester = new ProtocolTester(new CanTrackProtocol(), new CanTrackMessageHandler());
        }

        [Test]
        public void DeviceSendsLocationV1_LocationIsParsed()
        {
            protocolTester.SendStringFromDevice("*HQ,865205030330012,V1,145452,A,2240.55181,N,11358.32389,E,0.00,0,100815,FFFFFBFF#");

            Assert.IsNotNull(protocolTester.LastParsedLocation);
        }
        
        [Test]
        public void DeviceSendsLocationV2_LocationIsParsed()
        {
            protocolTester.SendStringFromDevice("*HQ,865205030330012,V2,150421,A,2240.55841,N,11358.33462,E,2.06,0,100815,FFFFFBFF#");

            Assert.IsNotNull(protocolTester.LastParsedLocation);
        }
        
        [Test]
        public void DeviceSendsLocationV4_LocationIsParsed()
        {
            protocolTester.SendStringFromDevice("*HQ,865205030330012,V4,S2,150950,151007,A,2240.55503,N,11358.35174,E,0.85,0,100815,FFFFFBFF#");

            Assert.IsNotNull(protocolTester.LastParsedLocation);
        }
    }
}
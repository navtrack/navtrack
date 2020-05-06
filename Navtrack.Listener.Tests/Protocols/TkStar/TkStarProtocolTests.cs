using Navtrack.Listener.Protocols.TkStar;
using NUnit.Framework;

namespace Navtrack.Listener.Tests.Protocols.TkStar
{
    public class TkStarProtocolTests
    {
        private IProtocolTester protocolTester;

        [SetUp]
        public void Setup()
        {
            protocolTester = new ProtocolTester(new TkStarProtocol(), new TkStarMessageHandler());
        }

        [Test]
        public void DeviceSendsLocationV1_LocationIsParsed()
        {
            protocolTester.SendStringFromDevice("*HQ,4106016320,V1,090458,A,5257.4318,N,15840.4221,E,000.00,000,101115,FFFFFBFF,250,01,0,0,5#");

            Assert.IsNotNull(protocolTester.LastParsedLocation);
        }
    }
}
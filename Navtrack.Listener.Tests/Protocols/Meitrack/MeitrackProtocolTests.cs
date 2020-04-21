using Navtrack.Listener.Protocols.Meitrack;
using NUnit.Framework;

namespace Navtrack.Listener.Tests.Protocols.Meitrack
{
    public class MeitrackProtocolTests
    {
        private IProtocolTester protocolTester;

        [SetUp]
        public void Setup()
        {
            protocolTester = new ProtocolTester(new MeitrackProtocol(), new MeitrackMessageHandler());
        }

        [Test]
        public void DeviceSendsLocation1_LocationIsParsed()
        {
            protocolTester.SendStringFromDevice(
                "$$F142,123456789012345,AAA,35,48.858837,2.277019,160221001509,A,5,30,0,147,2.5,475,56364283,8983665,123|4|0000|0000,0421,0200|000E||02EF|00FC,*7E");

            Assert.IsNotNull(protocolTester.ParsedLocation);
        }

        [Test]
        public void DeviceSendsLocation2_LocationIsParsed()
        {
            protocolTester.SendStringFromDevice(
                "$$W129,353358017784062,AAA,35,22.540113,114.076141,100313094354,A,5,22,1,174,4,129,0,435,0|0|10133|4110,0000,166|224|193|2704|916,*BE\r\n");

            Assert.IsNotNull(protocolTester.ParsedLocation);
        }
    }
}
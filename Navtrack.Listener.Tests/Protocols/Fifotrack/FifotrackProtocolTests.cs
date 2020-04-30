using Navtrack.Listener.Protocols.Fifotrack;
using NUnit.Framework;

namespace Navtrack.Listener.Tests.Protocols.Fifotrack
{
    public class FifotrackProtocolTests
    {
        private IProtocolTester protocolTester;

        [SetUp]
        public void Setup()
        {
            protocolTester = new ProtocolTester(new FifotrackProtocol(), new FifotrackMessageHandler());
        }

        [Test]
        public void DeviceSendsLocationV1_LocationIsParsed()
        {
            protocolTester.SendStringFromDevice(
                "$$135,866104023192332,29,A01,,160606093046,A,22.546430,114.079730,0,186,181,0,415322,0000,02,2,460|0|27B3|EA7,A2F|3B9|3|0,940C7E,31.76|30.98*46\r\n");

            Assert.IsNotNull(protocolTester.LastParsedLocation);
        }
    }
}
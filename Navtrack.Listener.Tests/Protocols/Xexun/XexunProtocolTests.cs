using Navtrack.Listener.Protocols.Xexun;
using NUnit.Framework;

namespace Navtrack.Listener.Tests.Protocols.Xexun
{
    public class XexunProtocolTests
    {
        private IProtocolTester protocolTester;

        [SetUp]
        public void Setup()
        {
            protocolTester = new ProtocolTester(new XexunProtocol(), new XexunMessageHandler());
        }

        [Test]
        public void DeviceSendsLocationV1_LocationIsParsed()
        {
            protocolTester.SendStringFromDevice("090805215127,+22663173122,GPRMC,215127.083,A,4717.3044,N,01135.0005,E,0.39,217.95,050809,,,A*6D,F,, imei:354776030393299,05,552.4,F:4.06V,0,141,54982,232,01,1A30,0949");

            Assert.IsNotNull(protocolTester.LastParsedLocation);
        }
    }
}
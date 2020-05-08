using Navtrack.Listener.Protocols.Coban;
using NUnit.Framework;

namespace Navtrack.Listener.Tests.Protocols.Coban
{
    public class CobanProtocolTests : BaseProtocolTests<CobanProtocol, CobanMessageHandler>
    {
        [Test]
        public void DeviceSendsIMEI_ServerRepliesWithAcknowledge()
        {
            protocolTester.SendStringFromDevice("##,imei:359586015829802,A;");
            
            Assert.AreEqual("LOAD", protocolTester.ReceiveStringInDevice());
        }
        
        [Test]
        public void DeviceSendsHeartbeat_ServerRepliesWithAcknowledge()
        {
            protocolTester.SendStringFromDevice("359586015829802");

            Assert.AreEqual("ON", protocolTester.ReceiveStringInDevice());
        }
        
        [Test]
        public void DeviceSendsLocationV1_LocationIsParsed()
        {
            protocolTester.SendStringFromDevice("imei:359587010124900,tracker,0809231929,13554900601,F,112909.397,A,2234.4669,N,11354.3287,E,0.11,;");

            Assert.IsNotNull(protocolTester.LastParsedLocation);
        }
    }
}
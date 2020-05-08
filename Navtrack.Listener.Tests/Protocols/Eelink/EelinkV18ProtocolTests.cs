using Navtrack.Listener.Protocols.Eelink;
using NUnit.Framework;

namespace Navtrack.Listener.Tests.Protocols.Eelink
{
    public class EelinkV18ProtocolTests : BaseProtocolTests<EelinkProtocol, EelinkMessageHandler>
    {
        [Test]
        public void DeviceSendsLoginPackage_ServerAcknowledges()
        {
            protocolTester.SendHexFromDevice("676701000C000101234567890123450120");

            Assert.AreEqual("67670100020001", protocolTester.ReceiveHexInDevice());
        }

        [Test]
        public void DeviceSendsHeartbeatPackage_ServerAcknowledges()
        {
            protocolTester.SendHexFromDevice("6767030004001A0001");

            Assert.AreEqual("6767030002001A", protocolTester.ReceiveHexInDevice());
        }

        [Test]
        public void DeviceSendsLocationPackage_LocationIsParsed()
        {
            protocolTester.SendHexFromDevice("676702001C02B259AE7387FCD360D6034332B2000000028F000A4F64002EB10101");

            Assert.IsNotNull(protocolTester.LastParsedLocation);
        }

        [Test]
        public void DeviceSendsWarningPackage_ServerAcknowledges()
        {
            protocolTester.SendHexFromDevice("676704001C00B7569FC3020517A2D7020B08E100000000D8001E005B0004460004");

            Assert.AreEqual("676704000200B7", protocolTester.ReceiveHexInDevice());
            Assert.IsNotNull(protocolTester.LastParsedLocation);
        }

        [Test]
        public void DeviceSendsReportPackage_ServerAcknowledges()
        {
            protocolTester.SendHexFromDevice(
                "676705002102B459AE7388FCD360D7034332B1000000028F000A4F64002EB101010159AE7388");

            Assert.AreEqual("676705000202B4", protocolTester.ReceiveHexInDevice());
        }
    }
}
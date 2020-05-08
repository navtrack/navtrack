using Navtrack.Listener.Protocols.Eelink;
using NUnit.Framework;

namespace Navtrack.Listener.Tests.Protocols.Eelink
{
    public class EelinkV18ProtocolTests
    {
        private IProtocolTester protocolTester;

        [SetUp]
        public void Setup()
        {
            protocolTester = new ProtocolTester(new EelinkProtocol(), new EelinkMessageHandler());
        }

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
            protocolTester.SendHexFromDevice("676705002102B459AE7388FCD360D7034332B1000000028F000A4F64002EB101010159AE7388");
        
            Assert.AreEqual("676705000202B4", protocolTester.ReceiveHexInDevice());
        }
        
        // [Test]
        // public void DeviceSendsMessagePackage_ServerAcknowledges()
        // {
        //     protocolTester.SendHexFromDevice("6767160039000D590BD5AF03026B940D0C3952AD0021000000000501CC0001A53F0170F0AB17323031383536363232313235300000000000000000313233");
        //
        //     Assert.AreEqual("6767160017000D323031383536363232313235300000000000000000", protocolTester.ReceiveHexInDevice());
        // }
        //
        // [Test]
        // public void DeviceSendsParamSetPackage_ServerAcknowledges()
        // {
        //     protocolTester.SendHexFromDevice("67671B009E000500010432009266DF000008053FC0A20341303EFE8110D414404C0680185610CEF3A23C8C18154005AB64300BD0AAA845755C0CE331CF0C1B036478B843D0EA288988320B42D068956405053C11A4588FA38803FD599EC6EF4B7383D0FC3FB7333919EA637F3D8EFB1D79F9D27B8D7782191146AE344DC0766F01599EE898BBE5ED3217444DBECA0AB4BADA4B08224A48F235D59759EDEB2A24EE9C20");
        //
        //     Assert.AreEqual("67671B0003000500", protocolTester.ReceiveHexInDevice());
        // }
    }
}
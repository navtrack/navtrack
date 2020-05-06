using Navtrack.Listener.Protocols.Smartrack;
using NUnit.Framework;

namespace Navtrack.Listener.Tests.Protocols.Smartrack
{
    public class MeiligaoProtocolTests
    {
        private IProtocolTester protocolTester;

        [SetUp]
        public void Setup()
        {
            protocolTester = new ProtocolTester(new SmartrackProtocol(), new SmartrackMessageHandler());
        }

        [Test]
        public void DeviceSendsLoginV1_ServerRespondsWithLoginConfirmation()
        {
            protocolTester.SendStringFromDevice(
                "(080524101241BP05000013632782450080524A2232.9806N11404.9355E000.1101241323.8700000000L000450AC)");

            Assert.AreEqual("(080524101241AP05)", protocolTester.ReceiveStringInDevice());
        }

        [Test]
        public void DeviceSendsLoginV2_ServerRespondsWithLoginConfirmation()
        {
            protocolTester.SendStringFromDevice("(040331141830BP00000013612345678HSO)");

            Assert.AreEqual("(040331141830AP01HSO)", protocolTester.ReceiveStringInDevice());
        }

        [Test]
        public void DeviceSendsLocation_LocationIsParsed()
        {
            protocolTester.SendStringFromDevice(
                "(080612022828BR00080612A2232.9828N11404.9297E000.0022828000.0000000000L000230AA)");

            Assert.IsNotNull(protocolTester.LastParsedLocation);
        }
    }
}
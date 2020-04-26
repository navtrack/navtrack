using System;
using Navtrack.Listener.Protocols.Teltonika;
using NUnit.Framework;

namespace Navtrack.Listener.Tests.Protocols.Teltonika
{
    public class TeltonikaProtocolCodec8ExtendedTests
    {
        private IProtocolTester protocolTester;

        [SetUp]
        public void Setup()
        {
            protocolTester = new ProtocolTester(new TeltonikaProtocol(), new TeltonikaMessageHandler());
        }

        [Test]
        public void Codec8Extended_DeviceSendImei_ServerReturnsAcknowledge()
        {
            protocolTester.SendHexFromDevice("000F333536333037303432343431303133");

            Assert.AreEqual("01", protocolTester.ReceiveInDevice());
        }

        [Test]
        public void Codec8Extended_DeviceSendsLocationV1_ServerConfirmsDataReceived()
        {
            protocolTester.SendHexFromDevice("000F333536333037303432343431303133");

            Assert.AreEqual("01", protocolTester.ReceiveInDevice());
            Assert.IsNotNull(protocolTester.Client.Device);

            protocolTester.SendHexFromDevice(
                "000000000000004A8E010000016B412CEE000100000000000000000000000000000000010005000100010100010011001D00010010015E2C880002000B000000003544C87A000E000000001DD7E06A00000100002994");

            Assert.AreEqual("00000001", protocolTester.ReceiveInDevice());
            Assert.AreEqual(1, protocolTester.LastParsedLocations.Count);
        }
        
        [Test]
        public void Codec8Extended_DeviceSendsLocationV2_ServerConfirmsDataReceived()
        {
            protocolTester.SendHexFromDevice("000F333536333037303432343431303133");

            Assert.AreEqual("01", protocolTester.ReceiveInDevice());
            Assert.IsNotNull(protocolTester.Client.Device);

            protocolTester.SendHexFromDevice(
                "000000000000002808010000016B40D9AD80010000000000000000000000000000000103021503010101425E100000010000F22A");

            Assert.AreEqual("00000001", protocolTester.ReceiveInDevice());
            Assert.AreEqual(1, protocolTester.LastParsedLocations.Count);
        }
    }
}
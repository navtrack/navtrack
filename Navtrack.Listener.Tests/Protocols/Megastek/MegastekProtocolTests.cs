using Navtrack.Listener.Protocols.Megastek;
using NUnit.Framework;

namespace Navtrack.Listener.Tests.Protocols.Megastek
{
    public class MegastekProtocolTests
    {
        private IProtocolTester protocolTester;

        [SetUp]
        public void Setup()
        {
            protocolTester = new ProtocolTester(new MegastekProtocol(), new MegastekMessageHandler());
        }

        [Test]
        public void DeviceSendsLocation_ParsedLocationIsNotNull()
        {
            protocolTester.SendStringFromDevice(
                "$MGV002,860719020193193,DeviceName,R,240214,104742,A,2238.20471,N,11401.97967,E,00,03,00,1.20,0.462,356.23,137.9,1.5,460,07,262C,0F54,25,0000,0000,0,0,0,28.5,28.3,,10,100,Timer;!");

            Assert.IsNotNull(protocolTester.ParsedLocation);
        }

        [Test]
        public void DeviceSendsLocation_ParsedIMEIIsCorrect()
        {
            protocolTester.SendStringFromDevice(
                "$MGV002,860719020193193,DeviceName,R,240214,104742,A,2238.20471,N,11401.97967,E,00,03,00,1.20,0.462,356.23,137.9,1.5,460,07,262C,0F54,25,0000,0000,0,0,0,28.5,28.3,,10,100,Timer;!");

            Assert.AreEqual("860719020193193", protocolTester.ParsedLocation.Device.IMEI);
        }
    }
}
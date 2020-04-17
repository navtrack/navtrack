using Navtrack.Listener.Helpers;
using Navtrack.Listener.Protocols.Meiligao;
using NUnit.Framework;

namespace Navtrack.Listener.Tests.Protocols.Meiligao
{
    public class MeiligaoMessageTests
    {
        private const string LoginMessageHex = "2424001113612345678fff500005d80d0a";
        private const string LocationMessageHex =
            "24 24 00 60 12 34 56 FF FF FF FF 99 55 30 33 35 36 34 34 2E 30 30 30 2C 41 2C 32 32 33 32 2E 36 30 38 33 2C 4E 2C 31 31 34 30 34 2E 38 31 33 37 2C 45 2C 30 2E 30 30 2C 2C 30 31 30 38 30 39 2C 2C 2A 31 43 7C 31 31 2E 35 7C 31 39 34 7C 30 30 30 30 7C 30 30 30 30 2C 30 30 30 30 69 62 0D 0A";
        
        private MeiligaoMessage loginMessage;
        private MeiligaoMessage locationMessage;
        
        [SetUp]
        public void Setup()
        {
            loginMessage = new MeiligaoMessage(HexUtil.ConvertHexStringToIntArray(LoginMessageHex));
            locationMessage = new MeiligaoMessage(HexUtil.ConvertHexStringToIntArray(LocationMessageHex.Replace(" ", "")));
        }

        [Test]
        public void LoginMessageHasValidChecksum()
        {
            Assert.IsTrue(loginMessage.ChecksumValid);
        }
        
        [Test]
        public void LocationMessageHasValidChecksum()
        {
            Assert.IsTrue(locationMessage.ChecksumValid);
        }
    }
}
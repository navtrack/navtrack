using Navtrack.Listener.Protocols.Teltonika;
using NUnit.Framework;

namespace Navtrack.Listener.Tests.Protocols.Teltonika
{
    public class TeltonikaLocationParserTests
    {
        private ITeltonikaLocationParser teltonikaLocationParser;
        
        [SetUp]
        public void Setup()
        {
            teltonikaLocationParser = new TeltonikaLocationParser();
        }

        [Test]
        public void ImeiIsParsedCorrectly()
        {
            byte[][] m = TeltonikaTestData.GetData();

            string imei = teltonikaLocationParser.GetIMEI(m[0]);
            
            Assert.AreEqual("352848026421822", imei);
        }
    }
}
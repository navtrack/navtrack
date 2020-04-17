using Navtrack.Listener.Models;
using Navtrack.Listener.Protocols.Meitrack;
using NUnit.Framework;

namespace Navtrack.Listener.Tests.Protocols.Meitrack
{
    public class MeitrackLocationParserTests
    {
        private MeitrackLocationParser meitrackLocationParser;
        private const string Message = "$$W129,353358017784062,AAA,35,22.540113,114.076141,100313094354,A,5,22,1,174,4,129,0,435,0|0|10133|4110,0000,166|224|193|2704|916,*BE\r\n";
        private Location<MeitrackData> result;
        
        [SetUp]
        public void Setup()
        {
            meitrackLocationParser = new MeitrackLocationParser();
            result = meitrackLocationParser.Parse(Message);
        }

        [Test]
        public void GpsStatusIsParsedCorrectly()
        {                     
            Assert.AreEqual("A", result.Data.GPSStatus);
        }
        
        [Test]
        public void AltitudeIsParsedCorrectly()
        {                     
            Assert.AreEqual(475, result.Altitude);
        }
    }
}
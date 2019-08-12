using Navtrack.Common.Model;
using Navtrack.Listener.Protocols.Meitrack;
using NUnit.Framework;

namespace Navtrack.Listener.Tests.Protocols.Meitrack
{
    public class MeitrackLocationParserTests
    {
        private MeitrackLocationParser meitrackLocationParser;
        private const string Message = "$$F142,123456789012345,AAA,35,48.858837,2.277019,160221001509,A,5,30,0,147,2.5,475,56364283,8983665,123|4|0000|0000,0421,0200|000E||02EF|00FC,*7E";
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
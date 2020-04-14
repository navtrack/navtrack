using Navtrack.Listener.Helpers;
using Navtrack.Listener.Models;
using Navtrack.Listener.Protocols.Meiligao;
using NUnit.Framework;

namespace Navtrack.Listener.Tests.Protocols.Meiligao
{
    public class MeiligaoLocationParserTests
    {
        private const string HexStringMessage =
            "24240060123456FFFFFFFF99553033353634342E3030302C412C323233322E363038332C4E2C31313430342E383133372C452C302E30302C2C3031303830392C2C2A31437C31312E357C3139347C303030307C303030302C3030303069620D0A";

        private string message;
        private MeiligaoLocationParser meitrackLocationParser;
        
        private Location<MeiligaoData> result;

        [SetUp]
        public void Setup()
        {
            message = HexUtil.ConvertHexStringToString(HexStringMessage);
            meitrackLocationParser = new MeiligaoLocationParser();

            result = meitrackLocationParser.Parse(message);
        }

        [Test]
        public void ParsingIsNotImplemented()
        {
            Assert.IsNull(result);
        }
    }
}
using Navtrack.Library.DI;
using Navtrack.Listener.Models;

namespace Navtrack.Listener.Protocols.Meiligao
{
    [Service(typeof(IMeiligaoLocationParser))]
    public class MeiligaoLocationParser : IMeiligaoLocationParser
    {
        public Location<MeiligaoData> Parse(string input)
        {
            MeiligaoMessage message = new MeiligaoMessage(input);
            
            if (message.HasValidChecksum)
            {
                // TODO parse data from message
            }

            return null;
        }
    }
}
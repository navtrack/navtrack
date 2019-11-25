using Navtrack.Listener.Models;

namespace Navtrack.Listener.Protocols.Meitrack
{
    public interface IMeitrackLocationParser
    {
        Location<MeitrackData> Parse(string input);
    }
}
using Navtrack.Listener.Models;

namespace Navtrack.Listener.Protocols.Meiligao
{
    public interface IMeiligaoLocationParser
    {
        Location<MeiligaoData> Parse(string input);
    }
}
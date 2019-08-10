namespace Navtrack.Listener.Protocols.Meitrack
{
    public interface IMeitrackLocationParser
    {
        MeitrackLocation Parse(string input);
    }
}
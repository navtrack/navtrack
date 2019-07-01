namespace Navtrack.Listener.Services.Protocols.Meitrack
{
    public interface IMeitrackLocationParser
    {
        MeitrackLocation Parse(string input);
    }
}
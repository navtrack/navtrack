namespace Navtrack.Listener.Protocols.Meiligao
{
    public interface IMeiligaoMessageHandler
    {
        MeiligaoCommand HandleMessage(MeiligaoMessage message);
    }
}
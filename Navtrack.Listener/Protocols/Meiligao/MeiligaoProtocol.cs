using Navtrack.Library.DI;
using Navtrack.Listener.Server;

namespace Navtrack.Listener.Protocols.Meiligao
{
    [Service(typeof(IProtocol))]
    public class MeiligaoProtocol : IProtocol
    {
        public int Port => 7003;
        public bool DetectNewLine => true;
    }
}
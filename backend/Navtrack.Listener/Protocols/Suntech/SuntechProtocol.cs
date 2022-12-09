using Navtrack.Library.DI;
using Navtrack.Listener.Server;

namespace Navtrack.Listener.Protocols.Suntech;

[Service(typeof(IProtocol))]
public class SuntechProtocol : BaseProtocol
{
    public override int Port => 7010;
    public override string SplitMessageBy => ";";
}
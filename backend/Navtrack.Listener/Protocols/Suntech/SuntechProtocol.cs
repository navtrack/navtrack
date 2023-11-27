using Navtrack.Listener.Server;
using Navtrack.Shared.Library.DI;

namespace Navtrack.Listener.Protocols.Suntech;

[Service(typeof(IProtocol))]
public class SuntechProtocol : BaseProtocol
{
    public override int Port => 7010;
    public override string SplitMessageBy => ";";
}
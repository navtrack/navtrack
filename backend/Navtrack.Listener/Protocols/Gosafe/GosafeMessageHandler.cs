using Navtrack.Listener.Server;
using Navtrack.Shared.Library.DI;

namespace Navtrack.Listener.Protocols.Gosafe;

[Service(typeof(ICustomMessageHandler<GosafeProtocol>))]
public class GosafeMessageHandler : BaseGosafeMessageHandler<GosafeProtocol>;
using Navtrack.Api.Services.Protocols;
using Navtrack.Api.Shared.Controllers;

namespace Navtrack.Api.Controllers;

public class ProtocolsController : ProtocolsControllerBase
{
    public ProtocolsController(IProtocolService protocolService) : base(protocolService)
    {
    }
}
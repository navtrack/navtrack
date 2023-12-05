using Navtrack.Api.Services.Protocols;
using Navtrack.Api.Shared.Controllers;

namespace Navtrack.Api.Controllers;

public class ProtocolsController(IProtocolService protocolService) : ProtocolsControllerBase(protocolService);
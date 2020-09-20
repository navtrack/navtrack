using System.Collections.Generic;
using Navtrack.Api.Model.Protocols;

namespace Navtrack.Api.Services.Protocols
{
    public interface IProtocolService
    {
        List<ProtocolModel> GetProtocols();
    }
}
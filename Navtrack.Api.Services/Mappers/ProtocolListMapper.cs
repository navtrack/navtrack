using System.Collections.Generic;
using System.Linq;
using Navtrack.Api.Model.Protocols;
using Navtrack.DataAccess.Model.Protocols;

namespace Navtrack.Api.Services.Mappers;

public static class ProtocolListMapper
{
    public static ProtocolListModel Map(IEnumerable<Protocol> protocols)
    {
        ProtocolListModel listModel = new()
        {
            Items = protocols.Select(ProtocolModelMapper.Map).ToList()
        };

        return listModel;
    }
}
using System.Collections.Generic;
using System.Linq;
using Navtrack.Api.Model.Common;
using Navtrack.Api.Model.Protocols;
using Navtrack.DataAccess.Model.Protocols;

namespace Navtrack.Api.Services.Protocols.Mappers;

public static class ProtocolListMapper
{
    public static ListModel<ProtocolModel> Map(IEnumerable<Protocol> protocols)
    {
        ListModel<ProtocolModel> listModel = new()
        {
            Items = protocols.Select(ProtocolModelMapper.Map).ToList()
        };

        return listModel;
    }
}
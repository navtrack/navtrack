using System.Collections.Generic;
using System.Linq;
using Navtrack.Api.Model.Locations;
using Navtrack.DataAccess.Model.Positions;

namespace Navtrack.Api.Services.Mappers.Positions;

public static class PositionListMapper
{
    public static PositionListModel Map(IEnumerable<PositionElement> source)
    {
        PositionListModel listModel = new()
        {
            Items = source.Select(PositionMapper.Map).ToList()
        };

        return listModel;
    }
}
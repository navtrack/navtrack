using System.Collections.Generic;
using System.Linq;
using Navtrack.Api.Model.Locations;
using Navtrack.DataAccess.Model.New;

namespace Navtrack.Api.Services.Mappers.Locations;

public static class LocationListMapper
{
    public static PositionListModel Map(IEnumerable<PositionElement> source)
    {
        PositionListModel listModel = new()
        {
            Items = source.Select(LocationMapper.Map).ToList()
        };

        return listModel;
    }
}
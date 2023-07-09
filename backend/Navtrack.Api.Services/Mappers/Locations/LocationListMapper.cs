using System.Collections.Generic;
using System.Linq;
using Navtrack.Api.Model.Locations;
using Navtrack.DataAccess.Model.Locations;

namespace Navtrack.Api.Services.Mappers.Locations;

public static class LocationListMapper
{
    public static LocationListModel Map(IEnumerable<LocationDocument> source)
    {
        LocationListModel listModel = new()
        {
            Items = source.Select(LocationMapper.Map).ToList()
        };

        return listModel;
    }
}
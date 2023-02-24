using System.Collections.Generic;
using System.Linq;
using Navtrack.Api.Model.Locations;
using Navtrack.Core.Model;
using Navtrack.DataAccess.Model.Common;
using Navtrack.DataAccess.Model.Locations;

namespace Navtrack.Api.Services.Mappers;

public static class LocationListMapper
{
    public static LocationListModel Map(IEnumerable<LocationDocument> source, UnitsType unitsType)
    {
        LocationListModel listModel = new()
        {
            Items = source.Select(source1 => LocationMapper.Map(source1, unitsType)).ToList()
        };

        return listModel;
    }
}
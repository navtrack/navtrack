using System.Collections.Generic;
using System.Linq;
using Navtrack.Api.Model.Trips;
using Navtrack.Core.Model.Trips;

namespace Navtrack.Api.Services.Mappers.Trips;

public static class TripListMapper
{
    public static TripListModel Map(IEnumerable<Trip> source)
    {
        return new TripListModel
        {
            Items = source.Select(TripModelMapper.Map).ToList()
        };
    }
}
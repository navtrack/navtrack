using System.Collections.Generic;
using Navtrack.Api.Model.Trips;

namespace Navtrack.Api.Services.Trips.Mappers;

public static class TripListMapper
{
    public static TripList Map(IEnumerable<Trip> source)
    {
        return new TripList
        {
            Items = source
        };
    }
}
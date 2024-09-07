using System.Collections.Generic;
using Navtrack.Api.Model.Trips;

namespace Navtrack.Api.Services.Trips.Mappers;

public static class TripListMapper
{
    public static TripListModel Map(IEnumerable<TripModel> source)
    {
        return new TripListModel
        {
            Items = source
        };
    }
}
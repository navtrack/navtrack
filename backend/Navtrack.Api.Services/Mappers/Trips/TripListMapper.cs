using System.Collections.Generic;
using Navtrack.Api.Model.Trips;

namespace Navtrack.Api.Services.Mappers.Trips;

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
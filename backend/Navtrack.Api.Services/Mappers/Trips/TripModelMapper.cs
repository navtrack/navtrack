using System.Linq;
using Navtrack.Api.Model.Trips;
using Navtrack.Api.Services.Mappers.Locations;
using Navtrack.Core.Model.Trips;

namespace Navtrack.Api.Services.Mappers.Trips;

public static class TripModelMapper
{
    public static TripModel Map(Trip source)
    {
        return new TripModel
        {
            Locations = source.Locations.Select(LocationMapper.Map).ToList(),
            Distance = source.Distance,
            Duration = source.Duration,
            MaxSpeed = source.MaxSpeed,
            AverageSpeed = source.AverageSpeed,
            AverageAltitude = source.AverageAltitude
        };
    }
}
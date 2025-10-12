using System.Collections.Generic;
using System.Linq;
using Navtrack.Api.Model.Trips;
using Navtrack.Api.Services.DeviceMessages.Mappers;
using Navtrack.Api.Services.Trips.Models;

namespace Navtrack.Api.Services.Trips.Mappers;

public static class TripListMapper
{
    public static TripList Map(List<InternalTrip> source)
    {
        return new TripList
        {
            Items = source.Select(x => new Trip
            {
                Distance = x.Distance,
                FuelConsumption = x.FuelConsumption,
                Positions = x.Messages.Select(MessagePositionMapper.Map).ToList()
            })
        };
    }
}
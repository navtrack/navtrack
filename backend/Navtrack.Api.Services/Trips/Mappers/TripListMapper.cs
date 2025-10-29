using System.Collections.Generic;
using System.Linq;
using Navtrack.Api.Model.Trips;
using Navtrack.Api.Services.DeviceMessages.Mappers;
using Navtrack.Api.Services.Trips.Models;

namespace Navtrack.Api.Services.Trips.Mappers;

public static class TripListMapper
{
    public static TripListModel Map(List<InternalTrip> source)
    {
        return new TripListModel
        {
            Items = source.Select(x => new TripModel
            {
                Distance = x.Distance,
                FuelConsumption = x.FuelConsumption,
                Positions = x.Messages.Select(MessagePositionMapper.Map).ToList()
            })
        };
    }
}
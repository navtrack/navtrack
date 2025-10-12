using System;
using System.Collections.Generic;
using System.Linq;
using Navtrack.Database.Model.Devices;

namespace Navtrack.Api.Services.Trips.Models;

public class InternalTrip
{
    public List<DeviceMessageEntity> Messages { get; set; } = [];
    public int Distance { get; set; }

    public DeviceMessageEntity StartMessage => Messages.First();

    public DeviceMessageEntity EndMessage => Messages.Last();
    public double Duration => Math.Ceiling((EndMessage.Date - StartMessage.Date).TotalMinutes);

    public double? MaxSpeed => Messages.Max(x => x.Speed);

    public double? FuelConsumption
    {
        get
        {
            DeviceMessageEntity? first = Messages.FirstOrDefault(x => x.VehicleFuelConsumption > 0);
            DeviceMessageEntity? last = Messages.LastOrDefault(x => x.VehicleFuelConsumption > 0);


            return last?.VehicleFuelConsumption - first?.VehicleFuelConsumption;
        }
    }

    public short? AverageSpeed
    {
        get
        {
            List<DeviceMessageEntity> messages = Messages.Where(x => x.Speed > 0).ToList();

            double? average = messages.Count != 0 ? messages.Average(x => x.Speed) : null;

            return average.HasValue ? (short?)Math.Round(average.Value) : null;
        }
    }

    public short? AverageAltitude
    {
        get
        {
            double? average = Messages.Average(x => x.Altitude);

            return average.HasValue ? (short?)Math.Round(average.Value) : null;
        }
    }
}
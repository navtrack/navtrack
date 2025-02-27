using System;
using System.Collections.Generic;
using System.Linq;
using Navtrack.DataAccess.Model.Devices.Messages;

namespace Navtrack.Api.Services.Trips.Models;

public class InternalTrip
{
    public List<DeviceMessageDocument> Messages { get; set; } = [];
    public int Distance { get; set; }

    public DeviceMessageDocument StartMessage => Messages.First();

    public DeviceMessageDocument EndMessage => Messages.Last();
    public double Duration => Math.Ceiling((EndMessage.Position.Date - StartMessage.Position.Date).TotalMinutes);

    public double? MaxSpeed => Messages.Max(x => x.Position.Speed);

    public double? FuelConsumption
    {
        get
        {
            DeviceMessageDocument? first = Messages.FirstOrDefault(x => x.Vehicle?.FuelConsumption is > 0);
            DeviceMessageDocument? last = Messages.LastOrDefault(x => x.Vehicle?.FuelConsumption is > 0);


            return last?.Vehicle?.FuelConsumption - first?.Vehicle?.FuelConsumption;
        }
    }

    public float? AverageSpeed
    {
        get
        {
            List<DeviceMessageDocument> messages = Messages.Where(x => x.Position.Speed > 0).ToList();

            double? average = messages.Count != 0 ? messages.Average(x => x.Position.Speed) : null;

            return average.HasValue ? (float?)Math.Round(average.Value) : null;
        }
    }

    public float? AverageAltitude
    {
        get
        {
            double? average = Messages.Average(x => x.Position.Altitude);

            return average.HasValue ? (float?)Math.Round(average.Value) : null;
        }
    }
}
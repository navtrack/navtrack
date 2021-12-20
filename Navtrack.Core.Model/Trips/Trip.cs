using System;
using System.Collections.Generic;
using System.Linq;
using Navtrack.DataAccess.Model.Locations;

namespace Navtrack.Core.Model.Trips;

public class Trip
{
    public List<LocationDocument> Locations { get; set; }

    public LocationDocument StartLocation => Locations.FirstOrDefault();

    public LocationDocument EndLocation => Locations.LastOrDefault();

    public double Duration => Math.Ceiling((EndLocation.DateTime - StartLocation.DateTime).TotalMinutes);

    public int Distance { get; set; }

    public float? MaxSpeed => Locations.Max(x => x.Speed);

    public float? AverageSpeed
    {
        get
        {
            float? average = Locations.Where(x => x.Speed > 0).Average(x => x.Speed);

            return average.HasValue ? (float?)Math.Round(average.Value) : null;
        }
    }

    public float? AverageAltitude
    {
        get
        {
            float? average = Locations.Average(x => x.Altitude);

            return average.HasValue ? (float?)Math.Round(average.Value) : null;
        }
    }
}
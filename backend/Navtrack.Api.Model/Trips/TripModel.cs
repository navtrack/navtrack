using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Navtrack.Api.Model.Locations;

namespace Navtrack.Api.Model.Trips;

public class TripModel
{
    [Required]
    public List<PositionModel> Locations { get; } = [];    

    [Required]
    public PositionModel StartPosition => Locations.First();

    [Required]
    // ReSharper disable once MemberCanBePrivate.Global
    public PositionModel EndPosition => Locations.Last();

    [Required]
    public double Duration => Math.Ceiling((EndPosition.DateTime - StartPosition.DateTime).TotalMinutes);

    [Required]
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
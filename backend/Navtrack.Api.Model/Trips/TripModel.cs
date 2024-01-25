using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Navtrack.Api.Model.Positions;

namespace Navtrack.Api.Model.Trips;

public class TripModel
{
    [Required]
    public List<PositionModel> Positions { get; } = [];    

    [Required]
    public PositionModel StartPosition => Positions.First();

    [Required]
    // ReSharper disable once MemberCanBePrivate.Global
    public PositionModel EndPosition => Positions.Last();

    [Required]
    public double Duration => Math.Ceiling((EndPosition.DateTime - StartPosition.DateTime).TotalMinutes);

    [Required]
    public int Distance { get; set; }

    public float? MaxSpeed => Positions.Max(x => x.Speed);

    public float? AverageSpeed
    {
        get
        {
            float? average = Positions.Where(x => x.Speed > 0).Average(x => x.Speed);

            return average.HasValue ? (float?)Math.Round(average.Value) : null;
        }
    }

    public float? AverageAltitude
    {
        get
        {
            float? average = Positions.Average(x => x.Altitude);

            return average.HasValue ? (float?)Math.Round(average.Value) : null;
        }
    }
}
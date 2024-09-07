using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Navtrack.Api.Model.Messages;

namespace Navtrack.Api.Model.Trips;

public class TripModel
{
    [Required]
    public List<MessagePositionModel> Positions { get; set; } = [];   
 
    [Required]
    public MessagePositionModel StartPosition => Positions.First();

    [Required]
    public MessagePositionModel EndPosition => Positions.Last();

    [Required]
    public double Duration => Math.Ceiling((EndPosition.Date - StartPosition.Date).TotalMinutes);

    [Required]
    public int Distance { get; set; }

    public double? MaxSpeed => Positions.Max(x => x.Speed);

    public float? AverageSpeed
    {
        get
        {
            double? average = Positions.Where(x => x.Speed > 0).Average(x => x.Speed);

            return average.HasValue ? (float?)Math.Round(average.Value) : null;
        }
    }

    public float? AverageAltitude
    {
        get
        {
            double? average = Positions.Average(x => x.Altitude);

            return average.HasValue ? (float?)Math.Round(average.Value) : null;
        }
    }
}
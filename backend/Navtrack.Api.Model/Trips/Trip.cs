using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Navtrack.Api.Model.Messages;

namespace Navtrack.Api.Model.Trips;

public class Trip
{
    [Required]
    public List<MessagePosition> Positions { get; set; } = [];   
 
    [Required]
    public MessagePosition StartPosition => Positions.First();

    [Required]
    public MessagePosition EndPosition => Positions.Last();

    [Required]
    public double Duration => Math.Ceiling((EndPosition.Date - StartPosition.Date).TotalMinutes);

    [Required]
    public int Distance { get; set; }

    public double? FuelConsumption { get; set; }

    public double? MaxSpeed => Positions.Max(x => x.Speed);

    public float? AverageSpeed
    {
        get
        {
            List<MessagePosition> positions = Positions.Where(x => x.Speed > 0).ToList();
            
            double? average = positions.Count != 0 ? positions.Average(x => x.Speed) : null;

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
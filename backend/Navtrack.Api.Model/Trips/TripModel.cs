using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Navtrack.Api.Model.Messages;

namespace Navtrack.Api.Model.Trips;

public class TripModel
{
    [Required]
    public List<PositionDataModel> Positions { get; set; } = [];   
 
    [Required]
    public PositionDataModel StartPosition => Positions.First();

    [Required]
    public PositionDataModel EndPosition => Positions.Last();

    [Required]
    public double Duration => Math.Ceiling((EndPosition.Date - StartPosition.Date).TotalSeconds);

    [Required]
    public int Distance { get; set; }

    public double? FuelConsumption { get; set; }

    public double MaxSpeed => Positions.Max(x => x.Speed);
    
    public double MaxAltitude => Positions.Max(x => x.Altitude);

    public double AverageSpeed
    {
        get
        {
            List<PositionDataModel> positions = Positions.Where(x => x.Speed > 0).ToList();
            
            double? average = positions.Count != 0 ? positions.Average(x => x.Speed) : null;

            return average.HasValue ? Math.Round(average.Value) : 0;
        }
    }

    public double AverageAltitude => Positions.Average(x => x.Altitude);

    public double? AverageFuelConsumption => FuelConsumption * 100 / Distance * 1000;
}
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Navtrack.Api.Model.Common;

namespace Navtrack.Api.Model.Trips;

public class TripList : ListModel<Trip>
{
    [Required]
    public int TotalDistance => Items.Sum(x => x.Distance);

    [Required]
    public double TotalDuration => Items.Sum(x => x.Duration);

    [Required]
    public double TotalPositions => Items.Sum(x => x.Positions.Count);

    [Required]
    public float? AvgSpeed => Items.Average(x => x.AverageSpeed);
        
    [Required]
    public float? AvgAltitude => Items.Average(x => x.AverageAltitude);
    
    [Required]
    public double? MaxSpeed => Items.Max(x => x.MaxSpeed);
}
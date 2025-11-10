using System.ComponentModel.DataAnnotations;
using System.Linq;
using Navtrack.Api.Model.Common;

namespace Navtrack.Api.Model.Trips;

public class TripListModel : ListModel<TripModel>
{
    [Required]
    public int TotalDistance => Items.Sum(x => x.Distance);

    [Required]
    public double TotalDuration => Items.Sum(x => x.Duration);

    [Required]
    public double TotalPositions => Items.Sum(x => x.Positions.Count);

    [Required]
    public double? AverageSpeed => Items.Any() ? Items.Average(x => x.AverageSpeed) : null;
    
    [Required]
    public double? MaxSpeed => Items.Any() ? Items.Max(x => x.MaxSpeed) : null;

    [Required]
    public double? AverageAltitude => Items.Any() ? Items.Average(x => x.AverageAltitude) : null;
    
    [Required]
    public double? MaxAltitude => Items.Any() ? Items.Max(x => x.MaxAltitude) : null;
    
    public double? FuelConsumption => Items.Any() ? Items.Sum(x => x.FuelConsumption) : null;
    
    public double? AverageFuelConsumption => Items.Any() ? Items.Average(x => x.AverageFuelConsumption) : null;
}
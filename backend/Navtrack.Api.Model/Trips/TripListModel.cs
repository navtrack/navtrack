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
    public float? TotalAvgSpeed => Items.Average(x => x.AverageSpeed);
        
    [Required]
    public float? TotalAvgAltitude => Items.Average(x => x.AverageAltitude);
}
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Navtrack.Api.Model.Locations;

namespace Navtrack.Api.Model.Trips;

public class TripModel
{
    [Required]
    public LocationModel StartLocation => Locations.First();

    [Required]
    public LocationModel EndLocation => Locations.Last();

    [Required]
    public List<LocationModel> Locations { get; set; }

    [Required]
    public int Distance { get; set; }

    [Required]
    public double Duration { get; set; }

    public float? MaxSpeed { get; set; }

    public float? AverageSpeed { get; set; }

    public float? AverageAltitude { get; set; }
}
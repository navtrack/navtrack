namespace Navtrack.Api.Model.Trips;

public class TripFilterModel : DateFilterModel
{
    public int? MinAvgAltitude { get; set; }
    public int? MaxAvgAltitude { get; set; }
    public int? MinAvgSpeed { get; set; }
    public int? MaxAvgSpeed { get; set; }
    public int? MinDuration { get; set; }
    public int? MaxDuration { get; set; }
    public double? Latitude { get; set; }
    public double? Longitude { get; set; }
    public int? Radius { get; set; }
}
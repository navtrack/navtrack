using Navtrack.DataAccess.Model.Locations;

namespace Navtrack.Core.Model.Trips;

public class TripFilter : DateFilter
{
    public virtual int? MinAvgAltitude { get; set; }
    public virtual int? MaxAvgAltitude { get; set; }
    public virtual int? MinAvgSpeed { get; set; }
    public virtual int? MaxAvgSpeed { get; set; }
    public virtual int? MinDuration { get; set; }
    public virtual int? MaxDuration { get; set; }
    public virtual double? Latitude { get; set; }
    public virtual double? Longitude { get; set; }
    public virtual int? Radius { get; set; }
}
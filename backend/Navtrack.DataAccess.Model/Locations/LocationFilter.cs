namespace Navtrack.DataAccess.Model.Locations;

public class LocationFilter : DateFilter
{
    public virtual int? MinAltitude { get; set; }
    public virtual int? MaxAltitude { get; set; }
    public virtual int? MinSpeed { get; set; }
    public virtual int? MaxSpeed { get; set; }
    public virtual double? Latitude { get; set; }
    public virtual double? Longitude { get; set; }
    public virtual int? Radius { get; set; }
}
using System;
using Navtrack.Database.Model.Filters;

namespace Navtrack.Api.Model.Messages;

public class MessageFilterModel : PositionFilterModel
{
    public override DateTime? StartDate { get; set; }
    public override DateTime? EndDate { get; set; }
    public override int? MinAltitude { get; set; }
    public override int? MaxAltitude { get; set; }
    public override int? MinSpeed { get; set; }
    public override int? MaxSpeed { get; set; }
    public override double? Latitude { get; set; }
    public override double? Longitude { get; set; }
    public override int? Radius { get; set; }
}
using System;
using Navtrack.DataAccess.Model.Devices.Messages;

namespace Navtrack.Api.Model.Positions;

public class PositionFilterModel : PositionFilter
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
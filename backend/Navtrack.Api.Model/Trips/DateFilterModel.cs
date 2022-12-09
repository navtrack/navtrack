using System;
using System.Text.Json.Serialization;
using Navtrack.DataAccess.Model.Locations;

namespace Navtrack.Api.Model.Trips;

public class DateFilterModel : DateFilter
{
    [JsonPropertyName("startDate")]
    public override DateTime? StartDate { get; set; }

    [JsonPropertyName("endDate")]
    public override DateTime? EndDate { get; set; }
}
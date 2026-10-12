using System;
using Navtrack.Api.Model.Trips;

namespace Navtrack.Api.Services.Trips;

public class GetAssetTripsRequest
{
    public Guid AssetId { get; set; }
    public TripFilterModel Filter { get; set; }
}
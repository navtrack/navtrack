using Navtrack.Api.Model.Trips;

namespace Navtrack.Api.Services.Trips;

public class GetAssetTripsRequest
{
    public string AssetId { get; set; }
    public TripFilter Filter { get; set; }
}
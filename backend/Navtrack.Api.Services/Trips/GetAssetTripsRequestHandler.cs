using System.Threading.Tasks;
using Navtrack.Api.Model.Trips;
using Navtrack.Api.Services.Requests;
using Navtrack.Shared.Library.DI;

namespace Navtrack.Api.Services.Trips;

[Service(typeof(IRequestHandler<GetAssetTripsRequest, TripList>))]
public class GetAssetTripsRequestHandler(ITripService tripService) 
    : BaseRequestHandler<GetAssetTripsRequest, TripList>
{
    public override async Task<TripList> Handle(GetAssetTripsRequest request)
    {
        TripList result = await tripService.GetTrips(request.AssetId, request.Filter);
        
        return result;
    }
}
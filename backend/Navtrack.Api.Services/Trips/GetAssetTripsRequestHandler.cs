using System.Threading.Tasks;
using Navtrack.Api.Model.Trips;
using Navtrack.Api.Services.Common.Exceptions;
using Navtrack.Api.Services.Common.RequestContext;
using Navtrack.Api.Services.Requests;
using Navtrack.Shared.Library.DI;

namespace Navtrack.Api.Services.Trips;

[Service(typeof(IRequestHandler<GetAssetTripsRequest, TripListModel>))]
public class GetAssetTripsRequestHandler(ITripService tripService, INavtrackRequestContextAccessor navtrackRequestContextAccessor) 
    : BaseRequestHandler<GetAssetTripsRequest, TripListModel>
{
    public override Task Validate(RequestValidationContext<GetAssetTripsRequest> context)
    {
        navtrackRequestContextAccessor.NavtrackContext?.Asset.Return404IfNull();
        
        return base.Validate(context);
    }
    
    public override async Task<TripListModel> Handle(GetAssetTripsRequest request)
    {
        TripListModel result = await tripService.GetTrips(navtrackRequestContextAccessor.NavtrackContext?.Asset!, request.Filter);
        
        return result;
    }
}
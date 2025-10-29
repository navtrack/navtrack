using System.Threading.Tasks;
using Navtrack.Api.Model.Trips;
using Navtrack.Api.Services.Common.Context;
using Navtrack.Api.Services.Common.Exceptions;
using Navtrack.Api.Services.Requests;
using Navtrack.Database.Model.Assets;
using Navtrack.Shared.Library.DI;

namespace Navtrack.Api.Services.Trips;

[Service(typeof(IRequestHandler<GetAssetTripsRequest, TripListModel>))]
public class GetAssetTripsRequestHandler(ITripService tripService, ICurrentContext currentContext) 
    : BaseRequestHandler<GetAssetTripsRequest, TripListModel>
{
    private AssetEntity? asset;

    public override async Task Validate(RequestValidationContext<GetAssetTripsRequest> context)
    {
        asset = await currentContext.GetCurrentAsset();
        asset.Return404IfNull();
    }
    
    public override async Task<TripListModel> Handle(GetAssetTripsRequest request)
    {
        TripListModel result = await tripService.GetTrips(asset!, request.Filter);
        
        return result;
    }
}
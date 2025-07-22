using System.Linq;
using System.Threading.Tasks;
using Navtrack.Api.Model.Reports;
using Navtrack.Api.Model.Trips;
using Navtrack.Api.Services.Common.Context;
using Navtrack.Api.Services.Common.Exceptions;
using Navtrack.Api.Services.Requests;
using Navtrack.Api.Services.Trips;
using Navtrack.DataAccess.Model.Assets;
using Navtrack.Shared.Library.DI;

namespace Navtrack.Api.Services.Reports;

[Service(typeof(IRequestHandler<GetTripReportRequest, TripReport>))]
public class GetTripReportRequestHandler(ITripService tripService, ICurrentContext currentContext) 
    : BaseRequestHandler<GetTripReportRequest, TripReport>
{
    private AssetDocument? asset;

    public override async Task Validate(RequestValidationContext<GetTripReportRequest> context)
    {
        asset = await currentContext.GetCurrentAsset();
        asset.Return404IfNull();
    }
    
    public override async Task<TripReport> Handle(GetTripReportRequest request)
    {
        TripList result = await tripService.GetTrips(asset!, new TripFilter
        {
            StartDate = request.Model.StartDate,
            EndDate = request.Model.EndDate
        });

        TripReport report = new()
        {
            Items = result.Items.Select(x => new TripReportItem
            {
                StartPosition = x.StartPosition,
                EndPosition = x.EndPosition,
                Distance = x.Distance,
                Duration = x.Duration,
                FuelConsumption = x.FuelConsumption,
                AverageFuelConsumption = x.AverageFuelConsumption,
                MaxSpeed = x.MaxSpeed,
                AverageSpeed = x.AverageSpeed
            }).ToList()
        };
        
        return report;
    }
}
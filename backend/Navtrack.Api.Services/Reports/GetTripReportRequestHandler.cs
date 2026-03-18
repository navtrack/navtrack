using System.Linq;
using System.Threading.Tasks;
using Navtrack.Api.Model.Reports;
using Navtrack.Api.Model.Trips;
using Navtrack.Api.Services.Common.Exceptions;
using Navtrack.Api.Services.Common.RequestContext;
using Navtrack.Api.Services.Requests;
using Navtrack.Api.Services.Trips;
using Navtrack.Shared.Library.DI;

namespace Navtrack.Api.Services.Reports;

[Service(typeof(IRequestHandler<GetTripReportRequest, TripReportModel>))]
public class GetTripReportRequestHandler(ITripService tripService, INavtrackRequestContextAccessor navtrackRequestContextAccessor) 
    : BaseRequestHandler<GetTripReportRequest, TripReportModel>
{
    public override Task Validate(RequestValidationContext<GetTripReportRequest> context)
    {
        navtrackRequestContextAccessor.NavtrackContext?.Asset.Return404IfNull();
        
        return base.Validate(context);
    }
    
    public override async Task<TripReportModel> Handle(GetTripReportRequest request)
    {
        TripListModel result = await tripService.GetTrips(navtrackRequestContextAccessor.NavtrackContext?.Asset!, new TripFilterModel
        {
            Date = request.Model.StartDate,
            // EndDate = request.Model.EndDate
        });

        TripReportModel report = new()
        {
            Items = result.Items.Select(x => new TripReportItemModel
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
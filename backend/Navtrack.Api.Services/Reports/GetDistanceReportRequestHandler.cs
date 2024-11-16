using System.Threading.Tasks;
using Navtrack.Api.Model.Reports;
using Navtrack.Api.Model.Trips;
using Navtrack.Api.Services.Common.Exceptions;
using Navtrack.Api.Services.Reports.Mappers;
using Navtrack.Api.Services.Requests;
using Navtrack.Api.Services.Trips;
using Navtrack.DataAccess.Model.Assets;
using Navtrack.DataAccess.Services.Assets;
using Navtrack.Shared.Library.DI;

namespace Navtrack.Api.Services.Reports;

[Service(typeof(IRequestHandler<GetDistanceReportRequest, DistanceReportList>))]
public class GetDistanceReportRequestHandler(IAssetRepository assetRepository, ITripService tripService)
    : BaseRequestHandler<GetDistanceReportRequest, DistanceReportList>
{
    public override async Task Validate(RequestValidationContext<GetDistanceReportRequest> context)
    {
        AssetDocument? asset = await assetRepository.GetById(context.Request.AssetId);
        asset.Return404IfNull();
    }

    public override async Task<DistanceReportList> Handle(GetDistanceReportRequest request)
    {
        TripList trips = await tripService.GetTrips(request.AssetId, new TripFilter
        {
            StartDate = request.Model.StartDate,
            EndDate = request.Model.EndDate
        });

        DistanceReportList result = DistanceReportListMapper.Map(trips.Items);

        return result;
    }
}
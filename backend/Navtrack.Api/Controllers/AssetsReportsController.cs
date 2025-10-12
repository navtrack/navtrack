using System.Threading.Tasks;
using IdentityServer4;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Navtrack.Api.Controllers.Shared;
using Navtrack.Api.Model.Reports;
using Navtrack.Api.Services.Common.ActionFilters;
using Navtrack.Api.Services.Reports;
using Navtrack.Api.Services.Requests;
using Navtrack.Database.Model.Assets;
using NSwag.Annotations;

namespace Navtrack.Api.Controllers;

[ApiController]
[Authorize(IdentityServerConstants.LocalApi.PolicyName)]
[OpenApiTag(ControllerTags.AssetsReports)]
public class AssetsReportsController(IRequestHandler requestHandler)
    : ControllerBase
{
    [HttpGet(ApiPaths.AssetReportsDistance)]
    [ProducesResponseType(typeof(DistanceReportModel), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [AuthorizeAsset(AssetUserRole.Viewer)]
    public async Task<DistanceReportModel> GetDistanceReport([FromRoute] string assetId,
        [FromQuery] BaseReportFilterModel filter)
    {
        DistanceReportModel result = await requestHandler.Handle<GetDistanceReportRequest, DistanceReportModel>(
            new GetDistanceReportRequest
            {
                AssetId = assetId,
                Model = filter
            });

        return result;
    }
    
    [HttpGet(ApiPaths.AssetReportsFuelConsumption)]
    [ProducesResponseType(typeof(FuelConsumptionReportModel), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [AuthorizeAsset(AssetUserRole.Viewer)]
    public async Task<FuelConsumptionReportModel> GetFuelConsumptionReport([FromRoute] string assetId,
        [FromQuery] BaseReportFilterModel filter)
    {
        FuelConsumptionReportModel result = await requestHandler.Handle<GetFuelConsumptionReportRequest, FuelConsumptionReportModel>(
            new GetFuelConsumptionReportRequest
            {
                AssetId = assetId,
                Model = filter
            });

        return result;
    }
    
    [HttpGet(ApiPaths.AssetReportsTrips)]
    [ProducesResponseType(typeof(TripReportModel), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [AuthorizeAsset(AssetUserRole.Viewer)]
    public async Task<TripReportModel> GetTripReport([FromRoute] string assetId,
        [FromQuery] BaseReportFilterModel filter)
    {
        TripReportModel result = await requestHandler.Handle<GetTripReportRequest, TripReportModel>(
            new GetTripReportRequest
            {
                AssetId = assetId,
                Model = filter
            });

        return result;
    }
}
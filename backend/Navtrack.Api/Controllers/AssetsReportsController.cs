using System.Threading.Tasks;
using Duende.IdentityServer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Navtrack.Api.Controllers.Shared;
using Navtrack.Api.Model.Reports;
using Navtrack.Api.Services.Common.ActionFilters;
using Navtrack.Api.Services.Reports;
using Navtrack.Api.Services.Requests;
using Navtrack.DataAccess.Model.Assets;
using NSwag.Annotations;

namespace Navtrack.Api.Controllers;

[ApiController]
[Authorize(IdentityServerConstants.LocalApi.PolicyName)]
[OpenApiTag(ControllerTags.AssetsReports)]
public class AssetsReportsController(IRequestHandler requestHandler)
    : ControllerBase
{
    [HttpGet(ApiPaths.AssetReportsTimeDistance)]
    [ProducesResponseType(typeof(DistanceReportList), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [AuthorizeAsset(AssetUserRole.Viewer)]
    public async Task<DistanceReportList> GetTimeDistanceReport([FromRoute] string assetId,
        [FromQuery] DistanceReportFilter filter)
    {
        DistanceReportList result = await requestHandler.Handle<GetDistanceReportRequest, DistanceReportList>(
            new GetDistanceReportRequest
            {
                AssetId = assetId,
                Model = filter
            });

        return result;
    }
}
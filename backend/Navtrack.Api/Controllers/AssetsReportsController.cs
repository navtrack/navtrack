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
using Navtrack.DataAccess.Model.Assets;
using NSwag.Annotations;

namespace Navtrack.Api.Controllers;

[ApiController]
[Authorize(IdentityServerConstants.LocalApi.PolicyName)]
[OpenApiTag(ControllerTags.AssetsReports)]
public class AssetsReportsController(IRequestHandler requestHandler)
    : ControllerBase
{
    [HttpGet(ApiPaths.AssetReportsDistance)]
    [ProducesResponseType(typeof(DistanceReport), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [AuthorizeAsset(AssetUserRole.Viewer)]
    public async Task<DistanceReport> GetDistanceReport([FromRoute] string assetId,
        [FromQuery] DistanceReportFilter filter)
    {
        DistanceReport result = await requestHandler.Handle<GetDistanceReportRequest, DistanceReport>(
            new GetDistanceReportRequest
            {
                AssetId = assetId,
                Model = filter
            });

        return result;
    }
}
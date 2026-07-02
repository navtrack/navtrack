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
    : NavtrackControllerBase(requestHandler)
{
    [HttpGet(ApiPaths.AssetReportsDistance)]
    [ProducesResponseType(typeof(DistanceReportModel), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [NavtrackAuthorize(AssetUserRole.Viewer)]
    public Task<DistanceReportModel> GetDistanceReport([FromRoute] string assetId,
        [FromQuery] BaseReportFilterModel filter) =>
        Query<GetDistanceReportRequest, DistanceReportModel>(new GetDistanceReportRequest
        {
            AssetId = assetId,
            Model = filter
        });
}

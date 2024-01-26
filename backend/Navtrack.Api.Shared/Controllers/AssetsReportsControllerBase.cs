using System.Threading.Tasks;
using IdentityServer4;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Navtrack.Api.Model;
using Navtrack.Api.Model.Reports;
using Navtrack.Api.Services.ActionFilters;
using Navtrack.Api.Services.Reports;
using Navtrack.DataAccess.Model.Assets;

namespace Navtrack.Api.Shared.Controllers;

[ApiController]
[Authorize(IdentityServerConstants.LocalApi.PolicyName)]
public abstract class AssetsReportsControllerBase(IReportService reportService) : ControllerBase
{
    [HttpGet(ApiPaths.AssetsAssetReportsTimeDistance)]
    [ProducesResponseType(typeof(DistanceReportListModel), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [AuthorizeAsset(AssetRoleType.Viewer)]
    public virtual async Task<JsonResult> GetTimeDistanceReport(
        [FromRoute] string assetId,
        [FromQuery] DistanceReportFilterModel filter)
    {
        DistanceReportListModel distanceReport = await reportService.GetDistanceReport(assetId, filter);

        return new JsonResult(distanceReport);
    }
}
using System.ComponentModel.DataAnnotations;
using System.Net.Mime;
using System.Threading.Tasks;
using IdentityServer4;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Navtrack.Api.Model.Assets;
using Navtrack.Api.Model.Common;
using Navtrack.Api.Model.Devices;
using Navtrack.Api.Model.Locations;
using Navtrack.Api.Model.Reports;
using Navtrack.Api.Model.Trips;
using Navtrack.Api.Services.ActionFilters;
using Navtrack.Api.Services.Assets;
using Navtrack.Api.Services.Devices;
using Navtrack.Api.Services.Locations;
using Navtrack.Api.Services.Reports;
using Navtrack.Api.Services.Trips;
using Navtrack.DataAccess.Model.Assets;

namespace Navtrack.Api.Controllers;

[ApiController]
[Route("assets")]
[Authorize(IdentityServerConstants.LocalApi.PolicyName)]
public class AssetsController : ControllerBase
{
    private readonly IAssetService assetService;
    private readonly ILocationService locationService;
    private readonly ITripService tripService;
    private readonly IReportService reportService;
    private readonly IDeviceService deviceService;

    public AssetsController(IAssetService assetService, ILocationService locationService, ITripService tripService,
        IReportService reportService, IDeviceService deviceService)
    {
        this.assetService = assetService;
        this.locationService = locationService;
        this.tripService = tripService;
        this.reportService = reportService;
        this.deviceService = deviceService;
    }

    [HttpGet]
    [ProducesResponseType(typeof(AssetsModel), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [Produces(MediaTypeNames.Application.Json)]
    public async Task<IActionResult> GetAssets()
    {
        AssetsModel assets = await assetService.GetAssets();

        return new JsonResult(assets);
    }

    [HttpGet("{assetId}")]
    [ProducesResponseType(typeof(AssetModel), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [Produces(MediaTypeNames.Application.Json)]
    [AuthorizeAsset(AssetRoleType.Viewer)]
    public async Task<AssetModel> GetAsset(string assetId)
    {
        AssetModel asset = await assetService.GetById(assetId);

        return asset;
    }

    [HttpPost]
    [ProducesResponseType(typeof(AssetModel), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status400BadRequest)]
    [Produces(MediaTypeNames.Application.Json)]
    public async Task<AssetModel> AddAsset([FromBody] AddAssetModel model)
    {
        AssetModel asset = await assetService.Add(model);

        return asset;
    }

    [HttpPatch("{assetId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [AuthorizeAsset(AssetRoleType.Owner)]
    public async Task<IActionResult> UpdateAsset(string assetId, [FromBody] UpdateAssetModel model)
    {
        await assetService.Update(assetId, model);

        return Ok();
    }

    [HttpDelete("{assetId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [AuthorizeAsset(AssetRoleType.Owner)]
    public async Task<IActionResult> DeleteAsset(string assetId)
    {
        await assetService.Delete(assetId);

        return Ok();
    }

    [HttpGet("{assetId}/locations")]
    [ProducesResponseType(typeof(LocationListModel), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [Authorize(IdentityServerConstants.LocalApi.PolicyName)]
    [AuthorizeAsset(AssetRoleType.Viewer)]
    public async Task<JsonResult> GetLocations(
        [FromRoute] string assetId,
        [FromQuery] LocationFilterModel filter,
        [FromQuery] int page = 0,
        [FromQuery] [Range(0, 1000)] int size = 1000)
    {
        LocationListModel locations = await locationService.GetLocations(assetId, filter, page, size);

        return new JsonResult(locations);
    }

    [HttpGet("{assetId}/users")]
    [ProducesResponseType(typeof(AssetUserListModel), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [AuthorizeAsset(AssetRoleType.Owner)]
    public async Task<JsonResult> GetAssetUsers([FromRoute] string assetId)
    {
        AssetUserListModel assetUserList = await assetService.GetAssetUsers(assetId);

        return new JsonResult(assetUserList);
    }

    [HttpPost("{assetId}/users")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
    [AuthorizeAsset(AssetRoleType.Owner)]
    public async Task<IActionResult> AddUserToAsset([FromRoute] string assetId, [FromBody] AddUserToAssetModel model)
    {
        await assetService.AddUserToAsset(assetId, model);

        return Ok();
    }

    [HttpDelete("{assetId}/users/{userId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
    [AuthorizeAsset(AssetRoleType.Owner)]
    public async Task<IActionResult> DeleteUserFromAsset([FromRoute] string assetId, [FromRoute] string userId)
    {
        await assetService.RemoveUserFromAsset(assetId, userId);

        return Ok();
    }

    [HttpGet("{assetId}/trips")]
    [ProducesResponseType(typeof(TripListModel), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [AuthorizeAsset(AssetRoleType.Viewer)]
    public async Task<JsonResult> GetTrips(
        [FromRoute] string assetId,
        [FromQuery] TripFilterModel filter)
    {
        TripListModel locations = await tripService.GetTrips(assetId, filter);

        return new JsonResult(locations);
    }

    [HttpGet("{assetId}/reports/time-distance")]
    [ProducesResponseType(typeof(TripListModel), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [AuthorizeAsset(AssetRoleType.Viewer)]
    public async Task<JsonResult> GetTimeDistanceReport(
        [FromRoute] string assetId,
        [FromQuery] DistanceReportFilterModel filter)
    {
        DistanceReportListModel distanceReport = await reportService.GetDistanceReport(assetId, filter);

        return new JsonResult(distanceReport);
    }

    [HttpGet("{assetId}/devices")]
    [ProducesResponseType(typeof(DevicesModel), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [Produces(MediaTypeNames.Application.Json)]
    [Authorize(IdentityServerConstants.LocalApi.PolicyName)]
    [AuthorizeAsset(AssetRoleType.Owner)]
    public async Task<JsonResult> GetDevices([FromRoute] string assetId)
    {
        DevicesModel model = await deviceService.Get(assetId);

        return new JsonResult(model);
    }

    [HttpPost("{assetId}/devices")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [Authorize(IdentityServerConstants.LocalApi.PolicyName)]
    [AuthorizeAsset(AssetRoleType.Owner)]
    public async Task<IActionResult> AddDevice([FromRoute] string assetId, [FromBody] AddDeviceModel model)
    {
        await deviceService.Add(assetId, model);

        return Ok();
    }

    [HttpDelete("{assetId}/devices/{deviceId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [Authorize(IdentityServerConstants.LocalApi.PolicyName)]
    [AuthorizeAsset(AssetRoleType.Owner)]
    public async Task<IActionResult> DeleteDevice([FromRoute] string assetId, [FromRoute] string deviceId)
    {
        await deviceService.Delete(deviceId);

        return Ok();
    }
}
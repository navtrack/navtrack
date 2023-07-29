using System.Threading.Tasks;
using IdentityServer4;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Navtrack.Api.Model.Assets;
using Navtrack.Api.Model.Common;
using Navtrack.Api.Services.ActionFilters;
using Navtrack.Api.Services.Assets;
using Navtrack.DataAccess.Model.Assets;

namespace Navtrack.Api.Shared.Controllers;

[ApiController]
[Authorize(IdentityServerConstants.LocalApi.PolicyName)]
public class AssetsUsersController : ControllerBase
{
    private readonly IAssetService assetService;

    public AssetsUsersController(IAssetService assetService)
    {
        this.assetService = assetService;
    }

    [HttpGet("assets/{assetId}/users")]
    [ProducesResponseType(typeof(AssetUserListModel), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [AuthorizeAsset(AssetRoleType.Owner)]
    public async Task<JsonResult> GetAssetUsers([FromRoute] string assetId)
    {
        AssetUserListModel assetUserList = await assetService.GetAssetUsers(assetId);

        return new JsonResult(assetUserList);
    }

    [HttpPost("assets/{assetId}/users")]
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

    [HttpDelete("assets/{assetId}/users/{userId}")]
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
}
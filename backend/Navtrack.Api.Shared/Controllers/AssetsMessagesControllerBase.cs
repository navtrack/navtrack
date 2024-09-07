using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using IdentityServer4;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Navtrack.Api.Model;
using Navtrack.Api.Model.Messages;
using Navtrack.Api.Services.ActionFilters;
using Navtrack.Api.Services.DeviceMessages;
using Navtrack.DataAccess.Model.Assets;

namespace Navtrack.Api.Shared.Controllers;

[ApiController]
[Authorize(IdentityServerConstants.LocalApi.PolicyName)]
public abstract class AssetsMessagesControllerBase(IDeviceMessageService service) : ControllerBase
{
    [HttpGet(ApiPaths.AssetsAssetMessages)]
    [ProducesResponseType(typeof(MessageListModel), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [Authorize(IdentityServerConstants.LocalApi.PolicyName)]
    [AuthorizeAsset(AssetRoleType.Viewer)]
    public virtual async Task<JsonResult> GetList(
        [FromRoute] string assetId,
        [FromQuery] MessageFilterModel filter,
        [FromQuery] int page = 0,
        [FromQuery] [Range(0, 10000)] int size = 10000)
    {
        MessageListModel messages = await service.GetPositions(assetId, filter, page, size);

        return new JsonResult(messages);
    }
}
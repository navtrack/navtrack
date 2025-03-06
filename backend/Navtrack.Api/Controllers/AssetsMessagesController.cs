using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using IdentityServer4;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Navtrack.Api.Controllers.Shared;
using Navtrack.Api.Model.Messages;
using Navtrack.Api.Services.Common.ActionFilters;
using Navtrack.Api.Services.DeviceMessages;
using Navtrack.Api.Services.Requests;
using Navtrack.DataAccess.Model.Assets;
using NSwag.Annotations;

namespace Navtrack.Api.Controllers;

[ApiController]
[Authorize(IdentityServerConstants.LocalApi.PolicyName)]
[OpenApiTag(ControllerTags.AssetsMessages)]
public class AssetsMessagesController(IRequestHandler requestHandler) : ControllerBase
{
    [HttpGet(ApiPaths.AssetMessages)]
    [ProducesResponseType(typeof(MessageList), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [AuthorizeAsset(AssetUserRole.Viewer)]
    public async Task<MessageList> GetList(
        [FromRoute] string assetId,
        [FromQuery] MessageFilter filter,
        [FromQuery] int page = 0,
        [FromQuery] [Range(0, 10000)] int size = 10000)
    {
        MessageList result = await requestHandler.Handle<GetAssetMessagesRequest, MessageList>(
            new GetAssetMessagesRequest
            {
                AssetId = assetId,
                Filter = filter,
                Page = page,
                Size = size
            });

        return result;
    }
}
